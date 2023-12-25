using Core.DataAccess;
using Entities.Concrete;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    public interface ICategoryDal : IEntityRepository<Category>
    {
        List<Category> GetUserCategories(int userId, int topCategoryId);
        Category GetUserCategory(int userId, int categoryId);
    }
}
