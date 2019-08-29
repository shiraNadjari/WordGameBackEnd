using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMON;

namespace DAL
{
    public class DALuser
    {
        public static void AddUser(COMuser user)
        {
            using (DBEntities context = new DBEntities())
            {
                context.Users_tbl.Add(MAPPER.ConvertCOMuserToDALuser(user));
                context.SaveChanges();
            }
        }

        public static COMuser GetUserById(int id)
        {
            using (DBEntities context = new DBEntities())
            {
                return MAPPER.ConvertDALuserToCOMuser(context.Users_tbl.FirstOrDefault(u => u.UserId == id));
            }
        }

        public static List<COMuser> GetUsers()
        {
            using (DBEntities context = new DBEntities())
            {
                return MAPPER.ConvertListDALuserToListCOMuser(context.Users_tbl.ToList());
            }
        }

        public static void RemoveUser(int id)
        {
            using (DBEntities contex = new DBEntities())
            {
                Users_tbl user = contex.Users_tbl.FirstOrDefault(u => u.UserId == id);
                if (user != null)
                {
                    contex.Users_tbl.Remove(user);
                    contex.SaveChanges();
                }
            }
        }
    }
}
