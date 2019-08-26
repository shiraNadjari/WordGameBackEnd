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

        public static void RemoveImage(int id)
        {
            DALimage.Removeimage(id);
        }
    }
}
