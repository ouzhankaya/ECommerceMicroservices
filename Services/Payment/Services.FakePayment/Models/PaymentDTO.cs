using System;
namespace Services.FakePayment.Models
{
  public class PaymentDTO
  {
    public string CardName { get; set; }
    public string CardNumber { get; set; }
    public string Expiration { get; set; }
    public string CVV { get; set; }
    public decimal TotalPrice { get; set; }

    public OrderDTO Order { get; set; }
  }
}
