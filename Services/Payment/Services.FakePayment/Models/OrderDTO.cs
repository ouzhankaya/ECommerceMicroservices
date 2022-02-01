using System;
using System.Collections.Generic;

namespace Services.FakePayment.Models
{
  public class OrderDTO
  {
    public OrderDTO()
    {
      OrderItems = new List<OrderItemDto>();
    }

    public string UserId { get; set; }

    public List<OrderItemDto> OrderItems { get; set; }

    public AddressDto Address { get; set; }
  }

  public class OrderItemDto
  {
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string ImageUrl { get; set; }
    public Decimal Price { get; set; }
  }

  public class AddressDto
  {
    public string Province { get; set; }

    public string District { get; set; }

    public string Street { get; set; }

    public string ZipCode { get; set; }

    public string Line { get; set; }
  }
}
