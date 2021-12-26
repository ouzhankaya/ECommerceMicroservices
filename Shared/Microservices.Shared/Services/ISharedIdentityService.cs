using System;
namespace Microservices.Shared.Services
{
  public interface ISharedIdentityService
  {
    public string GetUserId { get; }
  }
}
