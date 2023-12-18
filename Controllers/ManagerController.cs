using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Models;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using Services;


namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoreManagerController: ControllerBase
    {
        private List<TaskUser> users;

            IUserService UserServise;

        public StoreManagerController(IUserService UserServise)
        {
            this.UserServise = UserServise;
            users=UserServise.GetAll();
            // users = new List<TaskUser>
            // {
            //     new TaskUser { UserId = 1, Username = "a", Password = "a", Manager = true},
            //     new TaskUser { UserId = 2, Username = "b", Password = "b",Manager=false},
            //     new TaskUser { UserId = 3, Username = "Yaakov", Password = "Y1234#"}
            // };
        }
        [HttpPost]
        public ActionResult<String> Login([FromBody] TaskUser User)
        {
            System.Console.WriteLine(User.Username);
            var dt = DateTime.Now;
            System.Console.WriteLine(users.ToString());
            var user = users.FirstOrDefault(u =>
                u.Username == User.Username 
                && u.Password == User.Password
            );        

            System.Console.WriteLine(user.Username);

            if (user == null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim("UserType", user.Manager ? "Manager" : "Agent"),
                new Claim("userId", user.UserId.ToString()),
            };

            if(user.Manager)
                claims.Add(new Claim("UserType","Agent"));
            var token = TaskTokenService.GetToken(claims);
            System.Console.WriteLine(TaskTokenService.WriteToken(token));
            return new OkObjectResult(TaskTokenService.WriteToken(token));
        }
    }
}