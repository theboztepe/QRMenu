using Core.Utilities.Results;
using Entities.Concrete;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IProductService
    {
        #region CRUD
        IDataResult<List<Product>> GetCategoryProducts(int categoryId);
        IResult Add(Product product);
        IResult Update(Product product);
        IResult Remove(Product product);
        #endregion
    }
}
