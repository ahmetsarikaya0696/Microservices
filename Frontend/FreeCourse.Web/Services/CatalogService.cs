using FreeCourse.Shared.DTOs;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CourseVM>> GetAllCoursesAsync()
        {
            var response = await _httpClient.GetAsync("courses");

            if (!response.IsSuccessStatusCode) return null;

            var successfullResponse = await response.Content.ReadFromJsonAsync<ResponseDTO<List<CourseVM>>>();

            return successfullResponse.Data;
        }

        public async Task<List<CategoryVM>> GetAllCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("categories");

            if (!response.IsSuccessStatusCode) return null;

            var successfullResponse = await response.Content.ReadFromJsonAsync<ResponseDTO<List<CategoryVM>>>();

            return successfullResponse.Data;
        }

        public async Task<List<CourseVM>> GetAllCoursesByUserIdAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"courses/GetAllByUserId/{userId}");

            if (!response.IsSuccessStatusCode) return null;

            var successfullResponse = await response.Content.ReadFromJsonAsync<ResponseDTO<List<CourseVM>>>();

            return successfullResponse.Data;
        }

        public async Task<CourseVM> GetCourseByCourseIdAsync(string courseId)
        {
            var response = await _httpClient.GetAsync($"courses/{courseId}");

            if (!response.IsSuccessStatusCode) return null;

            var successfullResponse = await response.Content.ReadFromJsonAsync<ResponseDTO<CourseVM>>();

            return successfullResponse.Data;
        }

        public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
        {
            var response = await _httpClient.PostAsJsonAsync("courses", courseCreateInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
        {
            var response = await _httpClient.PutAsJsonAsync("courses", courseUpdateInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCourseAsync(string courseId)
        {
            var response = await _httpClient.DeleteAsync($"courses/{courseId}");

            return response.IsSuccessStatusCode;
        }
    }
}
