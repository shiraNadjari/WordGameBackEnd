using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMON;
using DAL;

namespace BLL
{
    public class BLLuser
    {
        public static void AddUser(COMuser user)
        {
            DALuser.AddUser(user);
        }

        public static COMuser GetUserById(int id)
        {
            return DALuser.GetUserById(id);
        }

        public static List<COMuser> GetUsers()
        {
            return DALuser.GetUsers();
        }

        public static void RemoveUser(int id)
        {
            DALuser.RemoveUser(id);
        }
    }
}
