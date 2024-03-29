﻿using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet("get")]
        public IActionResult Get([FromQuery] int productId)
        {
            IDataResult<Product> result = _productService.GetUserProduct(productId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] AddProductDto addProduct)
        {
            Product product = _mapper.Map<AddProductDto, Product>(addProduct);
            IResult result = _productService.Add(product);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] UpdateProductDto updateProduct)
        {
            Product product = _mapper.Map<UpdateProductDto, Product>(updateProduct);
            IResult result = _productService.Update(product);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("remove")]
        public IActionResult Remove([FromBody] RemoveProductDto removeProduct)
        {
            Product product = _mapper.Map<RemoveProductDto, Product>(removeProduct);
            IResult result = _productService.Remove(product);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("getcategoryproducts")]
        public IActionResult GetCategoryProducts([FromQuery] int categoryId)
        {
            IDataResult<List<Product>> result = _productService.GetCategoryProducts(categoryId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
