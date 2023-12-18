using Models;
using System.Collections.Generic;


namespace Interfaces
{
    public interface IUserService
    {
        List<TaskUser> GetAll();
        TaskUser GetById(long userId);
        void Add(long userId,TaskUser user);
        void Delete(long userId,int id);
        void Update(long userId,TaskUser user);
        int Count(long userId);
    }
}