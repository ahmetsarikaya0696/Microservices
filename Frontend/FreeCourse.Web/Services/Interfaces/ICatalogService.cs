using FreeCourse.Web.Models.Catalog;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<List<CourseVM>> GetAllCoursesAsync();
        Task<List<CourseVM>> GetAllCoursesByUserIdAsync(string userId);
        Task<bool> DeleteCourseAsync(string courseId);
        Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput);
        Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput);
        Task<CourseVM> GetCourseByCourseIdAsync(string courseId);
        Task<List<CategoryVM>> GetAllCategoriesAsync();
    }
}
