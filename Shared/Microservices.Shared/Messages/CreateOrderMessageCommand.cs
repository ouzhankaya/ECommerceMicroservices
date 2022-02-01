using System;
using System.Collections.Generic;

namespace Microservices.Shared.Messages
{
  public class CreateOrderMessageCommand
  {
    public CreateOrderMessageCommand()
    {
      OrderItems = new List<OrderItem>();
    }
    public string UserId { get; set; }

    public List<OrderItem> OrderItems { get; set; }

    public string Province { get; set; }
    public string District { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public string Line { get; set; }

  }
  public class OrderItem
  {
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
  }
}
