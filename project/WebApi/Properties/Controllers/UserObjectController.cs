using BLL;
using COMMON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class UserObjectController : ApiController
    {
       // [HttpPost]
        public void PostObjects([FromBody]List<COMimageObject> objs, int id, int catid)
        {
        
            try
            {
                COMimage img = new COMimage();
                img.UserId = id;
                img.CategoryID = catid;
                img.URL = "";
                int imageid=BLLgoogleVision.UserImageStorageAndDB(img,objs[0].VoiceURL);
                Dictionary<string, int> dic = new Dictionary<string, int>();
                foreach (COMimageObject item in objs)
                {
                    item.ImageID = imageid;
                    item.VoiceURL = BLLtextToSpeach.VoiceStorage(id,catid, BLLtextToSpeach.TextToSpeach(item.Name), dic);
                    BLLobject.AddObject(item);
                }
                //
            }
            catch (Exception)
            {
                //return BadRequest();
                throw;
            }
        }

    }
}
