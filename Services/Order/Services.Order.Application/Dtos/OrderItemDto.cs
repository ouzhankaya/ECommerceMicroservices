using System;
namespace Services.Order.Application.Dtos
{
  public class OrderItemDto
  {
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
  }
}
