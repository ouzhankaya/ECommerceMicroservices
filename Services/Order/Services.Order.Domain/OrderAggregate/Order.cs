using System;
using System.Collections.Generic;
using System.Linq;
using Services.Order.Domain.Core;

namespace Services.Order.Domain.OrderAggregate
{
  public class Order: Entity, IAggregateRoot
  {
    public DateTime CreatedDate { get; private set; }
    public string UserId { get; private set; }
    public Address Address { get; private set; }


    private readonly List<OrderItem> _orderItems;

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    public Order()
    {

    }

    public Order(string userId, Address address)
    {
      _orderItems = new List<OrderItem>();
      CreatedDate = DateTime.Now;
      UserId = userId;
      Address = address;
    }

    public void AddOrderItem(string productId, string productName, decimal price,string imageUrl)
    {
      var existProduct = _orderItems.Any(x=>x.ProductId == productId);

      if(!existProduct)
      {
        var newOrderItem = new OrderItem(productId, productName, imageUrl, price);
        _orderItems.Add(newOrderItem);
      }
    }

    public decimal TotalPrice => _orderItems.Sum(x=>x.Price);
  }
}
