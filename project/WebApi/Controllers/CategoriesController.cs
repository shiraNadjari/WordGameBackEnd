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
    public class CategoriesController : ApiController
    {
        //[Route("api/GetCategoryById/{id}")]
        //public IHttpActionResult GetCategoryById(int id)
        //{
        //    Comcategory cat = BLLcategory.GetCategoryById(id);
        //    if (cat == null)
        //    {
        //        return BadRequest("category does not exist");
        //    }
        //    return Ok(cat);
        //}

        public List<ComCategory> GetCategories()
        {
            
            return BLLcategory.GetCategories();
        }

        //[Route("api/GetPagesAmountPerCategory/{id}")]
        public int GetPagesAmountPerCategory(int id)
        {
            return BLLcategory.GetPagesAmountPerCategory(id);
        }

        public IHttpActionResult PostCategory(ComCategory com)
        {
            ComCategory c = BLLcategory.GetCategoryById(com.CategoryId);
            if(c!=null)
            {
                return BadRequest("category already exist");
            }
             BLLcategory.AddCategory(com);
             return Ok();
        }
        public void DeleteCategory(int id)
        {
            BLLcategory.RemoveCategory(id);
        }
    }
}
