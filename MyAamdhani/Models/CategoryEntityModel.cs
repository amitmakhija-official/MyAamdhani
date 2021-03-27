using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyAamdhani.Models
{
    public class CategoryEntityModel
    {
        MyAamdhaniEntities db = new MyAamdhaniEntities();
        public int ManageCategory(Category model, string ManageType)
        {
            int response = 0;

            if (ManageType == "Update")
            {
                using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("Sp_CategoryManage", connection);
                    cmd.Parameters.AddWithValue("@CategoryId", model.CategoryId);
                    cmd.Parameters.AddWithValue("@CategoryName", model.CategoryName);
                    cmd.Parameters.AddWithValue("@ImagePath", model.ImagePath);
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
                    SqlCommand cmd = new SqlCommand("Sp_CategoryManage", connection);
                    cmd.Parameters.AddWithValue("@CategoryName", model.CategoryName);
                    cmd.Parameters.AddWithValue("@ImagePath", model.ImagePath);
                    cmd.Parameters.AddWithValue("@ManageType", ManageType);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    response = Convert.ToInt32(cmd.Parameters["@CategoryId"].Value);
                    connection.Close();
                }
            }
            return response;
        }
        public List<SelectAllCategory> GetCategory(int skip, int take, string whereQuery, string SortQuery)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_CategoryManage", connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.CommandTimeout = 0;
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@action", "GetAllCategory"));
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@WhereQuery", whereQuery));
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@SortQuery", SortQuery));
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@RowFrom", skip));
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@RowTo", take));
                        adapter.Fill(dt);

                    }
                    connection.Close();
                }
                List<SelectAllCategory> categoryList = new List<SelectAllCategory>();
                categoryList = new List<SelectAllCategory>(from DataRow dr in dt.Rows
                                                           select new SelectAllCategory()
                                                           {
                                                               CategoryId = Convert.ToInt32(dr["CategoryId"]),
                                                               //DateAdded = Convert.ToDateTime(dr["NoticeDate"]).ToString("yyyy-MM-dd"),//Need to Convert.ToString() in App
                                                               Image = Convert.ToString(dr["Image"]),
                                                               CategoryName = Convert.ToString(dr["CategoryName"]),
                                                               IsActive = Convert.ToBoolean(dr["IsActive"]),
                                                               RCount = Convert.ToInt32(dr["RCount"]),
                                                               rownum = Convert.ToInt32(dr["rownum"]),
                                                               UniqueId = !string.IsNullOrEmpty(Convert.ToString(dr["UniqueId"])) ? Guid.Parse(Convert.ToString(dr["UniqueId"])) : Guid.Parse("00000000-0000-0000-0000-000000000000"),
                                                           }).ToList();
                return categoryList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Category CategorySelectCategoryById(Guid Id)
        {
            Category model = new Category();
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_CategoryManage", connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.CommandTimeout = 0;
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@action", "GetCategoryById"));
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@UniqueId", Id));
                        adapter.Fill(dt);

                    }
                    connection.Close();
                }
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        model.CategoryId = Convert.ToInt32(row["CategoryId"]);
                        model.CategoryName = Convert.ToString(row["CategoryName"]);
                        model.ImagePath = Convert.ToString(row["ImagePath"]);
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
        public int DeleteCategory(int CategoryId)
        {
            try
            {
                SqlParameter categoryIdParam = new SqlParameter("@CategoryId", CategoryId);
                SqlParameter isDeleteParam = new SqlParameter("@IsDeleted", true);
                int Response;
                Response = db.Database.ExecuteSqlCommand("EXECUTE Category_Delete @CategoryId,@IsDeleted", categoryIdParam, isDeleteParam);
                return Response;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
   
    public class SelectAllCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int rownum { get; set; }
        public Guid UniqueId { get; set; }
        public int RCount { get; set; }
    }


}