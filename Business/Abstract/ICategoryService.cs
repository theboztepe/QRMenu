using Core.Utilities.Results;
using Entities.Concrete;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        IDataResult<List<Category>> UserCategories(int topCategoryId);

        #region CRUD
        IResult Add(Category category);
        IResult Update(Category category);
        IResult Remove(Category category);
        #endregion
    }
}
