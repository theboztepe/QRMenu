using Core.DataAccess;
using Entities.Concrete;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    public interface IProductDal : IEntityRepository<Product>
    {
        List<Product> GetCategoryProducts(int userId, int categoryId);
        Product GetUserProduct(int v, int productId);
    }
}
