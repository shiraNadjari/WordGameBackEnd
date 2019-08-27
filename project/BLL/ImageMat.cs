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
            int j = 0, counter = 0;
            ImageWithObject[] matrix = new ImageWithObject[DALimage.NumImages()];
            var ImageArray = DALimage.Getimages();
            var ObjArray = DALimageObject.Getobjects();
            for (int i = 0; i < DALimage.NumImages(); i++)
            {
                //add image details to array
                matrix[i].image = ImageArray[i];
                while (ImageArray[i].ImageID!= ObjArray[counter].ImageID)
                {
                    matrix[i].imageobjects[j] = ObjArray[counter];
                    counter++;
                    j++;
                }
                j = 0;

                  
                
            }


        }
    }
}
