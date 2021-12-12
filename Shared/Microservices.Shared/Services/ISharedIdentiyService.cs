using System;
namespace Microservices.Shared.Services
{
  public interface ISharedIdentiyService
  {
    public string GetUserId { get; }
  }
}
