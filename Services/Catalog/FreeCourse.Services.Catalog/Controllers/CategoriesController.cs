using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Services.Catalog.Services;
using FreeCourse.Shared.BaseController;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Catalog.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryService.GetAllAsync();
            return CreateActionResult(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _categoryService.GetByIdAsync(id);
            return CreateActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDTO categoryCreateDTO)
        {
            var response = await _categoryService.CreateAsync(categoryCreateDTO);
            return CreateActionResult(response);
        }

    }
}
