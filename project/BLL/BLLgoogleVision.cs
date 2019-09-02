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
            string imageName = BLLcategory.GetCategoryById(categoryId).CategoryName+counter++;
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
            // if image in storage
            if (!IsException)
            {
                //insert image info db
                COMimage img = new COMimage();
                img.CategoryID = categoryId;
                img.URL = imageURL;
                img.UserId = UserId;
                DALimage.Addimage(img);
                int imgId = DALimage.GetImageIdByURL(imageURL);
                
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

                            obj.X1 = pop(x);
                            obj.Y1 = pop(y);
                            obj.X2 = pop(x);
                            obj.Y2 = pop(y);
                            obj.X3 = pop(x);
                            obj.Y3 = pop(y);
                            obj.X4 = pop(x);
                            obj.Y4 = pop(y);
                            DALimageObject.AddObject(obj);
                            ans.Add(annotation.Name);
                        }
                    }
                }
            }

            //{ { "mid": "/m/02xwb", "name": "Fruit", "score": 0.83356, "boundingPoly": { "normalizedVertices": [ { "x": 0.8784594, "y": 0.02347669 }, { "x": 0.994234145, "y": 0.02347669 }, { "x": 0.994234145, "y": 0.374120831 }, { "x": 0.8784594, "y": 0.374120831 } ] } }}


            return ans;

        }
        public void ReadJson(Object json)
        {

        }
    }
}
