using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;
using Services.Catalog.Dtos;
using Services.Catalog.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Services.Catalog.Controllers
{
  [Route("api/[controller]")]
  public class CategoriesController : CustomBaseController
  {
    private ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
      _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var response = await _categoryService.GetAllAsync();
      return CreateActionResultInstance(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
      var response = await _categoryService.GetByIdAsync(id);
      return CreateActionResultInstance(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryDto categoryDto)
    {
      var response = await _categoryService.CreateAsync(categoryDto);
      return CreateActionResultInstance(response);
    }
  }
}
