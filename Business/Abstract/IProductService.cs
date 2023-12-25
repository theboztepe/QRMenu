using Core.Utilities.Results;
using Entities.Concrete;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IProductService
    {
        #region CRUD
        IDataResult<Product> GetUserProduct(int productId);
        IResult Add(Product product);
        IResult Update(Product product);
        IResult Remove(Product product);
        IDataResult<List<Product>> GetCategoryProducts(int categoryId);
        #endregion
    }
}
