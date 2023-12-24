using Core.Utilities.Results;
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
    }
}
