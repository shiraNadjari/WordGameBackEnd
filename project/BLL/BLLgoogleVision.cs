using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Vision.V1;
using System.IO;
using Google.Cloud.Storage.V1;
using DAL;
using COMMON;

namespace BLL
{
    public class BLLgoogleVision
    {
        private static double pop(List<double> list)
        {
            double x = list.Last();
            list.RemoveAt(list.Count - 1);
            return x;
        }
        static int counter = 1;
        public static List<string> VisionApi(int categoryId,int UserId,string URL)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "C:\\wordproject-29b2e0d3e0d5.json");
            // Instantiates a client
            var client = ImageAnnotatorClient.Create();

            // Load the image file into memory
            var image = Image.FromFile(URL);
            // Performs label detection on the image file
            var response = client.DetectLocalizedObjects(image);
            //found most common word in objects list 
            Dictionary<string, int> countingDic = new Dictionary<string, int>();
            string common = string.Empty;
            foreach (var annotation in response)
            {
                if (countingDic.ContainsKey(annotation.Name))
                {
                    int max = 1;
                    countingDic[annotation.Name]++;
                    if(max < countingDic[annotation.Name])
                    {
                        max = countingDic[annotation.Name];
                        common = annotation.Name;
                    }

                }
                else
                {
                    countingDic.Add(annotation.Name, 1);
                }
            }
            string imageURL;
            bool IsException = false;
            // upload the image storage
            //----------------
            string imageName = BLLcategory.GetCategoryById(categoryId).CategoryName+counter++ +".jpg";
            string bucketName = "worproject";
            var storage = StorageClient.Create();
            using (var f = File.OpenRead(URL))
            try
            {
                var res=storage.UploadObject(bucketName, imageName, null, f);
                    imageURL = "https://storage.cloud.google.com/" + bucketName + "/" + imageName + ".jpg";
            }
            catch (Exception e)
            {
                IsException = true;
                throw (e);
            }
            List<string> ans = new List<string>();
            COMimage img = new COMimage();
            int c = 0;
            int imgId = -1;
            // if image in storage
            if (!IsException)
            {
                //insert image info db
                img.CategoryID = categoryId;
                img.URL = imageURL;
                img.UserId = UserId;
                DALimageObject.Refresh();
                img.BeginIndex = BLLobject.GetObjects().Count;
                DALimage.Addimage(img);
                imgId = DALimage.GetImageIdByURL(img.URL);
                //insert objects into db
                foreach (var annotation in response)
                {
                    if (annotation.Name != common)
                    {
                        List<double> x = new List<double>();
                        List<double> y = new List<double>();

                        foreach (var item in annotation.BoundingPoly.NormalizedVertices)
                        {
                            x.Add(item.X);
                            y.Add(item.Y);
                        }
                        if (!(BLLobject.CheckObjectExists(x, y, imgId)))
                        {
                            COMimageObject obj = new COMimageObject();
                            obj.ImageID = imgId;
                            obj.Name = annotation.Name;

                            obj.X1 = x[0];
                            obj.Y1 = y[0];
                            obj.X2 = x[1];
                            obj.Y2 = y[1];
                            obj.X3 = x[2];
                            obj.Y3 = y[2];
                            obj.X4 = x[3];
                            obj.Y4 = y[3];
                            DALimageObject.AddObject(obj);
                            c++;
                            ans.Add(annotation.Name);
                        }
                    }
                }
            }
            BLLimage.UpdateEndIndex(imgId, img.BeginIndex+c);
            return ans;
        }
        public void ReadJson(Object json)
        {

        }
    }
}
