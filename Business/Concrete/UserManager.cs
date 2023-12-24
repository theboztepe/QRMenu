using Business.Abstract;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using Entities.DTOs.QR;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public void Add(User user)
        {
            _userDal.Add(user);
        }

        public User GetByMail(string email)
        {
            return _userDal.Get(u => u.Email == email);
        }

        public User GetByUsername(string username)
        {
            return _userDal.Get(u => u.Username == username);
        }

        public User GetById(int id)
        {
            return _userDal.Get(u => u.Id == id);
        }

        public User GetQRCodeForUserId(QRCodeDto qrCode)
        {
            return _userDal.Get(u => u.QRCode == qrCode.QRCode);
        }

        public User GetQRMenuCode(int userId)
        {
            return _userDal.Get(u => u.Id == userId);
        }
    }
}