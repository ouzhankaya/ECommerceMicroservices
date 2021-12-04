using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservices.Shared.Dtos;
using Services.Catalog.Dtos;

namespace Services.Catalog.Services
{
  public interface ICategoryService
  {
    Task<Response<List<CategoryDto>>> GetAllAsync();
    Task<Response<CategoryDto>> CreateAsync(CategoryDto categorydto);
    Task<Response<CategoryDto>> GetByIdAsync(string id);
  }
}
