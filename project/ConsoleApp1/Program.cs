using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using MySql.Data.MySqlClient;
using MySqlConnector;
using MySql;

namespace ConsoleApp1
{
    class Program
    {
            static void Main()
            {
            var csb = new MySqlConnectionStringBuilder
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
                using (var connection = new MySqlConnection(csb.ConnectionString))
                {
                    Int64 categoryId = nextCategoryId();
                    connection.Open();
                MySqlCommand insert_table = new MySqlCommand("DELETE FROM Categories_tbl WHERE categoryId = '2';", connection);
                //MySqlCommand insert_table = new MySqlCommand("INSERT INTO Categories_tbl (categoryName, imageURL, categoryId) values ('Food', 'https://storage.googleapis.com/wordproject/MAINFood.jpg', @categoryId);", connection);
                //insert_table.Parameters.AddWithValue("categoryId", categoryId);
                    insert_table.ExecuteNonQuery();
                    connection.Close();
            }


            }
        public void add()
        {

        }
        public static Int64 nextCategoryId()
        {
            return 2;
        }
    }
}
