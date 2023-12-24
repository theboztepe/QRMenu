using Core.DataAccess;
using DataAccess.Helper;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface IQRDal : IEntityRepository<Category>
    {
        CategoryTree GetQRMenuCategories(int userId);
        CategoryTree GetQRMenuCategoriesWithProduct(int userId, IProductDal productDal);
        CategoryTree GetQRMenuWithQRCode(int userId, IProductDal productDal);
    }
}
