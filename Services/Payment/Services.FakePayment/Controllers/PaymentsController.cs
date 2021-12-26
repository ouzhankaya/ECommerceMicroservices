using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.Shared.ControllerBases;
using Microservices.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Services.FakePayment.Controllers
{
  [Route("api/[controller]")]
  public class PaymentsController : CustomBaseController
  {
   [HttpPost]
   public IActionResult ReceivePayment()
    {
      return CreateActionResultInstance(Response<NoContent>.Success(200));
    }
    
  }
}
