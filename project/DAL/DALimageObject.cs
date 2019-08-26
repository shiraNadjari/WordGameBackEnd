using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMON;
namespace DAL
{
    public class DALimageObject
    {
        public static void AddObject(COMimageObject obj)
        {
            using (DBEntities context = new DBEntities())
            {
                context.Objects_tbl.Add(MAPPER.ConvertCOMobjectToDALobject(obj));
                context.SaveChanges();
            }
        }

        public static COMimageObject GetObjectById(int id)
        {
            using (DBEntities context = new DBEntities())
            {
                return MAPPER.ConvertDALobjectToCOMobject(context.Objects_tbl.FirstOrDefault(obj => obj.ObjectID == id));
            }
        }

        public static List<COMimageObject> Getobjects()
        {
            using (DBEntities context = new DBEntities())
            {
                return MAPPER.ConvertListDALobjectToListCOMobject(context.Objects_tbl.ToList());
            }
        }

        public static void RemoveObject(int id)
        {
            using (DBEntities contex = new DBEntities())
            {
                Objects_tbl obj = contex.Objects_tbl.FirstOrDefault(o => o.ObjectID == id);
                if (obj != null)
                {
                    contex.Objects_tbl.Remove(obj);
                    contex.SaveChanges();
                }
            }
        }
    }
}
