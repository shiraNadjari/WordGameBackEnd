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

        public static string pathCred;

        public static string Storage(int catId, string URL, Dictionary<string, int> categoriesCounter, bool IsMainImg = false)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\keys\wordproject-29b2e0d3e0d5.json");
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
   
        public static string UserImageStorage(COMimage image,string base64)
        {
            int counter = BLLimage.Getimages().FindAll(img => img.UserId == image.UserId).Count;

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\keys\wordproject-29b2e0d3e0d5.json");


            string imageName = BLLuser.GetUserById(image.UserId).CategoryName + counter + ".jpg";
            string path=System.IO.Path.GetTempFileName();
            byte[] byte1 = Convert.FromBase64String(base64);
            try
            {
                File.WriteAllBytes(path, byte1);
            }
            catch(Exception e)
            {
                throw (e);
            }
            //string imageName = "bla2";
            string bucketName = "usersimages";
            var storage = StorageClient.Create();
            using (var f = File.OpenRead(path))
                try
                {
                    var res = storage.UploadObject(bucketName, imageName, null, f);
                        //(bucketName +"/"+ folderName, imageName, null, f);
                    image.URL="https://storage.googleapis.com/" + bucketName + "/" + imageName;
                }
                catch (Exception)
                {
                    throw;
                }
            return image.URL;
        }
        //get btm image , rotate it and return byte[]
        public static byte[] RotateImage(System.Drawing.Bitmap btm)
        {
            byte[] a=null;
            System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
            //return (byte[])converter.ConvertTo(img, typeof(byte[]));
            if (btm != null)
            {
                btm.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipY);
                btm.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipX);

                //convert btm to image-Google.Cloud.Vision.V1
                a = (byte[])converter.ConvertTo(btm, typeof(byte[]));
            }
            return a;
        }
        public static List<string> VisionApi(int categoryId, int UserId, string URL, Dictionary<string, int> categoriesCounter, Dictionary<string, int> voicesCounter)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\keys\wordproject-29b2e0d3e0d5.json");
            // Instantiates a client
            var client = ImageAnnotatorClient.Create();
            // Load the image file into memory
            var image = Image.FromFile(URL);
            // Performs label detection on the image file
            var response = client.DetectLocalizedObjects(image);//
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
                //DALimageObject.Refresh();
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
                        obj.VoiceURL = BLLtextToSpeach.VoiceStorage(UserId,BLLimage.GetImageById(obj.ImageID).CategoryID, BLLtextToSpeach.TextToSpeach(obj.Name), voicesCounter);

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
            return ans;
        }

        //send image to vision api and return all objects detected
        //wirhout insert them into database
        public static List<COMimageObject> CustomVisionApi(COMimage img,string base64)
        {
            //Directory.GetCurrentDirectory() +
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS",  @"C:\keys\wordproject-29b2e0d3e0d5.json"); 
            // Instantiates a client
            var client = ImageAnnotatorClient.Create();
            // Load the image file into memory
            byte[] byteBuffer = Convert.FromBase64String(base64);
            System.Drawing.Bitmap bitmap1 = null;
            //convert byte[] to bitmap
            MemoryStream memoryStream = new MemoryStream(byteBuffer);
            memoryStream.Position = 0;
            bitmap1 = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromStream(memoryStream);
            memoryStream.Close();
            memoryStream = null;
           // byteBuffer = null;
            //rotate the image-bitmap , return byte[]
          //  var res = RotateImage(bitmap1);
            var image = Image.FromBytes(byteBuffer);//convert to image
            // Performs label detection on the image file
            var response = client.DetectLocalizedObjects(image);
            List<COMimageObject> objects = new List<COMimageObject>();
            foreach (var annotation in response)
            {
                COMimageObject obj = new COMimageObject();
                obj.ImageID = img.ImageID;
                obj.Name = annotation.Name;
                obj.VoiceURL = "";
                obj.X1 = annotation.BoundingPoly.NormalizedVertices[0].X;
                obj.X2 = annotation.BoundingPoly.NormalizedVertices[1].X;
                obj.X3 = annotation.BoundingPoly.NormalizedVertices[2].X;
                obj.X4 = annotation.BoundingPoly.NormalizedVertices[3].X;
                obj.Y1 = annotation.BoundingPoly.NormalizedVertices[0].Y;
                obj.Y2 = annotation.BoundingPoly.NormalizedVertices[1].Y;
                obj.Y3 = annotation.BoundingPoly.NormalizedVertices[2].Y;
                obj.Y4 = annotation.BoundingPoly.NormalizedVertices[3].Y;
                objects.Add(obj);
            }
            return objects;
        }

        //storage image and insert it into atabase without
        //insert its objects into db yet.
        public static int UserImageStorageAndDB(COMimage img,string base64)
        {
            int imgId;
            try
            {
                //image storage
                img.URL = UserImageStorage(img,base64);
                //insert image into db
                //DALimageObject.Refresh();
                img.BeginIndex = BLLobject.GetObjects().Count;
                DALimage.Addimage(img);//insert image into db
                imgId = DALimage.GetImageIdByURL(img.URL);
            }
            catch (Exception)
            {
                throw;
            }
            return imgId;
        }
    }
}
