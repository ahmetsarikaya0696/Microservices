using FreeCourse.Services.Basket.DTOs;
using FreeCourse.Services.Basket.Services;
using FreeCourse.Shared.BaseController;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Basket.Controllers
{
    public class BasketsController : BaseController
    {
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public BasketsController(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            return CreateActionResult(await _basketService.GetBasketAsync(_sharedIdentityService.GetUserId));
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateBasket(BasketDTO basketDTO)
        {
            basketDTO.UserId = _sharedIdentityService.GetUserId;
            var response = await _basketService.SaveOrUpdateAsync(basketDTO);
            return CreateActionResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            return CreateActionResult(await _basketService.DeleteAsync(_sharedIdentityService.GetUserId));
        }
    }
}
