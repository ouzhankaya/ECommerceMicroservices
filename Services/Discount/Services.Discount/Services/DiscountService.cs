using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microservices.Shared.Dtos;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Services.Discount.Services
{
  public class DiscountService: IDiscountService
  {
    private readonly IConfiguration _configuration;
    private readonly IDbConnection _dbConnection;

    public DiscountService(IConfiguration configuration)
    {
      _configuration = configuration;
      _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSqlConnectionString"));
    }

    public async Task<Response<NoContent>> Create(Models.Discount discount)
    {
      var result = await _dbConnection.ExecuteAsync("insert into discount(userId,rate,code) values(@UserId,@Rate,@Code)", discount);

      if(result > 0)
      {
        return Response<NoContent>.Success(204);
      }
      return Response<NoContent>.Fail("an error accured while adding",500);
    }

    public async Task<Response<NoContent>> Delete(int id)
    {
      var result = await _dbConnection.ExecuteAsync("delete from discount where id=@Id", new { Id=id });

      return result > 0 ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("discount not found", 404);
    }

    public async Task<Response<List<Models.Discount>>> GetAll()
    {
      var discounts = await _dbConnection.QueryAsync<Models.Discount>("select * from discount");
      return Response<List<Models.Discount>>.Success(discounts.ToList(),200);
    }

    public async Task<Response<Models.Discount>> GetByCodeAndUserId(string code, string userId)
    {
      var discounts = await _dbConnection.QueryAsync<Models.Discount>("select * from discount where userid=@UserId and code=@Code", new
      {
        UserId = userId,
        Code = code
      });

      var discount = discounts.FirstOrDefault();

      if(discount == null)
      {
        return Response<Models.Discount>.Fail("discount not found", 404);
      }
      return Response<Models.Discount>.Success(discount, 200);
    }

    public async Task<Response<Models.Discount>> GetById(int id)
    {
      var discount = (await _dbConnection.QueryAsync<Models.Discount>("select * from discount where id=@Id", new { Id= id})).SingleOrDefault();

      if(discount == null)
      {
        return Response<Models.Discount>.Fail("discount not found", 404);
      }
      return Response<Models.Discount>.Success(discount, 200);
    }

    public async Task<Response<NoContent>> Update(Models.Discount discount)
    {
      var result = await _dbConnection.ExecuteAsync("update discount set userid=@UserId, rate=@Rate, code=@Code where id=@Id", new {
        Id = discount.Id,
        UserId = discount.UserId,
        Code = discount.Code,
        Rate = discount.Rate
      });

      if(result > 0)
      {
        return Response<NoContent>.Success(204);
      }
      return Response<NoContent>.Fail("discount not found", 404);
    }
  }
}
