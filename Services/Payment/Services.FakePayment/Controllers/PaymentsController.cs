using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microservices.Shared.ControllerBases;
using Microservices.Shared.Dtos;
using Microservices.Shared.Messages;
using Microsoft.AspNetCore.Mvc;
using Services.FakePayment.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Services.FakePayment.Controllers
{
  [Route("api/[controller]")]
  public class PaymentsController : CustomBaseController
  {

    private readonly ISendEndpointProvider _sendEndpointProvider;

    public PaymentsController(ISendEndpointProvider sendEndpointProvider)
    {
      _sendEndpointProvider = sendEndpointProvider;
    }


   [HttpPost]
   public async Task<IActionResult> ReceivePayment(PaymentDTO paymentDto)
    {
      var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));

      var createOrderMessageCommand = new CreateOrderMessageCommand();

      createOrderMessageCommand.UserId = paymentDto.Order.UserId;
      createOrderMessageCommand.Province = paymentDto.Order.Address.Province;
      createOrderMessageCommand.District = paymentDto.Order.Address.District;
      createOrderMessageCommand.Street = paymentDto.Order.Address.Street;
      createOrderMessageCommand.Line = paymentDto.Order.Address.Line;
      createOrderMessageCommand.ZipCode = paymentDto.Order.Address.ZipCode;

      paymentDto.Order.OrderItems.ForEach(x =>
      {
        createOrderMessageCommand.OrderItems.Add(new OrderItem
        {
          ImageUrl = x.ImageUrl,
          Price = x.Price,
          ProductId = x.ProductId,
          ProductName = x.ProductName
        });
      });

      await sendEndpoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);

      return CreateActionResultInstance(Microservices.Shared.Dtos.Response<NoContent>.Success(200));
    }
    
  }
}
