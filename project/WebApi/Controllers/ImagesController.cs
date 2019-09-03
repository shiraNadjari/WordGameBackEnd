using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using COMMON;
using BLL;
using WindowsFormsApp1;

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

        public IHttpActionResult GetTenNextImages(int id,int time)
        {
            //time is current page number in this category images
            List<COMimage> list = BLLimage.GetTenNextImages(id, time);
            if (list == null)
                return BadRequest("fail to load images");
            if (list.Count > 0)
                return Ok(ImageMat.CreateMat(list));
            else
                return BadRequest("no more pictures for this category.");
        }

        public IHttpActionResult PostImage(COMimage img)
        {
            COMimage im = BLLimage.GetImageById(img.ImageID);
            if (im != null)
            {
                return BadRequest("image already exist");
            }
            BLLimage.AddImage(img,Form1.categoriesCounter);
            return Ok();
        }

        public void DeleteImage(int id)
        {
            BLLimage.RemoveImage(id);
        }
    }
}
