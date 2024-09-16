using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Services.Catalog.Services;
using FreeCourse.Shared.BaseController;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Catalog.Controllers
{
    public class CoursesController : BaseController
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _courseService.GetByIdAsync(id);
            return CreateActionResult(response);
        }


        [HttpGet]
        [Route("/api/{controller}/GetAllByUserId/{userId}")]
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            var response = await _courseService.GetAllByUserIdAsync(userId);
            return CreateActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateDTO courseCreateDTO)
        {
            var response = await _courseService.CreateAsync(courseCreateDTO);
            return CreateActionResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CourseUpdateDTO courseUpdateDTO)
        {
            var response = await _courseService.UpdateAsync(courseUpdateDTO);
            return CreateActionResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _courseService.DeleteAsync(id);
            return CreateActionResult(response);
        }

    }
}
