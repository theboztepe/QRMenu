using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<Product, QRMenuContext>, IProductDal
    {
        public Product GetUserProduct(int userId, int productId)
        {
            using QRMenuContext context = new();
            Product result = (from p in context.Products
                              join c in context.Categories on p.CategoryId equals c.Id
                              where c.UserId == userId && p.Id == productId
                              select p).FirstOrDefault();
            return result;
        }

        public List<Product> GetCategoryProducts(int userId, int categoryId)
        {
            using QRMenuContext context = new();
            IQueryable<Product> result = from p in context.Products
                                         join c in context.Categories on p.CategoryId equals c.Id
                                         where c.UserId == userId && p.CategoryId == categoryId
                                         select p;
            return result.ToList();
        }
    }
}
