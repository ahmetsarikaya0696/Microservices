using FreeCourse.Shared.DTOs;
using FreeCourse.Web.Helpers;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly IPhotoStockService _photoStockService;
        private readonly PhotoHelper _photoHelper;

        public CatalogService(HttpClient httpClient, IPhotoStockService photoStockService, PhotoHelper photoHelper)
        {
            _httpClient = httpClient;
            _photoStockService = photoStockService;
            _photoHelper = photoHelper;
        }

        public async Task<List<CourseVM>> GetAllCoursesAsync()
        {
            var response = await _httpClient.GetAsync("courses");

            if (!response.IsSuccessStatusCode) return null;

            var successfullResponse = await response.Content.ReadFromJsonAsync<ResponseDTO<List<CourseVM>>>();

            successfullResponse.Data.ForEach(x =>
            {
                x.PictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);
            });

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

            successfullResponse.Data.ForEach(x =>
            {
                x.PictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);
            });

            return successfullResponse.Data;
        }

        public async Task<CourseVM> GetCourseByCourseIdAsync(string courseId)
        {
            var response = await _httpClient.GetAsync($"courses/{courseId}");

            if (!response.IsSuccessStatusCode) return null;

            var successfullResponse = await response.Content.ReadFromJsonAsync<ResponseDTO<CourseVM>>();

            successfullResponse.Data.PictureUrl = _photoHelper.GetPhotoStockUrl(successfullResponse.Data.Picture);

            return successfullResponse.Data;
        }

        public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
        {
            var resultPhotoService = await _photoStockService.UploadPhoto(courseCreateInput.PhotoFormFile);

            if (resultPhotoService != null)
            {
                courseCreateInput.Picture = resultPhotoService.Url;
            }

            var response = await _httpClient.PostAsJsonAsync("courses", courseCreateInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
        {
            var resultPhotoService = await _photoStockService.UploadPhoto(courseUpdateInput.PhotoFormFile);

            if (resultPhotoService != null)
            {
                await _photoStockService.DeletePhoto(courseUpdateInput.Picture);
                courseUpdateInput.Picture = resultPhotoService.Url;
            }

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
