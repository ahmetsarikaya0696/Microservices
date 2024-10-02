using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FreeCourse.Web.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public CoursesController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService)
        {
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _catalogService.GetAllCoursesByUserIdAsync(_sharedIdentityService.GetUserId);
            return View(courses);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _catalogService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateInput coursesCreateInput)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _catalogService.GetAllCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(coursesCreateInput);
            }

            coursesCreateInput.UserId = _sharedIdentityService.GetUserId;
            var result = await _catalogService.CreateCourseAsync(coursesCreateInput);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Veriler kaydedilirken bir hata oluştu!");
                var categories = await _catalogService.GetAllCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(coursesCreateInput);
            }

            return RedirectToAction(nameof(Index), "Courses");
        }

        public async Task<IActionResult> Update(string id)
        {
            var course = await _catalogService.GetCourseByCourseIdAsync(id);

            var categories = await _catalogService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", id);

            CourseUpdateInput courseUpdateInput = new CourseUpdateInput()
            {
                Id = course.Id,
                UserId = _sharedIdentityService.GetUserId,
                Name = course.Name,
                Price = course.Price,
                Picture = course.Picture,
                CategoryId = course.CategoryId,
                Description = course.Description,
                Feature = course.Feature
            };

            return View(courseUpdateInput);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CourseUpdateInput courseUpdateInput)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _catalogService.GetAllCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name", "courseUpdateInput.Id");
                return View(courseUpdateInput);
            }

            courseUpdateInput.UserId = _sharedIdentityService.GetUserId;

            var result = await _catalogService.UpdateCourseAsync(courseUpdateInput);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Güncelleme yapılırken bir hata meydana geldi!");
                var categories = await _catalogService.GetAllCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name", courseUpdateInput.Id);
                return View(courseUpdateInput);
            }

            return RedirectToAction(nameof(Index), "Courses");
        }

        public async Task<IActionResult> DeleteApproval(string id)
        {
            var course = await _catalogService.GetCourseByCourseIdAsync(id);
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await _catalogService.DeleteCourseAsync(id);
            return RedirectToAction(nameof(Index), "Courses");
        }
    }
}
