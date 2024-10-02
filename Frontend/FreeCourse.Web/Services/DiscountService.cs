using FreeCourse.Shared.DTOs;
using FreeCourse.Web.Models.Discount;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;

        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DiscountVM> GetDiscount(string discountCode)
        {
            var response = await _httpClient.GetAsync($"discounts/GetByCode/{discountCode}");

            if (!response.IsSuccessStatusCode) return null;

            var successfullResponse = await response.Content.ReadFromJsonAsync<ResponseDTO<DiscountVM>>();

            return successfullResponse.Data;
        }
    }
}
