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
        public IHttpActionResult GetTwelveNextImages(int id, int time)
        {
            //time is current page number in this category images
            List<COMimage> list = BLLimage.GetTwelveNextImages(id, time);
            if (list == null)
                return BadRequest("fail to load images");
            if (list.Count > 0)
                return Ok(ImageMat.CreateMat(list));
            else
                return BadRequest("no more pictures for this category.");
        }

        [Route("api/Images/GetImageById/{id}")]
        public IHttpActionResult GetImageById(int id)
        {
            COMimage img = BLLimage.GetImageById(id);
            if (img == null)
            {
                return BadRequest("image does not exist");
            }
            return Ok(img);
        }

        [Route("api/Images/GetImages/{id}")]
        public IHttpActionResult GetImages(int id,int catid)
        {
            List<COMimage> list;
            //if the call was made to get the users image
            if (catid != -1)
            {
                list = BLLimage.Getimages().FindAll(img => img.CategoryID == catid);
            }
            else
            {
                //time is current page number in this category images
                list = BLLimage.Getimages().FindAll(img => img.UserId == id);
            }
            if (list == null)
                return BadRequest("fail to load images");
            if (list.Count > 0)
                return Ok(ImageMat.CreateMat(list,true));
            else
                return BadRequest("no more pictures for this category.");
    }

        public IHttpActionResult PostImage([FromBody] COMimage img)
         {
            COMimage im = BLLimage.GetImageById(img.ImageID);
            if (im != null)
            {
                return BadRequest("image already exist");
            }
            BLLimage.AddImage(img, Form1.categoriesCounter, Form1.voicesCounter);
            return Ok();
        }

        public void DeleteImage(int id)
        {
            BLLimage.RemoveImage(id);
        }
    }
}
