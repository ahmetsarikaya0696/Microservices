using FreeCourse.Shared.DTOs;
using FreeCourse.Web.Models.Catalog.PhotoStock;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
    public class PhotoStockService : IPhotoStockService
    {
        private readonly HttpClient _httpClient;

        public PhotoStockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DeletePhoto(string photoUrl)
        {
            var response = await _httpClient.DeleteAsync($"photos?photoUrl={photoUrl}");

            return response.IsSuccessStatusCode;
        }

        public async Task<PhotoStockVM> UploadPhoto(IFormFile file)
        {
            if (file == null || file.Length < 0) return null;

            var randomFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var multipartFormDataContent = new MultipartFormDataContent
            {
                { new ByteArrayContent(memoryStream.ToArray()), "photo", randomFileName }
            };

            var response = await _httpClient.PostAsync("photos", multipartFormDataContent);

            if (!response.IsSuccessStatusCode) return null;
            var successfullResponse = await response.Content.ReadFromJsonAsync<ResponseDTO<PhotoStockVM>>();
            return successfullResponse.Data;
        }
    }
}
