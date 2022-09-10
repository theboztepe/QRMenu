using Core.Entities.Concrete;

namespace Business.Abstract
{
    public interface IUserService
    {
        void Add(User user);
        User GetByMail(string email);
        User GetByUsername(string username);
        User GetById(int id);
    }
}