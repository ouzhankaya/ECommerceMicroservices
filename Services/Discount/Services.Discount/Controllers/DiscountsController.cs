using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.Shared.ControllerBases;
using Microservices.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Services.Discount.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Services.Discount.Controllers
{
  [Route("api/[controller]")]
  public class DiscountsController : CustomBaseController
  {
    private readonly IDiscountService _discountService;
    private readonly ISharedIdentiyService _sharedIdentityService;

    public DiscountsController(IDiscountService discountService, ISharedIdentiyService sharedIdentityService)
    {
      _discountService = discountService;
      _sharedIdentityService = sharedIdentityService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      return CreateActionResultInstance(await _discountService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
      return CreateActionResultInstance(await _discountService.GetById(id));
    }

    [HttpGet]
    [Route("/api/[controller]/[action]/{code}")]
    public async Task<IActionResult> GetByCode(string code)
    {
      var userId = _sharedIdentityService.GetUserId;
      return CreateActionResultInstance(await _discountService.GetByCodeAndUserId(code, userId));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Models.Discount discount)
    {
      var newDiscount =  await _discountService.Create(discount);
      return CreateActionResultInstance(newDiscount);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Models.Discount discount)
    {
      var updatedDiscount = await _discountService.Update(discount);
      return CreateActionResultInstance(updatedDiscount);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      return CreateActionResultInstance(await _discountService.Delete(id));
    }
  }
}
