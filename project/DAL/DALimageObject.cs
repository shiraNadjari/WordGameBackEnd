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
            using (DBEntities context = new DBEntities())
            {
                context.Objects_tbl.FirstOrDefault(obj => obj.ObjectID == objId).VoiceURL = url;
                context.SaveChanges();
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
