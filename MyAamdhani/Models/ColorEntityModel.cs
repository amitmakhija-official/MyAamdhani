using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyAamdhani.Models
{
    public class ColorEntityModel
    {
        MyAamdhaniEntities db = new MyAamdhaniEntities();
        public int ManageColor(tbl_Color model, string ManageType)
        {
            int response = 0;

            if (ManageType == "Update")
            {
                using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("Sp_ColorManage", connection);
                    cmd.Parameters.AddWithValue("@ColorId", model.Color_Id);
                    cmd.Parameters.AddWithValue("@ColorName", model.Color_Name);
                    cmd.Parameters.AddWithValue("@ColorCode", model.Color_Code);
                    cmd.Parameters.AddWithValue("@ManageType", ManageType);

                    cmd.CommandType = CommandType.StoredProcedure;
                    response = cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }

            if (ManageType == "Insert")
            {
                using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("Sp_ColorManage", connection);
                    cmd.Parameters.AddWithValue("@ColorName", model.Color_Name);
                    cmd.Parameters.AddWithValue("@ColorCode", model.Color_Code);
                    cmd.Parameters.AddWithValue("@ManageType", ManageType);

                    cmd.CommandType = CommandType.StoredProcedure;
                    response = cmd.ExecuteNonQuery();
                    //response = Convert.ToInt32(cmd.Parameters["@CategoryId"].Value);
                    connection.Close();
                }
            }
            return response;
        }
        public List<SelectAllColor> GetAllColors(int skip, int take, string whereQuery, string SortQuery)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_ColorManage", connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.CommandTimeout = 0;
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@action", "GetAllColors"));
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@WhereQuery", whereQuery));
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@SortQuery", SortQuery));
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@RowFrom", skip));
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@RowTo", take));
                        adapter.Fill(dt);

                    }
                    connection.Close();
                }
                List<SelectAllColor> colorList = new List<SelectAllColor>();
                colorList = new List<SelectAllColor>(from DataRow dr in dt.Rows
                                                     select new SelectAllColor()
                                                     {
                                                         ColorId = Convert.ToInt32(dr["ColorId"]),
                                                         //DateAdded = Convert.ToDateTime(dr["NoticeDate"]).ToString("yyyy-MM-dd"),//Need to Convert.ToString() in App
                                                         ColorName = Convert.ToString(dr["ColorName"]),
                                                         ColorCode = Convert.ToString(dr["ColorCode"]),
                                                         IsActive = Convert.ToBoolean(dr["IsActive"]),
                                                         RCount = Convert.ToInt32(dr["RCount"]),
                                                         rownum = Convert.ToInt32(dr["rownum"]),
                                                         UniqueId = !string.IsNullOrEmpty(Convert.ToString(dr["UniqueId"])) ? Guid.Parse(Convert.ToString(dr["UniqueId"])) : Guid.Parse("00000000-0000-0000-0000-000000000000"),
                                                     }).ToList();
                return colorList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public tbl_Color ColorSelectColorById(Guid Id)
        {
            tbl_Color model = new tbl_Color();
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_ColorManage", connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.CommandTimeout = 0;
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@action", "GetColorById"));
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@UniqueId", Id));
                        adapter.Fill(dt);

                    }
                    connection.Close();
                }
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        model.Color_Id = Convert.ToInt32(row["Color_Id"]);
                        model.Color_Name = Convert.ToString(row["Color_Name"]);
                        model.Color_Code = Convert.ToString(row["Color_Code"]);
                        model.IsActive = Convert.ToBoolean(row["IsActive"]);
                        model.UniqueId = !string.IsNullOrEmpty(Convert.ToString(row["UniqueId"])) ? Guid.Parse(Convert.ToString(row["UniqueId"])) : Guid.Parse("00000000-0000-0000-0000-000000000000");

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return model;
        }
        public bool UpdateStatus(int ColorId, bool Status)
        {
            try
            {
                SqlParameter ColorIdParam = new SqlParameter("@ColorId", ColorId);
                SqlParameter IsActiveParam = new SqlParameter("@IsActive", Status);
                SqlParameter actionParam = new SqlParameter("@action", "StatusUpdate");
                int Response;
                Response = db.Database.ExecuteSqlCommand("EXECUTE Sp_ColorManage @ColorId,@IsActive,@action", ColorIdParam, IsActiveParam, actionParam);
                if (Response > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int DeleteColor(int ColorId)
        {
            try
            {
                SqlParameter colorIdParam = new SqlParameter("@ColorId", ColorId);
                SqlParameter actionParam = new SqlParameter("@action", "Delete");
                int Response;
                Response = db.Database.ExecuteSqlCommand("EXECUTE Sp_ColorManage @ColorId,@action", colorIdParam, actionParam);
                return Response;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }

    public class SelectAllColor
    {
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int rownum { get; set; }
        public Guid UniqueId { get; set; }
        public int RCount { get; set; }
    }


}