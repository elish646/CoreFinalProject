using Models;
using Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

namespace Services
{
    public class UserServise : IUserService
    {

        List<TaskUser> Users { get; }
        private IWebHostEnvironment  webHost;
        private string filePath;
        public UserServise(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "User.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                Users = JsonSerializer.Deserialize<List<TaskUser>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(Users));
        }

        public List<TaskUser> GetAll(){
            return Users.ToList();

        }
        public TaskUser GetById(long userId){
           return Users.FirstOrDefault(t =>t.UserId==userId);
        }
        public void Add(long userId,TaskUser user)
        {
            user.UserId = Users.Count() + 1;
          //  user.AgentId=userId;
            Users.Add(user);
            saveToFile();
        }
    
   

    
        public void Update(long userId,TaskUser user)
        {
            var index = Users.FindIndex(p =>p.UserId == user.UserId);
            if (index == -1)
                return;

            Users[index] = user;
            saveToFile();
        }

    
        public void Delete(long userId,int id)
        {

            System.Console.WriteLine("iiiiddd:"+id);
            System.Console.WriteLine("userIduserId: "+userId);
            var user = GetById(id);
            if (user is null)
                return;

            Users.Remove(user);
            saveToFile();
        }

            public int Count(long userId){
                return GetAll().Count();
            } 
    }

    
}