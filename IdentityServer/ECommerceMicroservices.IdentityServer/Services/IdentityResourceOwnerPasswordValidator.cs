using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceMicroservices.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;

namespace ECommerceMicroservices.IdentityServer.Services
{
  public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
  {

    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
    {
      _userManager = userManager;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
      var existsUser = await _userManager.FindByEmailAsync(context.UserName);

      if(existsUser == null)
      {
        var errors = new Dictionary<string, object>();
        errors.Add("errors", new List<string> { "Email ve ya parola hatalı" });
        context.Result.CustomResponse = errors;
        return;
      }

      var passwordCheck = await _userManager.CheckPasswordAsync(existsUser, context.Password);

      if (passwordCheck == false)
      {
        var errors = new Dictionary<string, object>();
        errors.Add("errors", new List<string> { "Email ve ya parola hatalı" });
        context.Result.CustomResponse = errors;
        return;
      }

      context.Result = new GrantValidationResult(existsUser.Id.ToString(), OidcConstants.AuthenticationMethods.Password);

    }
  }
}
