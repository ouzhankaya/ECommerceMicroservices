using System;
using Microsoft.AspNetCore.Http;

namespace Microservices.Shared.Services
{
  public class SharedIdentityService : ISharedIdentiyService
  {
    private IHttpContextAccessor _httpContextAccessor;

    public SharedIdentityService(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }
    public string GetUserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;
  }
}
