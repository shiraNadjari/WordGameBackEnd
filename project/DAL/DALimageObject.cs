using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMON;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class DALimageObject
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

        public static Int64 nextObjectId()
        {
            int x = -1;
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                connection.Open();
                MySqlCommand count_objects = new MySqlCommand("SELECT COUNT(objectId) FROM Objects_tbl;", connection);
                x = Convert.ToInt32(count_objects.ExecuteScalar());
                connection.Close();
            }
            return x + 1;
        }

        public static void AddObject(COMimageObject obj)
        {
            //using (DBEntities context = new DBEntities())
            //{
            //    context.Objects_tbl.Add(MAPPER.ConvertCOMobjectToDALobject(obj));
            //    context.SaveChanges();
            //}
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                Int64 objectId = nextObjectId();
                connection.Open();
                MySqlCommand insert_table = new MySqlCommand("INSERT INTO Objects_tbl SET objectName=@ObjectName,X1=@X1,X2=@X2,X3=@X3,X4=@X4,Y1=@Y1,Y2=@Y2,Y3=@Y3,Y4=@Y4,voiceURL=@VoiceURL,objectId=@ObjectId,imageId=(SELECT imageId FROM Images_tbl WHERE imageId=@imgId);", connection);
                insert_table.Parameters.AddWithValue("ObjectName", obj.Name);
                insert_table.Parameters.AddWithValue("X1", obj.X1);
                insert_table.Parameters.AddWithValue("X2", obj.X2);
                insert_table.Parameters.AddWithValue("X3", obj.X3);
                insert_table.Parameters.AddWithValue("X4", obj.X4);
                insert_table.Parameters.AddWithValue("Y1", obj.Y1);
                insert_table.Parameters.AddWithValue("Y2", obj.Y2);
                insert_table.Parameters.AddWithValue("Y3", obj.Y3);
                insert_table.Parameters.AddWithValue("Y4", obj.Y4);
                insert_table.Parameters.AddWithValue("VoiceURL", obj.VoiceURL);
                insert_table.Parameters.AddWithValue("ObjectId", objectId);
                insert_table.Parameters.AddWithValue("imgId", obj.ImageID);
                insert_table.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static COMimageObject GetObjectById(int id)
        {
            //using (DBEntities context = new DBEntities())
            //{
            //    return MAPPER.ConvertDALobjectToCOMobject(context.Objects_tbl.FirstOrDefault(obj => obj.ObjectID == id));
            //}
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                COMimageObject obj = new COMimageObject();
                connection.Open();
                MySqlCommand get_object_by_id = new MySqlCommand("SELECT * FROM Objects_tbl WHERE objectId=@objId;", connection);
                get_object_by_id.Parameters.AddWithValue("objId", id);
                using (var reader = get_object_by_id.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        obj.ObjectId = Convert.ToInt32(reader["objectId"]);
                        obj.Name = Convert.ToString(reader["objectName"]);
                        obj.ImageID = Convert.ToInt32(reader["imageId"]);
                        obj.VoiceURL = Convert.ToString(reader["voiceURL"]);
                        obj.X1 = Convert.ToInt32(reader["X1"]);
                        obj.X2 = Convert.ToInt32(reader["X2"]);
                        obj.X3 = Convert.ToInt32(reader["X3"]);
                        obj.X4 = Convert.ToInt32(reader["X4"]);
                        obj.Y1 = Convert.ToInt32(reader["Y1"]);
                        obj.Y2 = Convert.ToInt32(reader["Y2"]);
                        obj.Y3 = Convert.ToInt32(reader["Y3"]);
                        obj.Y4 = Convert.ToInt32(reader["Y4"]);
                    }
                }
                return obj;
            }
        }

        public static List<COMimageObject> Getobjects()
        {
            //using (DBEntities context = new DBEntities())
            //{
            //    return MAPPER.ConvertListDALobjectToListCOMobject(context.Objects_tbl.ToList());
            //}
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                List<COMimageObject> list = new List<COMimageObject>();
                connection.Open();
                MySqlCommand select_all_objects = new MySqlCommand("SELECT * FROM Objects_tbl", connection);
                using (var reader = select_all_objects.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //TODO: put the right syntax here
                        COMimageObject obj = new COMimageObject()
                        {
                            ImageID = Convert.ToInt32(reader["imageId"]),
                            ObjectId = Convert.ToInt32(reader["objectId"]),
                            Name = Convert.ToString(reader["objectName"]),
                            VoiceURL = Convert.ToString(reader["voiceURL"]),
                            X1 = Convert.ToInt32(reader["X1"]),
                            X2 = Convert.ToInt32(reader["X2"]),
                            X3 = Convert.ToInt32(reader["X3"]),
                            X4 = Convert.ToInt32(reader["X4"]),
                            Y1 = Convert.ToInt32(reader["Y1"]),
                            Y2 = Convert.ToInt32(reader["Y2"]),
                            Y3 = Convert.ToInt32(reader["Y3"]),
                            Y4 = Convert.ToInt32(reader["Y4"]),
                        };
                        list.Add(obj);
                    }
                }
                return list;
            }
        }

        public static List<COMimageObject> GetObjectsByCategory(int categoryId)
        {
            List<int> ids = new List<int>();
            List<COMimageObject> objects = new List<COMimageObject>();
            //return all objects in all images in specific category
            DALimage.GetImagesByCategoryId(categoryId).ForEach(img => ids.Add(img.ImageID));
            ids.ForEach(id=>objects.AddRange(Getobjects().FindAll(obj => obj.ImageID == id)));
            return objects;
        }

        public static void UpdateVoiceURL(int objId,string url)
        {
            //using (DBEntities context = new DBEntities())
            //{
            //    context.Objects_tbl.FirstOrDefault(obj => obj.ObjectID == objId).VoiceURL = url;
            //    context.SaveChanges();
            //}
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                connection.Open();
                MySqlCommand update_url = new MySqlCommand("UPDATE Objects_tbl SET voiceURL=@newurl WHERE objectId=@objId;", connection);
                update_url.Parameters.AddWithValue("newurl", url);
                update_url.Parameters.AddWithValue("objId", objId);
                update_url.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void UpdateNameAndVoiceURL(int objId,int voiceNumber,string newName,string newurl)
        {
            
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                connection.Open();
                MySqlCommand update_name_and_voiceurl = new MySqlCommand("UPDATE Objects_tbl SET objectName=@objName,voiceURL=@VoiceURL WHERE objectId=@objId;",connection);
                update_name_and_voiceurl.Parameters.AddWithValue("objName", newName);
                update_name_and_voiceurl.Parameters.AddWithValue("VoiceURL", newurl);
                update_name_and_voiceurl.Parameters.AddWithValue("objId", objId);
                update_name_and_voiceurl.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void Refresh()
        {
            using (DBEntities context = new DBEntities())
            {
               // context.Entry(context.Images_tbl).Reload();

                foreach (var entity in context.ChangeTracker.Entries())
                {
                    entity.Reload();
                }
            }
        }

        public static void RemoveObject(int id)
        {
            //using (DBEntities contex = new DBEntities())
            //{
            //    Objects_tbl obj = contex.Objects_tbl.FirstOrDefault(o => o.ObjectID == id);
            //    if (obj != null)
            //    {
            //        contex.Objects_tbl.Remove(obj);
            //        contex.SaveChanges();
            //    }
            //}
            using (var connection = new MySqlConnection(csb.ConnectionString))
            {
                connection.Open();
                MySqlCommand delete_record = new MySqlCommand("DELETE FROM Objects_tbl WHERE objectId = @id;", connection);
                delete_record.Parameters.AddWithValue("id", id);
                delete_record.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
