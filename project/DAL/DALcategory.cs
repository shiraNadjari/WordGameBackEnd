using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMON;
using MySql.Data.MySqlClient;

namespace DAL
{
   public class DALcategory:DAL
    {
        public static Int64 nextCategoryId()
        {
            int x = -1;
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception)
                {
                    throw;
                }
                MySqlCommand max_category_id = new MySqlCommand("SELECT MAX(CategoryId) FROM Categories_tbl;", connection);
                x = Convert.ToInt32(max_category_id.ExecuteScalar());
                connection.Close();
            }
            return x+1;
        }

        public static void AddCategory(COMCategory cat)
        {
            //using (DBEntities context = new DBEntities())
            //{
            //    context.Categories_tbl.Add(MAPPER.ConvertCOMcategoryToDALcategory(cat));
            //    context.SaveChanges();
            //}
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                Int64 categoryId = nextCategoryId();
                connection.Open();
                MySqlCommand insert_table = new MySqlCommand("INSERT INTO Categories_tbl (categoryName, imageURL, categoryId) values (@categoryName, @imageURL, @categoryId);", connection);
                insert_table.Parameters.AddWithValue("categoryId", categoryId);
                insert_table.Parameters.AddWithValue("categoryName", cat.CategoryName);
                insert_table.Parameters.AddWithValue("imageURL", cat.ImageURL);
                insert_table.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static COMCategory GetCategoryById(int id)
        {
            //using (DBEntities context = new DBEntities())
            //{
            //    return MAPPER.ConvertDALcategoryToCOMcategory(context.Categories_tbl.FirstOrDefault(cat=>cat.CategoryID==id));
            //}
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                COMCategory c = new COMCategory();
                connection.Open();
                MySqlCommand get_category_by_id=new MySqlCommand("SELECT * FROM Categories_tbl WHERE categoryId=@catId;",connection);
                get_category_by_id.Parameters.AddWithValue("catId", id);
                using (var reader = get_category_by_id.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        c.CategoryId = Convert.ToInt32(reader["categoryId"]);
                        c.CategoryName= Convert.ToString(reader["categoryName"]);
                        c.ImageURL = Convert.ToString(reader["imageURL"]);
                    }
                }
                return c;
            }
        }

        public static List<COMCategory> GetCategories()
        {
            //using (DBEntities context = new DBEntities())
            //{
            //    return MAPPER.ConvertListDALcategoryToListCOMcategory(context.Categories_tbl.ToList());
            //}
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                List<COMCategory> list = new List<COMCategory>();
                connection.Open();
                MySqlCommand select_all_categories = new MySqlCommand("SELECT * FROM Categories_tbl", connection);
                using (var reader = select_all_categories.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //TODO: put the right syntax here
                        COMCategory category = new COMCategory()
                        {
                            CategoryId = Convert.ToInt32(reader["categoryId"]),
                            CategoryName = Convert.ToString(reader["categoryName"]),
                            ImageURL = Convert.ToString(reader["imageURL"])
                        };
                        list.Add(category);
                    }
                }
                return list;
                //return (List<COMCategory>)select_all_categories.ExecuteScalar();
            }
        }

        public static int GetPagesAmountPerCategory(int categoryId)
        {
            //using (DBEntities context = new DBEntities())
            //{
            //    return (DALimage.Getimages().Where(img => img.CategoryID == categoryId).Count()/10)+1;
            //}
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                connection.Open();
                MySqlCommand count_records_per_category = new MySqlCommand("SELECT COUNT(imageId) FROM Images_tbl WHERE categoryId=@catId;", connection);
                count_records_per_category.Parameters.AddWithValue("catId", categoryId);
                return Convert.ToInt32(count_records_per_category.ExecuteScalar());
            }
        }

        public static void UpdateURL(int catId, string url)
        {
            //using (DBEntities context = new DBEntities())
            //{
            //    context.Categories_tbl.FirstOrDefault(cat => cat.CategoryID == catId).ImageURL = url;
            //    context.SaveChanges();
            //}
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                connection.Open();
                MySqlCommand update_url = new MySqlCommand("UPDATE Categories_tbl SET imageURL=@newurl WHERE categoryId=@catId;",connection);
                update_url.Parameters.AddWithValue("newurl", url);
                update_url.Parameters.AddWithValue("catId", catId);
                update_url.ExecuteNonQuery();
                connection.Close();
            }
        }

        //remove category from table but not the image in storage
        public static void RemoveCategory(int id) //perfect
        {
            //using (DBEntities contex = new DBEntities())
            //{
            //    Categories_tbl cat =contex.Categories_tbl.FirstOrDefault(c => c.CategoryID == id);
            //    if (cat != null)
            //    {
            //        contex.Categories_tbl.Remove(cat);
            //        contex.SaveChanges();
            //    }
            //}
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                connection.Open();
                MySqlCommand delete_record = new MySqlCommand("DELETE FROM Categories_tbl WHERE categoryId = @idid;", connection);
                delete_record.Parameters.AddWithValue("idid", id);
                delete_record.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
