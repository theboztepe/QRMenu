using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.Extensions;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Aspects.Autofac.Caching
{
    public class CacheAspect(int duration = 60) : MethodInterception
    {
        private readonly int _duration = duration;
        private readonly ICacheManager _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        private readonly IHttpContextAccessor _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();

        public override void Intercept(IInvocation invocation)
        {
            int userId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.ClaimRoles()[3].Value);
            string methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{userId}.{invocation.Method.Name}");
            List<object> arguments = [.. invocation.Arguments];
            string key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})";
            if (_cacheManager.IsAdd(key))
            {
                invocation.ReturnValue = _cacheManager.Get(key);
                return;
            }
            invocation.Proceed();
            _cacheManager.Add(key, invocation.ReturnValue, _duration);
        }
    }
}