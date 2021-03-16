using MyAamdhani.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAamdhani.Models
{

    public class SelectAllProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int rownum { get; set; }
        public Guid UniqueId { get; set; }
        public int RCount { get; set; }
    }
    public class ProductEntityModel
    {
        MyAamdhaniEntities db = new MyAamdhaniEntities();

        public object GetFabric()
        {
            try
            {
                var Fabric = db.tbl_Fabric.Select(s => new { FabricId = s.Fabric_Id, FabricName = s.Fabric_Name }).ToList();
                return Fabric;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public object GetPattern()
        {
            try
            {
                var Pattern = db.tbl_Pattern.Select(s => new { PatternId = s.Pattern_Id, PatternName = s.Pattern_Name}).ToList();
                return Pattern;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public object GetSareeBorder()
        {
            try
            {
                var SareeBorder = db.tbl_SareeBorder.Select(s => new { SareeBorderId = s.SareeBorder_Id, SareeBorderName = s.SareeBorder_Name }).ToList();
                return SareeBorder;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public object GetStyle()
        {
            try
            {
                var Style = db.tbl_ClothStyle.Select(s => new { StyleId = s.Style_Id, StyleName = s.Style_Name }).ToList();
                return Style;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public List<SelectListItem> GetLength()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "5.25 m",
                Value = "1"
            });
            items.Add(new SelectListItem
            {
                Text = "5.5 m",
                Value = "2"
            });
            items.Add(new SelectListItem
            {
                Text = "6.0 m",
                Value = "3"
            });
            return items;
        }

        public List<SelectAllProduct> GetProduct(int skip, int take, string whereQuery, string SortQuery)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_GetAllProduct", connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.CommandTimeout = 0;
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@action", "GetAllProducts"));
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@WhereQuery", whereQuery));
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@SortQuery", SortQuery));
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@RowFrom", skip));
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@RowTo", take));
                        adapter.Fill(dt);

                    }
                    connection.Close();
                }
                //SqlParameter[] param = new SqlParameter[5];
                //param[0] = new SqlParameter("@ProductId", 0);
                //param[1] = new SqlParameter("@WhereQuery", whereQuery);
                //param[2] = new SqlParameter("@SortQuery", SortQuery);
                //param[3] = new SqlParameter("@RowFrom", skip + 1);
                //param[4] = new SqlParameter("@RowTo", skip + take);
                //dt = DBHelper.BindDataTableWithParams(DBHelper.LoadStoredProcedure(db, "Sp_GetAllProducts"), param);
                List<SelectAllProduct> productList = new List<SelectAllProduct>();
                productList = new List<SelectAllProduct>(from DataRow dr in dt.Rows
                                                         select new SelectAllProduct()
                                                         {
                                                             ProductId = Convert.ToInt32(dr["ProductId"]),
                                                             //DateAdded = Convert.ToDateTime(dr["NoticeDate"]).ToString("yyyy-MM-dd"),//Need to Convert.ToString() in App
                                                             Image = Convert.ToString(dr["Image"]),
                                                             Quantity = Convert.ToInt32(dr["Quantity"]),
                                                             ProductName = Convert.ToString(dr["ProductName"]),
                                                             Price = Convert.ToInt32(dr["Price"]),
                                                             IsActive = Convert.ToBoolean(dr["IsActive"]),
                                                             RCount = Convert.ToInt32(dr["RCount"]),
                                                             rownum = Convert.ToInt32(dr["rownum"]),
                                                             UniqueId = !string.IsNullOrEmpty(Convert.ToString(dr["UniqueId"])) ? Guid.Parse(Convert.ToString(dr["UniqueId"])) : Guid.Parse("00000000-0000-0000-0000-000000000000"),
                                                         }).ToList();
                return productList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}