using FreeCourse.Services.Discount.Services;
using FreeCourse.Shared.BaseController;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Discount.Controllers
{
    public class DiscountsController : BaseController
    {
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IDiscountService _discountService;

        public DiscountsController(IDiscountService discountService, ISharedIdentityService sharedIdentityService)
        {
            _discountService = discountService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _discountService.GetAll();
            return CreateActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _discountService.GetById(id);
            return CreateActionResult(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var userId = _sharedIdentityService.GetUserId;
            var discount = await _discountService.GetByCodeAndUserId(userId, code);
            return CreateActionResult(discount);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Models.Discount discount)
        {
            var result = await _discountService.Save(discount);
            return CreateActionResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Models.Discount discount)
        {
            var result = await _discountService.Update(discount);
            return CreateActionResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var result = await _discountService.Delete(id);
            return CreateActionResult(result);
        }
    }
}
