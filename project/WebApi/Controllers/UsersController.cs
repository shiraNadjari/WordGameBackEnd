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
        [HttpGet]
        public List<COMimageObject> InsertImageReturnObjects(int id, int catId, string url)
        {
            try
            {
                COMimage img = new COMimage();
                img.UserId = id;
                img.CategoryID = catId;
                img.URL = url;
                List<COMimageObject> objs = BLLimage.GetImageFromUserReturnObjectsList(img);
                BLLgoogleVision.UserImageStorageAndDB(img);
                return objs;
            }
            catch (Exception)
            {
                throw;
            }
        }
                                    //imgId
        public static void postObjects(int id, List<COMimageObject> objs)
        {
            try
            {
                Dictionary<string, int> dic = new Dictionary<string, int>();
                foreach (COMimageObject item in objs)
                {
                    item.ImageID = id;
                    item.VoiceURL = BLLtextToSpeach.VoiceStorage(BLLimage.GetImageById(id).UserId,BLLimage.GetImageById(id).CategoryID, BLLtextToSpeach.TextToSpeach(item.Name), dic);
                    BLLobject.AddObject(item);
                }
                //return Ok();
            }
            catch (Exception)
            {
                //return BadRequest();
                throw;
            }
        }

        public IHttpActionResult PostUser(COMuser user)
        {
            COMuser u = BLLuser.GetUserById(user.UserId);
            if (u != null)
            {
                return BadRequest("user already exist");
            }
            BLLuser.AddUser(user);
            return Ok();
        }

        public void DeleteUser(int id)
        {
            BLLuser.RemoveUser(id);
        }
    }
}
