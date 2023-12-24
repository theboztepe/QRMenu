using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Helper;
using Entities.DTOs.QR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Http;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using Core.Extensions;
using System.IdentityModel.Tokens.Jwt;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRController : ControllerBase
    {
        private readonly IQRService _qrService;
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QRController(IQRService qrService, IAuthService authService)
        {
            _qrService = qrService;
            _authService = authService;

            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        [Authorize]
        [HttpGet("qrmenucode")]
        public IActionResult QRMenuCode()
        {
            IDataResult<string> result = _authService.GetQRMenuCode();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize]
        [HttpGet("qrmenucategories")]
        public IActionResult QRMenuCategories()
        {
            IDataResult<CategoryTree> result = _qrService.GetQRMenuCategories();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize]
        [HttpGet("qrmenucategorieswithproduct")]
        public IActionResult GetQRMenuCategoriesWithProduct()
        {
            IDataResult<CategoryTree> result = _qrService.GetQRMenuCategoriesWithProduct();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("qrmenu")]
        public IActionResult QRMenu([FromQuery] string qrCode)
        {
            QRCodeDto qrCodeDto = new()
            {
                QRCode = qrCode
            };

            IDataResult<int?> userInfo = _authService.GetQRCodeForUserId(qrCodeDto);
            if (!userInfo.Success)
                return BadRequest(userInfo);

            #region create guest claims
            if (_httpContextAccessor?.HttpContext?.User?.ClaimRoles().Length == 0)
            {
                var claims = new List<Claim>
                            {
                               new(JwtRegisteredClaimNames.Sub, userInfo.Data.ToString()),
                               new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                               new(JwtRegisteredClaimNames.UniqueName, "guest_" + Guid.NewGuid().ToString()),
                               new(JwtRegisteredClaimNames.Sub, userInfo.Data.ToString()),
                               new(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                            };

                var claimsIdentity = new ClaimsIdentity(claims);
                _httpContextAccessor.HttpContext.User.AddIdentity(claimsIdentity);
            }
            #endregion

            IDataResult<CategoryTree> result = _qrService.GetQRMenuWithQRCode((int)userInfo.Data);
            return result.Success ? Ok(result) : BadRequest(result);
        }

    }
}
