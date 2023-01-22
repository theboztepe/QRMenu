using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCategoryDal : EfEntityRepositoryBase<Category, QRMenuContext>, ICategoryDal
    {
        public List<Category> GetUserCategories(int userId, int topCategoryId)
        {
            using QRMenuContext context = new();
            IQueryable<Category> result = from c in context.Categories
                                          where c.UserId == userId && c.TopCategoryId == topCategoryId
                                          select c;
            return result.ToList();
        }
    }
}
