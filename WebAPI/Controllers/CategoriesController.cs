﻿using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet("get")]
        public IActionResult Get([FromQuery] int categoryId)
        {
            IDataResult<Category> result = _categoryService.GetUserCategory(categoryId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] AddCategoryDto addCategory)
        {
            Category category = _mapper.Map<AddCategoryDto, Category>(addCategory);
            IResult result = _categoryService.Add(category);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] UpdateCategoryDto updateCategory)
        {
            Category category = _mapper.Map<UpdateCategoryDto, Category>(updateCategory);
            IResult result = _categoryService.Update(category);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("remove")]
        public IActionResult Remove([FromBody] RemoveCategoryDto removeCategory)
        {
            Category category = _mapper.Map<RemoveCategoryDto, Category>(removeCategory);
            IResult result = _categoryService.Remove(category);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("getall")]
        public IActionResult GetAll([FromQuery] int topCategoryId)
        {
            IDataResult<List<Category>> result = _categoryService.GetUserCategories(topCategoryId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
