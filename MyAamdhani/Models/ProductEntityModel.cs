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
                var Pattern = db.tbl_Pattern.Select(s => new { PatternId = s.Pattern_Id, PatternName = s.Pattern_Name }).ToList();
                return Pattern;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public object GetColor()
        {
            try
            {
                var Color = db.tbl_Color.Select(s => new { ColorId = s.Color_Id, ColorName = s.Color_Name }).ToList();
                return Color;
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

        public object GetCategory()
        {
            try
            {
                var Category = db.Categories.Select(s => new { CategoryId = s.CategoryId, CategoryName = s.CategoryName }).ToList();
                return Category;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public object GetSubCategory()
        {
            try
            {
                var SubCategory = db.SubCategories.Select(s => new { SubCategoryId = s.SubCategoryId, SubCategoryName = s.SubCategoryName }).ToList();
                return SubCategory;
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
            var items = new List<SelectListItem>()
    {
        new SelectListItem() { Text = "Item 1", Value = "#" },
        new SelectListItem() { Text = "Item 2", Value = "#" },
    };

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

        public bool ManageProduct(string ProductName, string ProductDescription, int MinOrder, decimal PricePerPiece, decimal MRPPerPiece, string HSNCode, string SKUId, int FabricId, int PatternId, int SareeBorderId, int StyleId, string Occasion, string PackageType, string SareeLength, int CategoryId, int SubCategoryId, bool chkBlouse, DataTable table)
        {
            try
            {
                SqlCommand com = new SqlCommand("", (SqlConnection)db.Database.Connection);
                com.CommandText = "exec Sp_ProductManage @ProductId, @ProductName, @ProductDescription, @MinOrder, @PricePerPiece, @MRPPerPiece, @HSNCode, @SKUId, @FabricId, @PatternId, @SareeBorderId, @StyleId, @Occasion, @PackageType, @SareeLength, @CateogryId, @SubCategoryId, @chkBlouse,@ImageData";
                com.Parameters.AddWithValue("@ProductId", 0);
                com.Parameters.AddWithValue("@ProductName", ProductName);
                com.Parameters.AddWithValue("@ProductDescription", ProductDescription);
                com.Parameters.AddWithValue("@PricePerPiece", PricePerPiece);
                com.Parameters.AddWithValue("@MRPPerPiece", MRPPerPiece);                
                com.Parameters.AddWithValue("@HSNCode", HSNCode);
                com.Parameters.AddWithValue("@SKUId", SKUId);
                com.Parameters.AddWithValue("@FabricId", FabricId);
                com.Parameters.AddWithValue("@PatternId", PatternId);
                com.Parameters.AddWithValue("@SareeBorderId", SareeBorderId);
                com.Parameters.AddWithValue("@StyleId", StyleId);
                com.Parameters.AddWithValue("@Occasion", Occasion);
                com.Parameters.AddWithValue("@PackageType", PackageType);
                com.Parameters.AddWithValue("@SareeLength", SareeLength);
                com.Parameters.AddWithValue("@CateogryId", CategoryId);
                com.Parameters.AddWithValue("@SubCategoryId", SubCategoryId);
                com.Parameters.AddWithValue("@chkBlouse", chkBlouse);
                com.Parameters.AddWithValue("@Manage_Type", "Insert");
                SqlParameter param = new SqlParameter("@ImageData", SqlDbType.Structured)
                {
                    TypeName = "dbo.Tt_ImagesValue",
                    Value = (table.Rows.Count > 0 ? table : null)
                };
                com.Parameters.Add(param);

                if (com.Connection.State == ConnectionState.Closed)
                {
                    com.Connection.Open();
                    com.ExecuteNonQuery();
                    com.Parameters.Clear();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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