using Business.Concrete;
using Business.Constants;
using Castle.DynamicProxy;
using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;

namespace Business.BusinessAspects.Autofac
{
    public class SecuredOperation : MethodInterception
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager _userManager;

        public SecuredOperation()
        {
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
            _userManager = new(new EfUserDal());
        }

        protected override void OnBefore(IInvocation invocation)
        {
            try
            {
                Claim[] roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
                string userId = roleClaims[3].Value;
                string authTime = roleClaims[4].Value;
                User user = _userManager.GetById(Convert.ToInt32(string.IsNullOrEmpty(userId) ? 0 : userId));

                if (user == null || !user.Status || string.IsNullOrEmpty(authTime))
                {
                    throw new Exception(Messages.UnSigned);
                }

                if (user.PassChangeDate != null)
                {
                    if (Convert.ToDateTime(authTime) < user.PassChangeDate)
                    {
                        throw new Exception(Messages.UnSigned);
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception(Messages.UnSigned);
            }
        }
    }
}
