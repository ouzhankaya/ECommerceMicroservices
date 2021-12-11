using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microservices.Shared.ControllerBases;
using Microservices.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.PhotoStock.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Services.PhotoStock.Controllers
{
  [Route("api/[controller]")]
  public class PhotosController : CustomBaseController
  {
    [HttpPost]
   public async Task<IActionResult> SavePhoto(IFormFile photo, CancellationToken cancellationToken)
    {
        if(photo != null && photo.Length >0)
      {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);

        using var stream = new FileStream(path, FileMode.Create);
        await photo.CopyToAsync(stream, cancellationToken);

        var returnPath = "photos/" + photo.FileName;

        PhotoDto photoDto = new() { PhotoUrl = returnPath };

        return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto,200));
      }
      return CreateActionResultInstance(Response<PhotoDto>.Fail("photo is empty", 400));
    }
    [HttpDelete]
    public IActionResult DeletePhoto(string photoUrl)
    {
      var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);

      if(!System.IO.File.Exists(path))
      {
        return CreateActionResultInstance(Response<NoContent>.Fail("photo not found", 404));
      }
      System.IO.File.Delete(path);
      return CreateActionResultInstance(Response<NoContent>.Success(204));
    }
  }
}
