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
        public static void AddCategory(Comcategory cat)
        {
            DALcategory.AddCategory(cat);
        }

        public static Comcategory GetCategoryById(int id)
        {
            return DALcategory.GetCategoryById(id);
        }

        public static List<Comcategory> GetCategories()
        {
            return DALcategory.GetCategories();
        }

        public static void RemoveCategory(int id)
        {
            DALcategory.RemoveCategory(id);
        }
    }
}
