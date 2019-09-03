using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMON;
using DAL;

namespace BLL
{
    public class BLLcategory
    {
        public static void AddCategory(COMCategory cat,Dictionary<string,int> categoriesCounter)
        {
            DALcategory.AddCategory(cat);
            cat.ImageURL = BLLgoogleVision.Storage(GetCategoryIdByCategoryName(cat.CategoryName).CategoryId, cat.ImageURL,categoriesCounter, true);
            UpdateURL(GetCategoryIdByCategoryName(cat.CategoryName).CategoryId, cat.ImageURL);
        }

        public static COMCategory GetCategoryById(int id)
        {
            return DALcategory.GetCategoryById(id);
        }

        public static List<COMCategory> GetCategories()
        {
            return DALcategory.GetCategories();
        }

        public static COMCategory GetCategoryIdByCategoryName(string categoryName)
        {
            return DALcategory.GetCategories().FirstOrDefault(cat => cat.CategoryName == categoryName);
        }

        public static int GetPagesAmountPerCategory(int categoryId)
        {
            return DALcategory.GetPagesAmountPerCategory(categoryId);
        }

        public static void UpdateURL(int catId,string url)
        {
            DALcategory.UpdateURL(catId, url);
        }
        public static void RemoveCategory(int id)
        {
            DALcategory.RemoveCategory(id);
        }
    }
}
