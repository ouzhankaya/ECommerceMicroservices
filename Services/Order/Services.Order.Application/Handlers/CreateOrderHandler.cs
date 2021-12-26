using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microservices.Shared.Dtos;
using Services.Order.Application.Commands;
using Services.Order.Application.Dtos;
using Services.Order.Domain.OrderAggregate;
using Services.Order.Infrastracture;

namespace Services.Order.Application.Handlers
{
  public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDto>>
  {
    private readonly OrderDbContext _context;

    public CreateOrderCommandHandler(OrderDbContext context)
    {
      _context = context;
    }

    public async Task<Response<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
      var newAddress = new Address(request.Address.Province, request.Address.District, request.Address.Street, request.Address.ZipCode, request.Address.Line);

      var newOrder = new Domain.OrderAggregate.Order(request.UserId, newAddress);

      request.OrderItems.ForEach(x =>
      {
        newOrder.AddOrderItem(x.ProductId, x.ProductName, x.Price, x.ImageUrl);
      });

     await _context.Orders.AddAsync(newOrder);

     await _context.SaveChangesAsync();

      return Response<CreatedOrderDto>.Success(new CreatedOrderDto { OrderId = newOrder.Id}, 200);
    }
  }
}
