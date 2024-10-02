using FreeCourse.Shared.DTOs;
using FreeCourse.Web.Models.Basket;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;
        private readonly IDiscountService _discountService;

        public BasketService(HttpClient httpClient, IDiscountService discountService)
        {
            _httpClient = httpClient;
            _discountService = discountService;
        }

        public async Task AddBasketItem(BasketItemVM basketItemVM)
        {
            var basket = await Get();

            if (basket != null)
            {
                if (!basket.BasketItems.Any(x => x.CourseId == basketItemVM.CourseId))
                {
                    basket.BasketItems.Add(basketItemVM);
                }
            }
            else
            {
                basket = new BasketVM();
                basket.BasketItems.Add(basketItemVM);
            }

            await SaveOrUpdate(basket);
        }

        public async Task<bool> ApplyDiscount(string discountCode)
        {
            await CancelApplyDiscount();

            var basket = await Get();
            if (basket == null) return false;

            var discount = await _discountService.GetDiscount(discountCode);
            if (discount == null) return false;

            basket.ApplyDiscount(discount.Code, discount.Rate);

            return await SaveOrUpdate(basket);
        }

        public async Task<bool> CancelApplyDiscount()
        {
            var basket = await Get();

            if (basket == null || basket.DiscountCode == null) return false;

            basket.CancelDiscount();

            return await SaveOrUpdate(basket);
        }

        public async Task<bool> Delete()
        {
            var result = await _httpClient.DeleteAsync("baskets");
            return result.IsSuccessStatusCode;
        }

        public async Task<BasketVM> Get()
        {
            var response = await _httpClient.GetAsync("baskets");

            if (!response.IsSuccessStatusCode) return null;

            var successfullResponse = await response.Content.ReadFromJsonAsync<ResponseDTO<BasketVM>>();
            return successfullResponse.Data;
        }

        public async Task<bool> RemoveBasketItem(string coursesId)
        {
            var basket = await Get();
            if (basket == null) return false;

            var deletedBasketItem = basket.BasketItems.FirstOrDefault(x => x.CourseId == coursesId);
            if (deletedBasketItem == null) return false;

            var deleteResult = basket.BasketItems.Remove(deletedBasketItem);

            if (!deleteResult) return false;

            if (!basket.BasketItems.Any()) basket.DiscountCode = null;

            return await SaveOrUpdate(basket);

        }

        public async Task<bool> SaveOrUpdate(BasketVM basketVM)
        {
            var response = await _httpClient.PostAsJsonAsync<BasketVM>("baskets", basketVM);
            return response.IsSuccessStatusCode;
        }
    }
}
