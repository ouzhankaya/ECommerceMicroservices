using System;
using System.Collections.Generic;

namespace Services.Order.Application.Dtos
{
  public class OrderDto
  {
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string UserId { get; set; }
    public AddressDto Address { get; set; }

    public List<OrderItemDto> OrderItems { get; set; }
  }
}
