using Core.Utilities.Results;
using DataAccess.Helper;

namespace Business.Abstract
{
    public interface IQRService
    {
        IDataResult<CategoryTree> GetQRMenuCategories();
        IDataResult<CategoryTree> GetQRMenuCategoriesWithProduct();
        IDataResult<CategoryTree> GetQRMenuWithQRCode(int userId);
    }
}
