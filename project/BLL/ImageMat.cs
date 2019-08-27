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
        public static  void CreateMat()
        {
            int counter = 0;
            ImageWithObject[] array = new ImageWithObject[DALimage.NumImages()];
            var ImageArray = DALimage.Getimages();
            var ObjArray = DALimageObject.Getobjects();
            for (int i = 0; i < DALimage.NumImages(); i++)
            {
                //add image details to array
                array[i] = new ImageWithObject();
                //initialize imagesObjects list
                array[i].imageobjects = new List<COMimageObject>();
                array[i].image = new COMimage();
                array[i].image.ImageID = ImageArray[i].ImageID;
                array[i].image.CategoryID = ImageArray[i].CategoryID;
                array[i].image.URL = ImageArray[i].URL;
                //add image objects to array
                while ((counter<ObjArray.Count) && (ImageArray[i].ImageID == ObjArray[counter].ImageID))
                {
                    COMimageObject imgObj = new COMimageObject();
                    imgObj.ImageID = ObjArray[counter].ImageID;
                    imgObj.Name = ObjArray[counter].Name;
                    imgObj.ObjectId = ObjArray[counter].ObjectId;
                    imgObj.X1 = ObjArray[counter].X1;
                    imgObj.X2 = ObjArray[counter].X2;
                    imgObj.Y1 = ObjArray[counter].Y1;
                    imgObj.Y2 = ObjArray[counter].Y2;
                    array[i].imageobjects.Add(imgObj);
                    counter++;  //increases pointer to the next obj of the current image
                }
            }
        }
    }
}
