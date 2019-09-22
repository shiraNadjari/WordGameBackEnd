using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMON;
using MySql.Data.MySqlClient;

namespace DAL
{
   public class DALimage
    {
        public static MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder
        {
            Server = "35.228.221.113",
            UserID = "root",
            Password = "root",
            Database = "database",
            CertificateFile = @"C:\Users\ריקי\Downloads\client.pfx",
            CACertificateFile = @"C:\Users\ריקי\Downloads\server-ca.pem",
            SslCa = @"C:\Users\ריקי\Downloads\server-ca.pem",
            //SslMode = MySqlSslMode.VerifyCA,
            SslMode = MySqlSslMode.None
        };
        public static Int64 nextImageId()
        {
            int x = -1;
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                connection.Open();
                MySqlCommand count_categories = new MySqlCommand("SELECT COUNT(imageId) FROM Images_tbl;", connection);
                x = Convert.ToInt32(count_categories.ExecuteScalar());
                connection.Close();
            }
            return x + 1;
        }
        public static void Addimage(COMimage img)
        {
            //using (DBEntities context = new DBEntities())
            //{
            //    context.Images_tbl.Add(MAPPER.ConvertCOMimageToDALimage(img));
            //    context.SaveChanges();
            //}
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                Int64 imageId = nextImageId();
                connection.Open();
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
            //using (DBEntities context = new DBEntities())
            //{
            //    return MAPPER.ConvertDALimageToCOMimage(context.Images_tbl.FirstOrDefault(img => img.ImageID == id));
            //}
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                COMimage image = new COMimage();
                connection.Open();
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
            //using (DBEntities context = new DBEntities())
            //{
            //    return MAPPER.ConvertListDAlimageToListCOMimage(context.Images_tbl.ToList());
            //}
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                List<COMimage> list = new List<COMimage>();
                connection.Open();
                MySqlCommand select_all_images = new MySqlCommand("SELECT * FROM Images_tbl", connection);
                using (var reader = select_all_images.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //TODO: put the right syntax here
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
            //using (DBEntities context = new DBEntities())
            //{
            //    return Getimages().FindAll(img => img.CategoryID == categoryId);
            //}
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

        public static void UpdateEndIndex(int imgId,int end)
        {
            using (DBEntities context = new DBEntities())
            {
                context.Images_tbl.FirstOrDefault(img => img.ImageID == imgId).EndIndex = end;
                context.SaveChanges();
            }
        }

        public static void UpdateURL(int imgId, string url)
        {
            //using (DBEntities context = new DBEntities())
            //{
            //    context.Images_tbl.FirstOrDefault(img => img.ImageID == imgId).URL = url;
            //    context.SaveChanges();
            //}
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                connection.Open();
                MySqlCommand update_url = new MySqlCommand("UPDATE Images_tbl SET imageURL=@newurl WHERE imageId=@imgId;", connection);
                update_url.Parameters.AddWithValue("newurl", url);
                update_url.Parameters.AddWithValue("imgId", imgId);
                update_url.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void Refresh()
        {
            using (DBEntities context = new DBEntities())
            {
                context.Entry(context.Images_tbl).Reload();
            }
        }

        public static void Removeimage(int id)
        {
            //using (DBEntities contex = new DBEntities())
            //{
            //    Images_tbl img = contex.Images_tbl.FirstOrDefault(c => c.ImageID == id);
            //    if (img != null)
            //    {
            //        contex.Images_tbl.Remove(img);
            //        contex.SaveChanges();
            //    }
            //}
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                connection.Open();
                MySqlCommand delete_record = new MySqlCommand("DELETE FROM Images_tbl WHERE imageId = @id;", connection);
                delete_record.Parameters.AddWithValue("id", id);
                delete_record.ExecuteNonQuery();
                connection.Close();
            }

        }
        public static int NumImages()
        {
            using (DBEntities contex = new DBEntities())
            {
                return contex.Images_tbl.Count();
            }
        }

        public static int GetImageIdByURL(string URL)
        {
            //using (DBEntities context = new DBEntities())
            //{
            //    return context.Images_tbl.FirstOrDefault(img => img.URL == URL).ImageID;            
            //}
            int id = 0;
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                connection.Open();
                MySqlCommand get_imageId_by_url = new MySqlCommand("SELECT imageId FROM Images_tbl WHERE imageURL=@imgURL;", connection);
                get_imageId_by_url.Parameters.AddWithValue("imgURL", URL);
                id = Convert.ToInt32(get_imageId_by_url.ExecuteScalar());
                connection.Close();
            }
            return id;
        }
    }
}
