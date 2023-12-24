using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Core.Aspects.Autofac.Caching;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Business.Concrete
{
    [SecuredOperation]
    public class QRManager : IQRService
    {
        private readonly IQRDal _qrDal;
        private readonly IProductDal _productDal;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QRManager(IQRDal qrDal, IProductDal productDal)
        {
            _qrDal = qrDal;
            _productDal = productDal;

            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        [CacheAspect]
        public IDataResult<CategoryTree> GetQRMenuCategories()
        {
            return new SuccessDataResult<CategoryTree>(_qrDal.GetQRMenuCategories(Convert.ToInt32(_httpContextAccessor.HttpContext.User.ClaimRoles()[3].Value)));
        }

        [CacheAspect]
        public IDataResult<CategoryTree> GetQRMenuCategoriesWithProduct()
        {
            return new SuccessDataResult<CategoryTree>(_qrDal.GetQRMenuCategoriesWithProduct(Convert.ToInt32(_httpContextAccessor.HttpContext.User.ClaimRoles()[3].Value), _productDal));
        }

        [CacheAspect]
        public IDataResult<CategoryTree> GetQRMenuWithQRCode(int userId)
        {
            return new SuccessDataResult<CategoryTree>(_qrDal.GetQRMenuWithQRCode(userId, _productDal));
        }
    }
}
