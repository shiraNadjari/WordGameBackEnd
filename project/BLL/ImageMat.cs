using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMON;
using DAL;
namespace BLL
{
   public class ImageMat
    {
        //get list of images return for each image list of objects
        public static List<ImageWithObject> CreateMat(List<COMimage> imageList,bool IsAllImages=false)
        {
            if (imageList.Count() <= 0)
                return null;
            //answer list
            List<ImageWithObject> ImagesAndObjectsMat = new List<ImageWithObject>();
            //list with all objects of all images from all categories
            List<COMimageObject> ObjList = DALimageObject.Getobjects();
            foreach (COMimage img in imageList)
            {
                ImageWithObject imageAndObject = new ImageWithObject();
                imageAndObject.image = new COMimage();
                imageAndObject.imageObjects = new List<COMimageObject>();
                imageAndObject.image.ImageID = img.ImageID;
                imageAndObject.image.URL = img.URL;
                imageAndObject.image.BeginIndex = img.BeginIndex;
                imageAndObject.image.EndIndex = img.EndIndex;
                imageAndObject.image.CategoryID = img.CategoryID;
                List<COMimageObject> objs = BLLobject.GetObjects().FindAll(obj => obj.ImageID == img.ImageID);
                objs.ForEach(obj => imageAndObject.imageObjects.Add(obj));
                ImagesAndObjectsMat.Add(imageAndObject);
            }
            return ImagesAndObjectsMat;
        }
    }
}
