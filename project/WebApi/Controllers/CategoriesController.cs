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
        public IHttpActionResult GetCategoryById(int id)
        {
            Comcategory cat = BLLcategory.GetCategoryById(id);
            if (cat == null)
            {
                return BadRequest("category does not exist");
            }
            return Ok(cat);
        }

        public List<Comcategory> GetCategories()
        {
            return BLLcategory.GetCategories();
        }
        public IHttpActionResult PostCategory(Comcategory com)
        {
            Comcategory c = BLLcategory.GetCategoryById(com.CategoryId);
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
