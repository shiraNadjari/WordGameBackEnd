using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMON;

namespace DAL
{
   public class DALcategory
    {
        public static void AddCategory(COMCategory cat)
        {
            using (DBEntities context = new DBEntities())
            {
                context.Categories_tbl.Add(MAPPER.ConvertCOMcategoryToDALcategory(cat));
                context.SaveChanges();
            }
        }

        public static COMCategory GetCategoryById(int id)
        {
            using (DBEntities context = new DBEntities())
            {
                return MAPPER.ConvertDALcategoryToCOMcategory(context.Categories_tbl.FirstOrDefault(cat=>cat.CategoryID==id));
            }
        }

        public static List<COMCategory> GetCategories()
        {
            using (DBEntities context = new DBEntities())
            {
                return MAPPER.ConvertListDALcategoryToListCOMcategory(context.Categories_tbl.ToList());
            }
        }

        public static int GetPagesAmountPerCategory(int categoryId)
        {
            using (DBEntities context = new DBEntities())
            {
                return DALimage.Getimages().Where(img => img.CategoryID == categoryId).Count()/10;
            }
        }

        public static void UpdateURL(int catId, string url)
        {
            using (DBEntities context = new DBEntities())
            {
                context.Categories_tbl.FirstOrDefault(cat => cat.CategoryID == catId).ImageURL = url;
                context.SaveChanges();
            }
        }

        public static void RemoveCategory(int id)
        {
            using (DBEntities contex = new DBEntities())
            {
                Categories_tbl cat =contex.Categories_tbl.FirstOrDefault(c => c.CategoryID == id);
                if (cat != null)
                {
                    contex.Categories_tbl.Remove(cat);
                    contex.SaveChanges();
                }
            }
        }
    }
}
