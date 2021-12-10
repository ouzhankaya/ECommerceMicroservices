using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using ECommerceMicroservices.IdentityServer.Dtos;
using ECommerceMicroservices.IdentityServer.Models;
using Microservices.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static IdentityServer4.IdentityServerConstants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECommerceMicroservices.IdentityServer.Controllers
{
  [Authorize(LocalApi.PolicyName)]
  [Route("api/[controller]/[action]")]
  public class UserController : Controller
  {
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(UserManager<ApplicationUser> userManager)
    {
      _userManager = userManager;
    }
    [HttpPost]
    public async Task<IActionResult> Signup([FromBody]SignupDto signupDto)
    {
      var user = new ApplicationUser()
      {
        UserName = signupDto.Username,
        Email = signupDto.Email,
        City = signupDto.City
      };

      var result = await _userManager.CreateAsync(user, signupDto.Password);

      if(!result.Succeeded)
      {
        return BadRequest(Response<NoContent>.Fail(result.Errors.Select(x => x.Description).ToList(), 400));
      }

      return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetUser()
    {
      var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);

      if (userIdClaim == null) return BadRequest();

      var user = await _userManager.FindByIdAsync(userIdClaim.Value);

      if (user == null) return BadRequest();

      return Ok(new { Id= user.Id, UserName= user.UserName, Email= user.Email, City= user.City });
    }
  }
}
