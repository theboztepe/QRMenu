using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<Product, QRMenuContext>, IProductDal
    {
        public List<Product> GetCategoryProducts(int userId, int categoryId)
        {
            using QRMenuContext context = new();
            IQueryable<Product> result = from p in context.Products
                                         join c in context.Categories on p.CategoryId equals c.Id
                                         where c.UserId == userId
                                         select p;
            return result.ToList();
        }
    }
}
