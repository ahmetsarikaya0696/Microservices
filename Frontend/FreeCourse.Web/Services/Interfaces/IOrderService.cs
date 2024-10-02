using FreeCourse.Web.Models.Order;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderCreatedVM> CreateOrder(CheckoutInfoInput checkoutInfoInput); // senkron iletişim
        Task SuspendOrder(CheckoutInfoInput checkoutInfoInput); // asenkron iletişim
        Task<List<OrderVM>> GetOrders();
    }
}
