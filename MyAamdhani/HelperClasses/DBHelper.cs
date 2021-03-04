using MyAamdhani.Models;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MyAamdhani.HelperClasses
{
    public static class DBHelper
    {
        public static DbCommand LoadStoredProcedure(this MyAamdhaniEntities dbContext, string StoredProcedure)
        {
            try
            {
                var cmd = dbContext.Database.Connection.CreateCommand();
                cmd.CommandText = StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                return cmd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable MapToDataTable(DbDataReader dr)
        {
            DataTable dt = new DataTable();
            if (dr.HasRows)
                dt.Load(dr);
            return dt;

        }
        public static int ExecuteCommand(this DbCommand command, params SqlParameter[] nameValue)
        {
            using (command)
            {
                int result = 0;
                if (command.Connection.State == System.Data.ConnectionState.Closed)
                    command.Connection.Open();
                try
                {
                    command.Parameters.AddRange(nameValue);
                    result = command.ExecuteNonQuery();
                }
                finally
                {

                    command.Connection.Close();
                }
                return result;
            }
        }
        public static int GetScalar(DbCommand command, params SqlParameter[] values)
        {
            using (command)
            {
                command.Parameters.AddRange(values);
                int result = Convert.ToInt32(command.ExecuteScalar());
                return result;
            }
        }
        /// <summary>
        /// Insert / Update / Delete with/Without Parameters and returns Scalar Value
        /// </summary>
        /// <param name="safeSql"></param>
        /// <returns></returns>
        public static int GetScalar(DbCommand command)
        {
            using (command)
            {
                int result = Convert.ToInt32(command.ExecuteScalar());
                return result;
            }
        }

        public static DataTable BindDataTableWithParams(this DbCommand command, params SqlParameter[] nameValue)
        {
            using (command)
            {
                //foreach (var pair in nameValue)
                //{
                //    var param = command.CreateParameter();
                //    param.ParameterName = pair.Item1;
                //    param.Value = pair.Item2 ?? DBNull.Value;
                //    command.Parameters.Add(param);
                //}
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(nameValue);
                if (command.Connection.State == System.Data.ConnectionState.Closed)
                    command.Connection.Open();
                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        return MapToDataTable(reader);
                    }
                }
                finally
                {

                    command.Connection.Close();
                }
            }
        }
        public static DataSet BindDataSetWithParams(this DbCommand command, params SqlParameter[] nameValue)
        {
            try
            {
                using (command)
                {
                    foreach (var pair in nameValue)
                    {
                        var param = command.CreateParameter();
                        param.ParameterName = pair.ParameterName;
                        param.Value = pair.Value ?? DBNull.Value;
                        command.Parameters.Add(param);
                    }
                    if (command.Connection.State == System.Data.ConnectionState.Closed)
                        command.Connection.Open();
                    try
                    {
                        //using (var reader = command.ExecuteReader())
                        //{
                        var ds = new DataSet();
                        var dataAdapter = new SqlDataAdapter(command as SqlCommand);
                        dataAdapter.Fill(ds);
                        return ds;
                        //}
                    }
                    finally
                    {

                        command.Connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
                return new DataSet();
            }
        }

    }
}