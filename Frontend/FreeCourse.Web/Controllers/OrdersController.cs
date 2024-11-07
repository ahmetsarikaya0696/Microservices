using FreeCourse.Web.Models.Order;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public OrdersController(IBasketService basketService, IOrderService orderService)
        {
            _basketService = basketService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Checkout()
        {
            var basket = await _basketService.Get();
            ViewBag.Basket = basket;

            return View(new CheckoutInfoInput());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutInfoInput checkoutInfoInput)
        {
            // 1.Yol Senkron İletişim
            //var orderStatus = await _orderService.CreateOrder(checkoutInfoInput);

            // 2.Yol Asenkron İletişim
            var orderSuspend = await _orderService.SuspendOrder(checkoutInfoInput);

            if (!orderSuspend.IsSuccessfull)
            {
                var basket = await _basketService.Get();
                ViewBag.Basket = basket;

                ViewBag.Error = orderSuspend.Error;

                return View();
            }

            // 1.Yol Senkron İletişim
            //return RedirectToAction(nameof(SuccessfullCheckout), new { orderId = orderSuspend.OrderId });

            // 2.Yol Asenkron İletişim
            return RedirectToAction(nameof(SuccessfullCheckout), new { orderId = new Random().Next(1, 1000) });
        }

        public IActionResult SuccessfullCheckout(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }

        public async Task<IActionResult> CheckoutHistory()
        {
            var orders = await _orderService.GetOrders();
            return View(orders);
        }
    }
}
