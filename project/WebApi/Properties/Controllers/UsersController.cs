using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using COMMON;
using BLL;
using System.Web;

namespace WebApi.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        public IHttpActionResult GetUserById(int id)
        {
            COMuser user = BLLuser.GetUserById(id);
            if (user == null)
            {
                return BadRequest("user does not exist");
            }
            return Ok(user);
        }

        public List<COMuser> GetUsers()
        {
            return BLLuser.GetUsers();
        }

        //insert image to db and storage without inserting its objects.
        //return objects list
        //[HttpPost]
        //

        //public static WebResult<int> InsertImages()
        //{
        //    HttpResponseMessage response = new HttpResponseMessage();
        //    var httpRequest = HttpContext.Current.Request;
        //    string temp = "~/UploadFile/";
        //    string uploaded_image;
        //    if (httpRequest.Files.Count > 0)
        //    {
        //        for (var i = 0; i < httpRequest.Files.Count; i++)
        //        {
        //            var postedFile = httpRequest.Files[i];
        //            var filePath = HttpContext.Current.Server.MapPath(temp + postedFile.FileName);
        //            postedFile.SaveAs(filePath);
        //            uploaded_image = SendToStorage(filePath);
        //            InitImageDetailsAsync(filePath);
        //            InitImageDetailsAsync(filePath);
        //        }

        //    }
        //    return new WebResult<int>()
        //    {
        //        Status = true,
        //        Message = "Success",
        //        Value = 1
        //    };
        //    //return response;
        //}

        //[HttpPost]
        //public void PostInsertImageReturnObjects(int id, int catId)
        //{
        //    var httpRequest = HttpContext.Current.Request;

        //   var re= httpRequest.GetBufferlessInputStream();
        //    ////=httpRequest.Params["image_data"];
        //   // re = httpRequest.Form["image_data"];

        //    string temp = "~/UploadFile/";
        //    string uploaded_image;
        //    if (httpRequest.Files.Count > 0)
        //    {
        //        for (var i = 0; i < httpRequest.Files.Count; i++)
        //        {
        //            var postedFile = httpRequest.Files[i];
        //            var filePath = HttpContext.Current.Server.MapPath(temp + postedFile.FileName);
        //            postedFile.SaveAs(filePath);
        //            //List < COMimageObject >

        //        }
        //    }
        //    /////
        //    string fileName = string.Empty;
        //    try
        //    {
        //        if (HttpContext.Current.Request.Files.AllKeys.Any())
        //        {
        //            // Get the uploaded image from the Files collection
        //            var httpPostedFile = HttpContext.Current.Request.Files["image_data"];
        //            fileName = httpPostedFile.FileName;
        //        }
        //        //if (httpPostedFile != null)
        //        //{
        //        //    // OBtient le path du fichier 
        //        //    fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedFiles"), httpPostedFile.FileName);

        //        //    // Sauvegarde du fichier dans UploadedFiles sur le serveur
        //        //    httpPostedFile.SaveAs(fileSavePath);
        //        //}

        //        //string base64 = MyBase64.URL.Substring(23);
        //        COMimage img = new COMimage();
        //        img.UserId = id;
        //        img.CategoryID = catId;
        //        img.URL = "";
        //        //List<COMimageObject> objs = BLLimage.GetImageFromUserReturnObjectsList(img, MyBase64.URL);

        //        // return objs;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        public List<COMimageObject> PostInsertImageReturnObjects([FromBody]COMimage MyBase64, int id, int catId)
        {
            try
            {
                //string base64 = MyBase64.URL.Substring(23);
                COMimage img = new COMimage();
                img.UserId = id;
                img.CategoryID = catId;
                img.URL = "";
                List<COMimageObject> objs = BLLimage.GetImageFromUserReturnObjectsList(img, MyBase64.URL);

                return objs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void PostObjects([FromBody] List<COMimageObject> objs, int id,int catid)
        {
            try
            {
                COMimage img = new COMimage();
                img.UserId = id;
                img.CategoryID = catid;
                img.URL = "";
                //BLLgoogleVision.UserImageStorageAndDB(img,objs[0].VoiceURL);
                Dictionary<string, int> dic = new Dictionary<string, int>();
                //foreach (COMimageObject item in objs)
                //{
                //    item.ImageID = id;
                //    item.VoiceURL = BLLtextToSpeach.VoiceStorage(BLLimage.GetImageById(id).UserId, BLLimage.GetImageById(id).CategoryID, BLLtextToSpeach.TextToSpeach(item.Name), dic);
                //    BLLobject.AddObject(item);
                //}
                //return Ok();
            }
            catch (Exception)
            {
                //return BadRequest();
                throw;
            }
        }

        //public IHttpActionResult PostUser([FromBody]COMuser user)
        //{
        //    COMuser u = BLLuser.GetUserById(user.UserId);
        //    if (u != null)
        //    {
        //        return BadRequest("user already exist");
        //    }
        //    BLLuser.AddUser(user);
        //    return Ok();
        //}

        public void DeleteUser(int id)
        {
            BLLuser.RemoveUser(id);
        }
    }
}
