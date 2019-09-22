using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMON;
using MySql.Data.MySqlClient;

namespace DAL
{
   public class DALimage:DAL
    {
        public static Int64 nextImageId()
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
                MySqlCommand max_image_id = new MySqlCommand("SELECT MAX(imageId) FROM Images_tbl;", connection);
                x = Convert.ToInt32(max_image_id.ExecuteScalar());
                connection.Close();
            }
            return x+1;
        }
        //insert image into db
        public static void Addimage(COMimage img)
        {
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                Int64 imageId = nextImageId();
                try
                {
                    connection.Open();
                }
                catch (Exception)
                {
                    throw;
                }
                MySqlCommand insert_table = new MySqlCommand("INSERT INTO Images_tbl SET imageURL=@ImageURL,beginIndex=@BeginIndex,endIndex=@EndIndex,imageId=@ImageId,categoryId=(SELECT categoryId FROM Categories_tbl WHERE categoryId=@catId),userId=(SELECT userId FROM users_tbl WHERE userId=@uId);", connection);
                insert_table.Parameters.AddWithValue("ImageURL", img.URL);
                insert_table.Parameters.AddWithValue("BeginIndex", img.BeginIndex);
                insert_table.Parameters.AddWithValue("EndIndex", img.EndIndex);
                insert_table.Parameters.AddWithValue("ImageId", imageId);
                insert_table.Parameters.AddWithValue("catId", img.CategoryID);
                insert_table.Parameters.AddWithValue("uId", img.UserId);
                insert_table.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static COMimage GetImageById(int id)
        {
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                COMimage image = new COMimage();
                try
                {
                    connection.Open();
                }
                catch (Exception)
                {
                    throw;
                }
                MySqlCommand get_image_by_id = new MySqlCommand("SELECT * FROM Images_tbl WHERE imageId=@imgId;", connection);
                get_image_by_id.Parameters.AddWithValue("imgId", id);
                using (var reader = get_image_by_id.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        image.ImageID = Convert.ToInt32(reader["imageId"]);
                        image.URL = Convert.ToString(reader["imageURL"]);
                        image.UserId = Convert.ToInt32(reader["userId"]);
                        image.CategoryID = Convert.ToInt32(reader["categoryId"]);
                        image.BeginIndex = Convert.ToInt32(reader["beginIndex"]);
                        image.EndIndex = Convert.ToInt32(reader["endIndex"]);
                    }
                }
                return image;
            }
        }

        public static List<COMimage> Getimages()
        {
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                List<COMimage> list = new List<COMimage>();
                try
                {
                    connection.Open();
                }
                catch (Exception)
                {
                    throw;
                }
                MySqlCommand select_all_images = new MySqlCommand("SELECT * FROM Images_tbl", connection);
                using (var reader = select_all_images.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        COMimage image = new COMimage()
                        {
                            ImageID = Convert.ToInt32(reader["imageId"]),
                            URL = Convert.ToString(reader["imageURL"]),
                            UserId = Convert.ToInt32(reader["userId"]),
                            CategoryID = Convert.ToInt32(reader["categoryId"]),
                            BeginIndex = Convert.ToInt32(reader["beginIndex"]),
                            EndIndex = Convert.ToInt32(reader["endIndex"]),
                        };
                        list.Add(image);
                    }
                }
                return list;
            }
        }
        public static List<COMimage> GetImagesByCategoryId(int categoryId)
        {
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                return Getimages().FindAll(img => img.CategoryID == categoryId);
            }
        }

        public static List<COMimage> GetTwelveNextImages(int categoryId)
        {
            //return list with all images in this category
            using (DBEntities context = new DBEntities())
            {
                List<COMimage> list = new List<COMimage>();
                foreach (Images_tbl item in context.Images_tbl.Where(img => img.CategoryID == categoryId))
                {
                    list.Add(MAPPER.ConvertDALimageToCOMimage(item));
                }
                return list;
            }
        }

        public static void UpdateURL(int imgId, string url)
        {
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
                MySqlCommand update_url = new MySqlCommand("UPDATE Images_tbl SET imageURL=@newurl WHERE imageId=@imgId;", connection);
                update_url.Parameters.AddWithValue("newurl", url);
                update_url.Parameters.AddWithValue("imgId", imgId);
                update_url.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void Removeimage(int id)
        {
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
                MySqlCommand delete_record = new MySqlCommand("DELETE FROM Images_tbl WHERE imageId = @id;", connection);
                delete_record.Parameters.AddWithValue("id", id);
                delete_record.ExecuteNonQuery();
                connection.Close();
            }

        }

        public static int GetImageIdByURL(string URL)
        {
            int id = 0;
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
                MySqlCommand get_imageId_by_url = new MySqlCommand("SELECT imageId FROM Images_tbl WHERE imageURL=@imgURL;", connection);
                get_imageId_by_url.Parameters.AddWithValue("imgURL", URL);
                id = Convert.ToInt32(get_imageId_by_url.ExecuteScalar());
                connection.Close();
            }
            return id;
        }
    }
}
