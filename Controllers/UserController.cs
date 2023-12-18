using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
namespace Controllers{

[ApiController]
[Route("[controller]")]
// [Authorize(Policy = "Manager")]

public class UserController : ControllerBase
{
    private long userId;
    IUserService UserServise;


    public UserController(IUserService UserServise, IHttpContextAccessor httpContextAccessor)
    {
        this.UserServise = UserServise;
        this.userId = long.Parse(httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value ?? "");
    }

    [HttpGet]
    [Authorize(Policy = "Manager")]
    public ActionResult<List<TaskUser>> GetAll() =>
        UserServise.GetAll();


    [HttpGet("{id}")]
    [Authorize(Policy = "Agent")]
        public ActionResult<TaskUser> GetById()
        {
            var user = UserServise.GetById(userId);

            if (user == null)
                return NotFound();

            return user;
        }

    [HttpPost]
    [Authorize(Policy = "Manager")]
        public IActionResult Create(TaskUser user)
        {
            UserServise.Add(userId,user);
            return CreatedAtAction(nameof(Create), new {id=user.UserId}, user);

        }

    [HttpPut("{id}")]
    [Authorize(Policy = "Agent")]
        public IActionResult Update(int id, TaskUser user)
        {
            if (id != user.UserId)
                return BadRequest();

            var MyUser = UserServise.GetById(userId);
            if (MyUser is null)
                return  NotFound();

            UserServise.Update(userId,user);

            return NoContent();
        } 

    [HttpDelete("{id}")]
    [Authorize(Policy = "Manager")]
        public IActionResult Delete(int id)
        
        {
            System.Console.WriteLine("id:"+id);
            System.Console.WriteLine("ddddd");
            var user = UserServise.GetById(userId);
            System.Console.WriteLine("uuuuuu  "+user.UserId);
            System.Console.WriteLine("userId "+userId);
            if (user is null)
                return  NotFound();

            UserServise.Delete(userId,id);

            return Content(UserServise.Count(userId).ToString());
        }
    
   } 
    
}
