using System;
using System.Collections.Generic;
using MediatR;
using Microservices.Shared.Dtos;
using Services.Order.Application.Dtos;

namespace Services.Order.Application.Queries
{
  public class GetOrdersByUserIdQuery: IRequest<Response<List<OrderDto>>>
  {
    public string UserId { get; set; }
  }
}
