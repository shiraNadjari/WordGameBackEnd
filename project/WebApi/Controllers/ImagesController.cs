﻿using System;
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
        public IHttpActionResult GetImages(int id)
        {
            //return BLLimage.Getimages();
            //time is current page number in this category images
            List<COMimage> list = BLLimage.Getimages().FindAll(img => img.UserId == id || img.UserId == 11);
            if (list == null)
                return BadRequest("fail to load images");
            if (list.Count > 0)
                return Ok(ImageMat.CreateMat(list,true));
            else
                return BadRequest("no more pictures for this category.");
    }
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
