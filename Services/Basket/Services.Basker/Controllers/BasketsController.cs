using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.Shared.ControllerBases;
using Microservices.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Services.Basket.Dtos;
using Services.Basket.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Services.Basket.Controllers
{
  [Route("api/[controller]")]
  public class BasketsController : CustomBaseController
  {
    private readonly IBasketService _basketService;
    private readonly ISharedIdentityService _sharedIdentiyService;

    public BasketsController(IBasketService basketService, ISharedIdentityService sharedIdentiyService)
    {
      _basketService = basketService;
      _sharedIdentiyService = sharedIdentiyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBaskets()
    {
      return CreateActionResultInstance(await _basketService.GetBasket(_sharedIdentiyService.GetUserId));
    }

    [HttpPost]
    public async Task<IActionResult> Upsert([FromBody]BasketDto basketDto)
    {
      var result = await _basketService.Upsert(basketDto);
      return CreateActionResultInstance(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
      return CreateActionResultInstance(await _basketService.Delete(_sharedIdentiyService.GetUserId));
    }
  }
}
