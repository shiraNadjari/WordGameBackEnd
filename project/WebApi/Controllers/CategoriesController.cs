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
    public class CategoriesController : ApiController
    {
        [Route("api/Categories/GetCategoryById/{id}")]
        public IHttpActionResult GetCategoryById(int id)
        {
            COMCategory cat = BLLcategory.GetCategoryById(id);
            if (cat == null)
            {
                return BadRequest("category does not exist");
            }
            return Ok(cat);
        }

        public List<COMCategory> GetCategories()
        {
            return BLLcategory.GetCategories();
        }
        [Route("api/Categories/GetPagesAmountPerCategory/{id}")]
        public int GetPagesAmountPerCategory(int id)
        {
            int num;
            num = BLLcategory.GetPagesAmountPerCategory(id);
            return num;
        }

        public IHttpActionResult PostCategory(COMCategory com)
        {
            COMCategory c = BLLcategory.GetCategoryById(com.CategoryId);
            if (c != null)
            {
                return BadRequest("category already exist");
            }
            BLLcategory.AddCategory(com, Form1.categoriesCounter);
            return Ok();
        }
        public void DeleteCategory(int id)
        {
            BLLcategory.RemoveCategory(id);
        }
    }
}
