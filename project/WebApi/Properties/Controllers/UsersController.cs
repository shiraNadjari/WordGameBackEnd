using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using COMMON;
using BLL;
//using WindowsFormsApp1;

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
     
        public List<COMimageObject> PostInsertImageReturnObjects([FromBody]COMimage MyBase64, int id, int catId )
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
