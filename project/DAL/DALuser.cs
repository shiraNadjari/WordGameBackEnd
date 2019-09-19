using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMON;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class DALuser
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

        public static Int64 nextUserId()
        {
            int x = -1;
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                connection.Open();
                MySqlCommand count_categories = new MySqlCommand("SELECT COUNT(userId) FROM users_tbl;", connection);
                x = Convert.ToInt32(count_categories.ExecuteScalar());
                connection.Close();
            }
            return x + 1;
        }

        public static void AddUser(COMuser user)
        {
            //using (DBEntities context = new DBEntities())
            //{
            //    context.Users_tbl.Add(MAPPER.ConvertCOMuserToDALuser(user));
            //    context.SaveChanges();
            //}
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                Int64 userId = nextUserId();
                connection.Open();
                MySqlCommand insert_table = new MySqlCommand("INSERT INTO users_tbl (email, password, categoryName,imageURL,userId) values (@Email,@Password,@CategoryName, @ImageURL, @UserId);", connection);
                insert_table.Parameters.AddWithValue("Email", user.Email);
                insert_table.Parameters.AddWithValue("Password", user.Password);
                insert_table.Parameters.AddWithValue("CategoryName", user.CategoryName);
                insert_table.Parameters.AddWithValue("ImageURL", user.ImageURL);
                insert_table.Parameters.AddWithValue("UserId", userId);
                insert_table.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static COMuser GetUserById(int id)
        {
            //using (DBEntities context = new DBEntities())
            //{
            //    return MAPPER.ConvertDALuserToCOMuser(context.Users_tbl.FirstOrDefault(u => u.UserId == id));
            //}
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                COMuser u = new COMuser();
                connection.Open();
                MySqlCommand get_user_by_id = new MySqlCommand("SELECT * FROM users_tbl WHERE userId=@uId;", connection);
                get_user_by_id.Parameters.AddWithValue("uId", id);
                using (var reader = get_user_by_id.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        u.UserId = Convert.ToInt32(reader["userId"]);
                        u.Password = Convert.ToString(reader["password"]);
                        u.Email = Convert.ToString(reader["email"]);
                        u.CategoryName = Convert.ToString(reader["categoryName"]);
                        u.ImageURL = Convert.ToString(reader["imageURL"]);
                    }
                }
                return u;
            }
        }

        public static List<COMuser> GetUsers()
        {
            //using (DBEntities context = new DBEntities())
            //{
            //    return MAPPER.ConvertListDALuserToListCOMuser(context.Users_tbl.ToList());
            //}
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                List<COMuser> list = new List<COMuser>();
                connection.Open();
                MySqlCommand select_all_users = new MySqlCommand("SELECT * FROM users_tbl", connection);
                using (var reader = select_all_users.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //TODO: put the right syntax here
                        COMuser user = new COMuser()
                        {
                            UserId = Convert.ToInt32(reader["userId"]),
                            Password = Convert.ToString(reader["password"]),
                            Email = Convert.ToString(reader["email"]),
                            ImageURL = Convert.ToString(reader["imageURL"]),
                            CategoryName = Convert.ToString(reader["categoryName"]),

                        };
                        list.Add(user);
                    }
                }
                return list;
            }
        }

        public static void RemoveUser(int id)
        {
            //using (DBEntities contex = new DBEntities())
            //{
            //    Users_tbl user = contex.Users_tbl.FirstOrDefault(u => u.UserId == id);
            //    if (user != null)
            //    {
            //        contex.Users_tbl.Remove(user);
            //        contex.SaveChanges();
            //    }
            //}
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                connection.Open();
                MySqlCommand delete_record = new MySqlCommand("DELETE FROM users_tbl WHERE userId = @id;", connection);
                delete_record.Parameters.AddWithValue("id", id);
                delete_record.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
