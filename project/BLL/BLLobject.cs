using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMON;
using DAL;

namespace BLL
{
    public class BLLobject
    {
        public static void AddObject(COMimageObject obj)
        {
            DALimageObject.AddObject(obj);
        }

        public static COMimageObject GetObjectById(int id)
        {
            return DALimageObject.GetObjectById(id);
        }

        public static List<COMimageObject> GetObjects()
        {
            return DALimageObject.Getobjects();
        }

        public static void RemoveObject(int id)
        {
            DALimageObject.RemoveObject(id);
        }
    }
}
