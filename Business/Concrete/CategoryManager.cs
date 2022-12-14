using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Extensions;
using Core.Utilities.Business;
using Core.Utilities.IoC;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    [SecuredOperation]
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDal _categoryDal;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;

            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        public IDataResult<List<Category>> UserCategories(int topCategoryId)
        {
            return new SuccessDataResult<List<Category>>(_categoryDal.UserCategories(Convert.ToInt32(_httpContextAccessor.HttpContext.User.ClaimRoles()[3].Value), topCategoryId));
        }

        #region CRUD
        [ValidationAspect(typeof(CategoryValidator))]
        [TransactionScopeAspect]
        public IResult Add(Category category)
        {
            category.UserId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.ClaimRoles()[3].Value);
            IResult result = BusinessRules.Run(CheckIfCategoryNameExistsAdd(category));
            if (result != null)
            {
                return result;
            }

            _categoryDal.Add(category);

            return new SuccessResult(Messages.CategoryAdded);
        }

        [ValidationAspect(typeof(CategoryValidator))]
        [TransactionScopeAspect]
        public IResult Update(Category category)
        {
            category.UserId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.ClaimRoles()[3].Value);
            IResult result = BusinessRules.Run(CheckIfCategoryFound(category), CheckIfCategoryNameExistsUpdate(category));
            if (result != null)
            {
                return result;
            }

            _categoryDal.Update(category);

            return new SuccessResult(Messages.CategoryUpdated);
        }

        [TransactionScopeAspect]
        public IResult Remove(Category category)
        {
            category.UserId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.ClaimRoles()[3].Value);
            IResult result = BusinessRules.Run(CheckIfCategoryFound(category));
            if (result != null)
            {
                return result;
            }

            _categoryDal.Delete(category);

            return new SuccessResult(Messages.CategoryRemoved);
        }

        #region RULES
        private IResult CheckIfCategoryNameExistsAdd(Category category)
        {
            bool result = _categoryDal.GetAll(p => p.Name == category.Name && p.UserId == category.UserId).Any();
            return result ? new ErrorResult(Messages.CategoryNameAlreadyExists) : new SuccessResult();
        }

        private IResult CheckIfCategoryFound(Category category)
        {
            Category result = _categoryDal.Get(p => p.Id == category.Id && p.UserId == category.UserId);
            return result == null ? new ErrorResult(Messages.CategoryNotFound) : new SuccessResult();
        }

        private IResult CheckIfCategoryNameExistsUpdate(Category category)
        {
            bool result = _categoryDal.GetAll(p => p.Name == category.Name && p.UserId == category.UserId && p.Id != category.Id).Any();
            return result ? new ErrorResult(Messages.CategoryNameAlreadyExists) : new SuccessResult();
        }
        #endregion
        #endregion
    }
}
