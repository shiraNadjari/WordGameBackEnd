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
    public class ImagesController : ApiController
    {

        public IHttpActionResult GetimageById(int id)
        {
            COMimage img = BLLimage.GetImageById(id);
            if (img == null)
            {
                return BadRequest("image does not exist");
            }
            return Ok(img);
        }

        public List<COMimage> Getimages()
        {
            return BLLimage.Getimages();
        }
        public IHttpActionResult Postimages(COMimage img)
        {
            COMimage im = BLLimage.GetImageById(img.ImageID);
            if (im != null)
            {
                return BadRequest("image already exist");
            }
            BLLimage.AddImage(img);
            return Ok();
        }
        public void Deleteimage(int id)
        {
            BLLimage.RemoveImage(id);
        }
    }
}
