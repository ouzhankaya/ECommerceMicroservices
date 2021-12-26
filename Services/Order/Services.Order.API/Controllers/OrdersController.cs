using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microservices.Shared.ControllerBases;
using Microservices.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Services.Order.Application.Commands;
using Services.Order.Application.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Services.Order.API.Controllers
{
  [Route("api/[controller]")]
  public class OrdersController : CustomBaseController
  {
    private IMediator _mediator;
    private ISharedIdentityService _sharedIdentiyService;
    public OrdersController(IMediator mediator, ISharedIdentityService sharedIdentiyService)
    {
      _mediator = mediator;
      _sharedIdentiyService = sharedIdentiyService;
    }

    [HttpGet]

    public async Task<IActionResult> GetOrders()
    {
      var response = await _mediator.Send(new GetOrdersByUserIdQuery { UserId = _sharedIdentiyService.GetUserId });

      return CreateActionResultInstance(response);
    }

    [HttpPost]

    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand createOrderCommand)
    {
      var response = await _mediator.Send(createOrderCommand);

      return CreateActionResultInstance(response);

    }
  }
}
