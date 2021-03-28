using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyAamdhani.Models
{
    public class SubCategoryEntityModel
    {
        MyAamdhaniEntities db = new MyAamdhaniEntities();
        public int ManageSubCategory(SubCategory model, string ManageType)
        {
            int response = 0;

            if (ManageType == "Update")
            {
                using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("Sp_SubCategoryManage", connection);
                    cmd.Parameters.AddWithValue("@SubCategoryId", model.SubCategoryId);
                    cmd.Parameters.AddWithValue("@CategoryId", model.CategoryId);
                    cmd.Parameters.AddWithValue("@SubCategoryName", model.SubCategoryName);
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
                    cmd.Parameters.AddWithValue("@CategoryId", model.CategoryId);
                    cmd.Parameters.AddWithValue("@SubCategoryName", model.SubCategoryName);
                    cmd.Parameters.AddWithValue("@ImagePath", model.ImagePath);
                    cmd.Parameters.AddWithValue("@ManageType", ManageType);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    response = Convert.ToInt32(cmd.Parameters["@SubCategoryId"].Value);
                    connection.Close();
                }
            }
            return response;
        }
        public List<SelectAllSubCategory> GetSubCategory(int skip, int take, string whereQuery, string SortQuery)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_SubCategoryManage", connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.CommandTimeout = 0;
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@action", "GetAllSubCategory"));
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@WhereQuery", whereQuery));
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@SortQuery", SortQuery));
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@RowFrom", skip));
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@RowTo", take));
                        adapter.Fill(dt);

                    }
                    connection.Close();
                }
                List<SelectAllSubCategory> subCategoryList = new List<SelectAllSubCategory>();
                subCategoryList = new List<SelectAllSubCategory>(from DataRow dr in dt.Rows
                                                           select new SelectAllSubCategory()
                                                           {
                                                               SubCategoryId = Convert.ToInt32(dr["SubCategoryId"]),
                                                               CategoryId = Convert.ToInt32(dr["CategoryId"]),
                                                               //DateAdded = Convert.ToDateTime(dr["NoticeDate"]).ToString("yyyy-MM-dd"),//Need to Convert.ToString() in App
                                                               Image = Convert.ToString(dr["Image"]),
                                                               CategoryName = Convert.ToString(dr["CategoryName"]),
                                                               SubCategoryName = Convert.ToString(dr["SubCategoryName"]),
                                                               IsActive = Convert.ToBoolean(dr["IsActive"]),
                                                               RCount = Convert.ToInt32(dr["RCount"]),
                                                               rownum = Convert.ToInt32(dr["rownum"]),
                                                               UniqueId = !string.IsNullOrEmpty(Convert.ToString(dr["UniqueId"])) ? Guid.Parse(Convert.ToString(dr["UniqueId"])) : Guid.Parse("00000000-0000-0000-0000-000000000000"),
                                                           }).ToList();
                return subCategoryList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SubCategory SubCategorySelectSubCategoryById(Guid Id)
        {
            SubCategory model = new SubCategory();
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_SubCategoryManage", connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.CommandTimeout = 0;
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@action", "GetSubCategoryById"));
                        adapter.SelectCommand.Parameters.Add(new SqlParameter("@UniqueId", Id));
                        adapter.Fill(dt);

                    }
                    connection.Close();
                }
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        model.SubCategoryId = Convert.ToInt32(row["SubCategoryId"]);
                        model.CategoryId = Convert.ToInt32(row["CategoryId"]);
                        model.SubCategoryName = Convert.ToString(row["SubCategoryName"]);
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
        public int DeleteSubCategory(int SubCategoryId)
        {
            try
            {
                SqlParameter subCategoryIdParam = new SqlParameter("@SubCategoryId", SubCategoryId);
                SqlParameter isDeleteParam = new SqlParameter("@IsDeleted", true);
                int Response;
                Response = db.Database.ExecuteSqlCommand("EXECUTE SubCategory_Delete @SubCategoryId,@IsDeleted", subCategoryIdParam, isDeleteParam);
                return Response;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
    public class SelectAllSubCategory
    {
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int rownum { get; set; }
        public Guid UniqueId { get; set; }
        public int RCount { get; set; }
    }
}