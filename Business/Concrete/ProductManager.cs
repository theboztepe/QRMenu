using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
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
    public class ProductManager : IProductService
    {
        private readonly IProductDal _productDal;
        private readonly ICategoryDal _categoryDal;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductManager(IProductDal productDal, ICategoryDal categoryDal)
        {
            _productDal = productDal;
            _categoryDal = categoryDal;

            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        #region CRUD
        [CacheAspect]
        public IDataResult<List<Product>> GetCategoryProducts(int categoryId)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetCategoryProducts(Convert.ToInt32(_httpContextAccessor.HttpContext.User.ClaimRoles()[3].Value), categoryId));
        }

        [ValidationAspect(typeof(ProductValidator))]
        [TransactionScopeAspect]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(Product product)
        {
            Category category = new()
            {
                Id = product.CategoryId,
                UserId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.ClaimRoles()[3].Value)
            };
            IResult result = BusinessRules.Run(
                    CheckIfCategory(category),
                    CheckIfProductNameExistsAdd(product)
                );
            if (result != null)
            {
                return result;
            }

            _productDal.Add(product);

            return new SuccessResult(Messages.ProductAdded);
        }

        [ValidationAspect(typeof(ProductValidator))]
        [TransactionScopeAspect]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product)
        {
            Category category = new()
            {
                Id = product.CategoryId,
                UserId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.ClaimRoles()[3].Value)
            };
            IResult result = BusinessRules.Run(
                    CheckIfCategory(category),
                    CheckIfProductFound(product),
                    CheckIfProductNameExistsUpdate(product)
                );
            if (result != null)
            {
                return result;
            }

            _productDal.Update(product);

            return new SuccessResult(Messages.ProductUpdated);
        }

        [TransactionScopeAspect]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Remove(Product product)
        {
            Category category = new()
            {
                Id = product.CategoryId,
                UserId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.ClaimRoles()[3].Value)
            };
            IResult result = BusinessRules.Run(
                    CheckIfCategory(category),
                    CheckIfProductFound(product)
                );
            if (result != null)
            {
                return result;
            }

            _productDal.Delete(product);

            return new SuccessResult(Messages.ProductRemoved);
        }

        #region RULES
        private IResult CheckIfCategory(Category category)
        {
            Category result = _categoryDal.Get(c => c.Id == category.Id && c.UserId == category.UserId);
            return result == null ? new ErrorResult(Messages.CategoryNotFound) : (result.TopCategoryId == 0 ? new ErrorResult(Messages.ProductNotCreateBaseCategory) : new SuccessResult());
        }
        private IResult CheckIfProductNameExistsAdd(Product product)
        {
            bool result = _productDal.GetAll(p => p.Name == product.Name && p.CategoryId == product.CategoryId).Any();
            return result ? new ErrorResult(Messages.ProductNameAlreadyExists) : new SuccessResult();
        }

        private IResult CheckIfProductFound(Product product)
        {
            Product result = _productDal.Get(p => p.Id == product.Id && p.CategoryId == product.CategoryId);
            return result == null ? new ErrorResult(Messages.ProductNotFound) : new SuccessResult();
        }

        private IResult CheckIfProductNameExistsUpdate(Product product)
        {
            bool result = _productDal.GetAll(p => p.Name == product.Name && p.CategoryId == product.CategoryId && p.Id != product.Id).Any();
            return result ? new ErrorResult(Messages.ProductNameAlreadyExists) : new SuccessResult();
        }
        #endregion
        #endregion
    }
}
