using AutoMapper;
using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Services.Catalog.Model;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.DTOs;
using FreeCourse.Shared.Messages;
using MassTransit;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings, IPublishEndpoint publishEndpoint)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);

            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<ResponseDTO<List<CourseDTO>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(x => true).ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstOrDefaultAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }

            return ResponseDTO<List<CourseDTO>>.Success(_mapper.Map<List<CourseDTO>>(courses), 200);
        }


        public async Task<ResponseDTO<CourseDTO>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find<Course>(x => x.Id == id).FirstOrDefaultAsync();

            if (course == null) return ResponseDTO<CourseDTO>.Fail($"Course not found (Id = {id})", 404);

            course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstOrDefaultAsync();

            return ResponseDTO<CourseDTO>.Success(_mapper.Map<CourseDTO>(course), 200);
        }

        public async Task<ResponseDTO<List<CourseDTO>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find(x => x.UserId == userId).ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstOrDefaultAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }

            return ResponseDTO<List<CourseDTO>>.Success(_mapper.Map<List<CourseDTO>>(courses), 200);
        }

        public async Task<ResponseDTO<CourseDTO>> CreateAsync(CourseCreateDTO courseCreateDTO)
        {
            var newCourse = _mapper.Map<Course>(courseCreateDTO);
            newCourse.CreatedTime = DateTime.Now;

            await _courseCollection.InsertOneAsync(newCourse);

            return ResponseDTO<CourseDTO>.Success(_mapper.Map<CourseDTO>(newCourse), 200);
        }

        public async Task<ResponseDTO<NoContentDTO>> UpdateAsync(CourseUpdateDTO courseUpdateDTO)
        {
            var updateCourse = _mapper.Map<Course>(courseUpdateDTO);

            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDTO.Id, updateCourse);

            if (result == null) return ResponseDTO<NoContentDTO>.Fail($"Course not found (Id = {courseUpdateDTO.Id})", 404);

            await _publishEndpoint.Publish(new CourseNameChangedEvent() { CourseId = updateCourse.Id, UpdatedName = courseUpdateDTO.Name });

            return ResponseDTO<NoContentDTO>.Success(204);
        }

        public async Task<ResponseDTO<NoContentDTO>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);

            if (result.DeletedCount > 0) return ResponseDTO<NoContentDTO>.Success(204);

            return ResponseDTO<NoContentDTO>.Fail($"Course not found (Id = {id})", 404);
        }
    }
}