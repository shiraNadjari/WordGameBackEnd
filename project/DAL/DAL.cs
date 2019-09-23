using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DAL
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
    }
}
