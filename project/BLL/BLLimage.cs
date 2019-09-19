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
            //DALimage.Addimage(img);
            return BLLgoogleVision.VisionApi(img.CategoryID,img.UserId,img.URL,categoriesCounter,voicesCounter);
        }

        public static List<COMimageObject> GetImageFromUserReturnObjectsList(COMimage img,string base64)
        {
            //GetImageFromUserReturnObjectsList
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

        public static List<COMimage> GetTwelveNextImages(int categoryId,int time)
        {
            //ten next images in this category- in accordance to time- current page number
            List<COMimage> list = new List<COMimage>();
            int count = DALimage.GetTwelveNextImages(categoryId).Count;
            count -= time * 12;
            for (int i = 12*time; i < (count<12? (12 * time)+count:(12*time)+12); i++)
            {
                list.Add(DALimage.GetTwelveNextImages(categoryId)[i]);
            }
            return list;
        }

        public static void UpdateEndIndex(int imgId,int end)
        {
            //DALimage.UpdateEndIndex(imgId, end);
        }

        public static void UpdateURL(int imgId,string url)
        {
            DALimage.UpdateURL(imgId, url);
        }

        public static void RemoveImage(int id)
        {
            DALimage.Removeimage(id);
        }
    }
}
