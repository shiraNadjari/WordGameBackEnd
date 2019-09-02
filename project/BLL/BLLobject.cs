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

        public static bool CheckObjectExists(List<double> x,List<double> y,int imgId)
        {
            List<COMimageObject> objects = GetObjects().FindAll(obj => obj.ImageID == imgId);
            if (objects.Count == 0)
                return false;
            foreach (COMimageObject obj in objects)
            {
                if (obj.X1 == x[0] && obj.Y1 == y[0] && obj.X2 == x[1] && obj.Y2 == y[1] && obj.X3 == x[2] && obj.Y3 == y[2] && obj.X4 == x[3] && obj.Y4 == y[3])
                    return true;
            }
            return false;
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
