using FreeCourse.Web.Models.Basket;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface IBasketService
    {
        Task<bool> SaveOrUpdate(BasketVM basketVM);
        Task<BasketVM> Get();
        Task<bool> Delete();
        Task AddBasketItem(BasketItemVM basketItemVM);
        Task<bool> RemoveBasketItem(string coursesId);
        Task<bool> ApplyDiscount(string discountCode);
        Task<bool> CancelApplyDiscount();
    }
}
