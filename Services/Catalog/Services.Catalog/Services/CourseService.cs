using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microservices.Shared.Dtos;
using MongoDB.Driver;
using Services.Catalog.Dtos;
using Services.Catalog.Models;
using Services.Catalog.Settings;

namespace Services.Catalog.Services
{
  public class CourseService: ICourseService
  {
    private readonly IMongoCollection<Course> _courseCollection;
    private readonly IMongoCollection<Category> _categoryCollection;
    private readonly IMapper _mapper;

    public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
    {
      var client = new MongoClient(databaseSettings.ConnectionString);
      var database = client.GetDatabase(databaseSettings.DatabaseName);

      _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
      _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);

      _mapper = mapper;
    }

    public async Task<Response<List<CourseDto>>> GetAllAsync()
    {
      var courses = await _courseCollection.Find(course => true).ToListAsync();

      if (courses.Any())
      {
        foreach (var course in courses)
        {
          course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstOrDefaultAsync();
        }
      } else
      {
        courses = new List<Course>();
      }

      return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
    }

    public async Task<Response<CourseDto>> GetByIdAsync(string id)
    {
      var course = await _courseCollection.Find<Course>(x => x.Id == id).FirstOrDefaultAsync();
      if(course == null)
      {
        return Response<CourseDto>.Fail("Course Not Found", 404);
      }

      course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstOrDefaultAsync();
      return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
    }

    public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
    {
      var courses = await _courseCollection.Find<Course>(x => x.UserId == userId).ToListAsync();
      if(courses.Any())
      {
        foreach (var course in courses) 
        {
          course.Category = _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstOrDefault();
        }
      } else
      {
        courses = new List<Course>();
      }

      return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);

    }

    public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
    {
      var newCourse = _mapper.Map<Course>(courseCreateDto);
      newCourse.CreatedDate = DateTime.Now;
      await _courseCollection.InsertOneAsync(newCourse);

      return Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);
    }

    public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
    {
      var updatedCourse = _mapper.Map<Course>(courseUpdateDto);
      var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDto.Id, updatedCourse);

      if(result == null)
      {
        return Response<NoContent>.Fail("Course not found", 404);
      }

      return Response<NoContent>.Success(204);
    }

    public async Task<Response<NoContent>> DeleteAsync(string id)
    {
      var result = await _courseCollection.DeleteOneAsync(x=>x.Id == id);

      if(result.DeletedCount > 0)
      {
        return Response<NoContent>.Success(204);
      } else
      {
        return Response<NoContent>.Fail("Course not found", 404);
      }


    }
  }
}
