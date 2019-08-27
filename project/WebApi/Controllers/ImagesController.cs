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

        public IHttpActionResult GetImageById(int id)
        {
            COMimage img = BLLimage.GetImageById(id);
            if (img == null)
            {
                return BadRequest("image does not exist");
            }
            return Ok(img);
        }

        public List<COMimage> GetImages()
        {
            return BLLimage.Getimages();
        }

        public List<ImageWithObject> GetTenNextImages(int id,int time)
        {
            //time is current page number in this category images
            return ImageMat.CreateMat(BLLimage.GetTenNextImages(id, time)); ;
        }

        public IHttpActionResult PostImages(COMimage img)
        {
            COMimage im = BLLimage.GetImageById(img.ImageID);
            if (im != null)
            {
                return BadRequest("image already exist");
            }
            BLLimage.AddImage(img);
            return Ok();
        }

        public void DeleteImage(int id)
        {
            BLLimage.RemoveImage(id);
        }
    }
}
