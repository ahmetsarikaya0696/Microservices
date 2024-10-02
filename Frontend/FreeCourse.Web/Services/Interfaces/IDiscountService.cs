using FreeCourse.Web.Models.Discount;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<DiscountVM> GetDiscount(string discountCode);
    }
}
