using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMON;
namespace DAL
{
   public class DALimage
    {
        public static void Addimage(COMimage img)
        {
            using (DBEntities context = new DBEntities())
            {
                context.Images_tbl.Add(MAPPER.ConvertCOMimageToDALimage(img));
                context.SaveChanges();
            }
        }

        public static COMimage GetImageById(int id)
        {
            using (DBEntities context = new DBEntities())
            {
                return MAPPER.ConvertDALimageToCOMimage(context.Images_tbl.FirstOrDefault(img => img.ImageID == id));
            }
        }

        public static List<COMimage> Getimages()
        {
            using (DBEntities context = new DBEntities())
            {
                return MAPPER.ConvertListDAlimageToListCOMimage(context.Images_tbl.ToList());
            }
        }

        public static List<COMimage> GetTenNextImages(int categoryId)
        {
            //return list with all images objects in this category
            using (DBEntities context = new DBEntities())
            {
                List<COMimage> list = new List<COMimage>();
                foreach (Images_tbl item in context.Images_tbl.Where(img => img.CategoryID == categoryId))
                {
                    list.Add(MAPPER.ConvertDALimageToCOMimage(item));
                }
                return list;
            }
        }

        public static void Removeimage(int id)
        {
            using (DBEntities contex = new DBEntities())
            {
                Images_tbl img = contex.Images_tbl.FirstOrDefault(c => c.ImageID == id);
                if (img != null)
                {
                    contex.Images_tbl.Remove(img);
                    contex.SaveChanges();
                }
            }

        }
        public static int NumImages()
        {
            using (DBEntities contex = new DBEntities())
            {
                return contex.Images_tbl.Count();
            }
        }
    }
}
