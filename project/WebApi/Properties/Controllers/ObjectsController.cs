using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using COMMON;
using BLL;

namespace WebApi.Controllers
{
    public class ObjectsController : ApiController
    {
        public IHttpActionResult GetObjectById(int id)
        {
            COMimageObject obj = BLLobject.GetObjectById(id);
            if (obj == null)
            {
                return BadRequest("object does not exist");
            }
            return Ok(obj);
        }

        public List<COMimageObject> GetObjects()
        {
            return BLLobject.GetObjects();
        }

        public IHttpActionResult PostObject([FromBody]COMimageObject obj)
        {
            COMimageObject o = BLLobject.GetObjectById(obj.ObjectId);
            if (o != null)
            {
                return BadRequest("object already exist");
            }
            BLLobject.AddObject(obj);
            return Ok();
        }

        public void DeleteObject(int id)
        {
            BLLobject.RemoveObject(id);
        }
    }
}

