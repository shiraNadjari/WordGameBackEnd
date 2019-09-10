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
using Google.Apis.Auth.OAuth2;
namespace BLL
{
    public class Bounding
    {
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double X3 { get; set; }
        public double X4 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }
        public double Y3 { get; set; }
        public double Y4 { get; set; }
        public Bounding(double x1, double x2, double x3, double x4, double y1, double y2, double y3, double y4)
        {
            X1 = x1;
            X2 = x2;
            X3 = x3;
            X4 = x4;
            Y1 = y1;
            Y2 = y2;
            Y3 = y3;
            Y4 = y4;
        }
        public Bounding(List<double> x, List<double> y) : this(x[0], x[1], x[2], x[3], y[0], y[1], y[2], y[3])
        {

        }
        public Bounding()
        {

        }
        public bool IsEqual(Bounding b)
        {
            if (X1 == b.X1 && X2 == b.X2 && X3 == b.X3
                && X4 == b.X4 && Y1 == b.Y1 && Y2 == b.Y2
                && Y3 == b.Y3 && Y4 == b.Y4)
                return true;
            return false;
        }
    }
    public class BLLgoogleVision
    {

        public static string Storage(int catId, string URL, Dictionary<string, int> categoriesCounter, bool IsMainImg = false)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\wordproject-29b2e0d3e0d5.json");
            // upload the image storage
            //----------------
            string imageName;
            if (!IsMainImg)
                imageName = BLLcategory.GetCategoryById(catId).CategoryName + categoriesCounter[BLLcategory.GetCategoryById(catId).CategoryName]++ + ".jpg";
            else
                imageName = "MAIN" + BLLcategory.GetCategoryById(catId).CategoryName + ".jpg";
            string bucketName = "wordproject";
            var storage = StorageClient.Create();
            using (var f = File.OpenRead(URL))
                try
                {
                    var res = storage.UploadObject(bucketName, imageName, null, f);
                    URL = "https://storage.googleapis.com/" + bucketName + "/" + imageName;
                }
                catch (Exception e)
                {
                    throw e;
                }
            return URL;
        }

        public static string ImageUserStorage(int UserId, string URL, int counter)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\wordproject-29b2e0d3e0d5.json");
            // upload the image storage
            string ImageName = BLLuser.GetUserById(UserId).CategoryName + UserId.ToString() + "-" + counter.ToString() + ".jpg";
            string bucketName = "objectsound";
            var storage = StorageClient.Create();
            using (var f = File.OpenRead(URL))
            {
                try
                {
                    // var res = storage.UploadObject(bucketName, voiceName, null, f);
                    // URL = "https://storage.googleapis.com/" + bucketName + "/" + voiceName;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return URL;
        }

        public static string CustomStorage(int catId, string url, int counter, int userId)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\wordproject-29b2e0d3e0d5.json");
            // upload the image storage
            string imageName = BLLuser.GetUserById(userId).CategoryName + counter.ToString() + ".jpg";
            string bucketName = "wordproject";
            string projectId = "wordproject-249810";
            var storage = StorageClient.Create();
            //storage.CreateBucket(projectId,)
            using (var f = File.OpenRead(url))
                try
                {
                    var res = storage.UploadObject(bucketName, imageName, null, f);
                    url = "https://storage.googleapis.com/" + bucketName + "/" + imageName;
                }
                catch (Exception)
                {
                    throw;
                }
            return url;
        }

        public static void AddUserImage(string url, int userId, int counter)
        {
            COMimage img = new COMimage();
            img.CategoryID = BLLcategory.GetCategoryIdByCategoryName("Custom");
            img.UserId = userId;
            //CustomStorage(img.CategoryID, url, counter);
        }

        public static void createBucket()
        {

        }

        public static List<string> VisionApi(int categoryId, int UserId, string URL, Dictionary<string, int> categoriesCounter, Dictionary<string, int> voicesCounter)
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
                    if (max < countingDic[annotation.Name])
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
                Dictionary<Bounding, string> finalList = new Dictionary<Bounding, string>();

                foreach (var annotation in response)
                {
                    List<double> x = new List<double>();
                    List<double> y = new List<double>();

                    foreach (var item in annotation.BoundingPoly.NormalizedVertices)
                    {
                        x.Add(item.X);
                        y.Add(item.Y);
                    }
                    bool help = false;
                    Bounding bounding = new Bounding(x, y);
                    //if there is item with same bounding box and the existing is common name and the new not- swap them.
                    foreach (var item in finalList)
                    {
                        if (item.Key.IsEqual(bounding))
                        {
                            help = true;
                            if (item.Value == common && annotation.Name != common)
                            {
                                finalList.Remove(item.Key);
                                finalList.Add(bounding, annotation.Name);
                            }
                        }
                    }
                    if (help == false)
                        finalList.Add(bounding, annotation.Name);
                    else
                        help = true;
                    //if (!(BLLobject.CheckObjectExists(x, y, imgId)))
                    //{
                    //    COMimageObject obj = new COMimageObject();
                    //    obj.ImageID = imgId;
                    //    obj.Name = annotation.Name;
                    //    obj.X1 = x[0];
                    //    obj.Y1 = y[0];
                    //    obj.X2 = x[1];
                    //    obj.Y2 = y[1];
                    //    obj.X3 = x[2];
                    //    obj.Y3 = y[2];
                    //    obj.X4 = x[3];
                    //    obj.Y4 = y[3];
                    //    //obj.VoiceURL = BLLtextToSpeach.TextToSpeach(obj.Name);
                    //    try
                    //    {
                    //        obj.VoiceURL = BLLtextToSpeach.VoiceStorage(BLLimage.GetImageById(obj.ImageID).CategoryID, BLLtextToSpeach.TextToSpeach(obj.Name), voicesCounter);

                    //    }
                    //    catch (Exception e)
                    //    {
                    //        throw e;
                    //    }
                    //    DALimageObject.AddObject(obj);
                    //    c++;
                    //    ans.Add(annotation.Name);
                    //}
                }
                foreach (var item in finalList)
                {
                    COMimageObject obj = new COMimageObject();
                    obj.ImageID = imgId;
                    obj.Name = item.Value;
                    obj.X1 = item.Key.X1;
                    obj.X2 = item.Key.X2;
                    obj.X3 = item.Key.X3;
                    obj.X4 = item.Key.X4;
                    obj.Y1 = item.Key.Y1;
                    obj.Y2 = item.Key.Y2;
                    obj.Y3 = item.Key.Y3;
                    obj.Y4 = item.Key.Y4;
                    try
                    {
                        obj.VoiceURL = BLLtextToSpeach.VoiceStorage(BLLimage.GetImageById(obj.ImageID).CategoryID, BLLtextToSpeach.TextToSpeach(obj.Name), voicesCounter);

                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    DALimageObject.AddObject(obj);
                    c++;
                    ans.Add(obj.Name);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            BLLimage.UpdateEndIndex(imgId, img.BeginIndex + c);
            return ans;
        }
    }
}
