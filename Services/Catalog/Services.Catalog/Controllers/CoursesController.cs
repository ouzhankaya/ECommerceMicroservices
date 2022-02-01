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
  public class CoursesController : CustomBaseController
  {
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
      _courseService = courseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var response = await _courseService.GetAllAsync();
      return CreateActionResultInstance(response);
    }

    [HttpGet("{id}")]

    public async Task<IActionResult> GetById(string id)
    {
      var response = await _courseService.GetByIdAsync(id);
      return CreateActionResultInstance(response);
    }

    [HttpGet]
    [Route("/api/[controller]/GetAllByUserId/{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
      var response = await _courseService.GetAllByUserIdAsync(userId);
      return CreateActionResultInstance(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody]CourseCreateDto courseCreateDto)
    {
      var response = await _courseService.CreateAsync(courseCreateDto);
      return CreateActionResultInstance(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody]CourseUpdateDto courseUpdateDto)
    {
      var response = await _courseService.UpdateAsync(courseUpdateDto);
      return CreateActionResultInstance(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
      var response = await _courseService.DeleteAsync(id);
      return CreateActionResultInstance(response);
    }
  }
}
