using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Shared.DTOs;

namespace FreeCourse.Services.Catalog.Services
{
    public interface ICourseService
    {
        Task<ResponseDTO<List<CourseDTO>>> GetAllAsync();
        Task<ResponseDTO<CourseDTO>> GetByIdAsync(string id);
        Task<ResponseDTO<List<CourseDTO>>> GetAllByUserIdAsync(string userId);
        Task<ResponseDTO<CourseDTO>> CreateAsync(CourseCreateDTO courseCreateDTO);
        Task<ResponseDTO<NoContentDTO>> UpdateAsync(CourseUpdateDTO courseUpdateDTO);
        Task<ResponseDTO<NoContentDTO>> DeleteAsync(string id);
    }
}
