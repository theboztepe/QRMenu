using Core.Entities.Concrete;
using Entities.DTOs.QR;

namespace Business.Abstract
{
    public interface IUserService
    {
        void Add(User user);
        User GetByMail(string email);
        User GetByUsername(string username);
        User GetById(int id);
        User GetQRMenuCode(int v);
        User GetQRCodeForUserId(QRCodeDto qrCode);
    }
}