using ECommerce.Web.Models;
using IdentityModel.Client;
using Microservices.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Web.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<Response<bool>> SignIn(SigninInput signinInput);

        Task<TokenResponse> GetAccessTokenByRefreshToken();

        Task RevokeRefreshToken();
    }
}