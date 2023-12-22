using Core.Utilities.Results;
using DataAccess.Helper;
using Entities.Concrete;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        #region CRUD
        IDataResult<List<Category>> GetUserCategories(int topCategoryId);
        IResult Add(Category category);
        IResult Update(Category category);
        IResult Remove(Category category);
        #endregion
        IDataResult<CategoryTree> GetUserCategoriesTree();
        IDataResult<CategoryTree> GetUserCategoriesTreeWithProduct();

    }
}
