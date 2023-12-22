using Core.DataAccess;
using DataAccess.Helper;
using Entities.Concrete;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    public interface ICategoryDal : IEntityRepository<Category>
    {
        List<Category> GetUserCategories(int userId, int topCategoryId);
        CategoryTree GetUserCategoriesTree(int userId);
        CategoryTree GetUserCategoriesTreeWithProduct(int userId, IProductDal productDal);
    }
}
