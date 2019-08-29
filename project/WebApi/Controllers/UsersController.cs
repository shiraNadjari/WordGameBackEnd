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
    public class UsersController : ApiController
    {
        public IHttpActionResult GetUserById(int id)
        {
            COMuser user = BLLuser.GetUserById(id);
            if (user == null)
            {
                return BadRequest("user does not exist");
            }
            return Ok(user);
        }

        public List<COMuser> GetUsers()
        {
            return BLLuser.GetUsers();
        }

        public IHttpActionResult PostUser(COMuser user)
        {
            COMuser u = BLLuser.GetUserById(user.UserId);
            if (u != null)
            {
                return BadRequest("user already exist");
            }
            BLLuser.AddUser(user);
            return Ok();
        }

        public void DeleteUser(int id)
        {
            BLLuser.RemoveUser(id);
        }
    }
}
