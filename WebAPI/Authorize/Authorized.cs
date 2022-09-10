using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace API.Authorize
{
    public class Authorized : ActionFilterAttribute
    {
        private readonly IAuthService _authService;
        public Authorized(IAuthService authService)
        {
            _authService = authService;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (filterContext.Controller is ControllerBase controller)
                {
                    base.OnActionExecuting(filterContext);

                    string userId = controller.User.Claims.ToArray()[3].Value;
                    string authTime = controller.User.Claims.ToArray()[4].Value;

                    IDataResult<User> userControl = _authService.FindUser(Convert.ToInt32(userId == null ? 0 : userId));

                    if (!userControl.Success || userControl.Data == null || !userControl.Data.Status || string.IsNullOrEmpty(authTime))
                    {
                        filterContext.Result = new UnauthorizedResult();
                        return;
                    }

                    User user = userControl.Data;
                    if (user.PassChangeDate != null)
                    {
                        if (Convert.ToDateTime(authTime) < user.PassChangeDate)
                        {
                            filterContext.Result = new UnauthorizedResult();
                            return;
                        }
                    }
                }
            }
            catch (Exception)
            {
                filterContext.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
