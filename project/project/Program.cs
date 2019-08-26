using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMON;
using DAL;
namespace project
{
    class Program
    {
        static void Main(string[] args)
        {
            //Comcategory cat = new Comcategory();
            //cat.CategoryId = 1;
            //cat.CategoryName = "stationary";
            //cat.ImageURL = @"C: \Users\ריקי\Desktop\WordGameBackend\project\pictures\stationary\1.jpg";
            //DALcategory.AddCategory(cat);
            foreach (Comcategory item in DALcategory.GetCategories())
            {
                Console.WriteLine(item.CategoryName);
            }
            DALcategory.RemoveCategory(1);
            foreach (Comcategory item in DALcategory.GetCategories())
            {
                Console.WriteLine(item.CategoryName);
            }
        }
    }
}
