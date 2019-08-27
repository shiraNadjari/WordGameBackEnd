using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMON;
using DAL;

namespace BLL
{
   public class BLLimage
    {
        public static void AddImage(COMimage img)
        {
            DALimage.Addimage(img);
        }

        public static COMimage GetImageById(int id)
        {
            return DALimage.GetImageById(id);
        }

        public static List<COMimage> Getimages()
        {
            return DALimage.Getimages();
        }

        public static List<COMimage> GetTenNextImages(int categoryId,int time)
        {
            //ten next images in this category- in accordance to time- current page number
            List<COMimage> list = new List<COMimage>();
            int count = DALimage.GetTenNextImages(categoryId).Count;
            count -= time * 10;
            for (int i = 10*time; i < (count<10? (10 * time)+count:(10*time)+10); i++)
            {
                list.Add(DALimage.GetTenNextImages(categoryId)[i]);
            }
            return list;
        }

        public static void RemoveImage(int id)
        {
            DALimage.Removeimage(id);
        }
    }
}
