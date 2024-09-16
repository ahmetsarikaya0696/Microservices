using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Shared.DTOs;

namespace FreeCourse.Services.Catalog.Services
{
    public interface ICategoryService
    {
        Task<ResponseDTO<List<CategoryDTO>>> GetAllAsync();
        Task<ResponseDTO<CategoryDTO>> CreateAsync(CategoryCreateDTO categoryCreateDTO);
        Task<ResponseDTO<CategoryDTO>> GetByIdAsync(string id);
    }
}
