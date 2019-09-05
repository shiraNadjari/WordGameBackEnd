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

        public static string Storage(int catId,string URL,Dictionary<string,int> categoriesCounter, bool IsMainImg=false)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\wordproject-29b2e0d3e0d5.json");
            // upload the image storage
            //----------------
            string imageName;
            if (!IsMainImg)
                imageName = BLLcategory.GetCategoryById(catId).CategoryName + categoriesCounter[BLLcategory.GetCategoryById(catId).CategoryName]++ + ".jpg";
            else
                imageName = "MAIN"+BLLcategory.GetCategoryById(catId).CategoryName + ".jpg";
            string bucketName = "wordproject";
            var storage = StorageClient.Create();
            using (var f = File.OpenRead(URL))
                try
                {
                    var res = storage.UploadObject(bucketName, imageName, null, f);
                    URL = "https://storage.cloud.google.com/" + bucketName + "/" + imageName;
                }
                catch (Exception e)
                {
                    throw e;
                }
            return URL;
        }

        

        public static List<string> VisionApi(int categoryId,int UserId,string URL,Dictionary<string,int> categoriesCounter, Dictionary<string, int> voicesCounter)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\wordproject-29b2e0d3e0d5.json");
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
            COMimage img = new COMimage();
            string imgUrl;
            List<string> ans = new List<string>();
            int c = 0;
            int imgId = -1;
            try
            {
                imgUrl = Storage(categoryId, URL, categoriesCounter);

                // if image in storage
                //insert image info db
                img.CategoryID = categoryId;
                img.URL = imgUrl;
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
                            obj.VoiceURL = BLLtextToSpeach.TextToSpeach(obj.Name);
                            try
                            {
                                BLLtextToSpeach.VoiceStorage(BLLimage.GetImageById(obj.ImageID).CategoryID, obj.VoiceURL, voicesCounter);

                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                            DALimageObject.AddObject(obj);
                            c++;
                            ans.Add(annotation.Name);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            
            BLLimage.UpdateEndIndex(imgId, img.BeginIndex+c);
            return ans;
        }
    }
}
