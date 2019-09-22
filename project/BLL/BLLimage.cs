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
        public static List<string> AddImage(COMimage img,Dictionary<string,int> categoriesCounter, Dictionary<string, int> voicesCounter) 
        {
            return BLLgoogleVision.VisionApi(img.CategoryID,img.UserId,img.URL,categoriesCounter,voicesCounter);
        }

        public static List<COMimageObject> GetImageFromUserReturnObjectsList(COMimage img,string base64)
        {
            return BLLgoogleVision.CustomVisionApi(img, base64);
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
