using System;
namespace ECommerceMicroservices.IdentityServer.Dtos
{
  public class SignupDto
  {
    public string Username { get; set; }
    public string Email { get; set; }
    public string City { get; set; }
    public string Password { get; set; }
  }
}
