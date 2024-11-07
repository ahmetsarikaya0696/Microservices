using FreeCourse.Shared.DTOs;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.FakePayment;
using FreeCourse.Web.Models.Order;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly IPaymentService _paymentService;
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrderService(HttpClient httpClient, IPaymentService paymentService, IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _httpClient = httpClient;
            _paymentService = paymentService;
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<OrderCreatedVM> CreateOrder(CheckoutInfoInput checkoutInfoInput)
        {
            var basket = await _basketService.Get();
            var payment = new PaymentInfoInput()
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                CVV = checkoutInfoInput.CVV,
                Expiration = checkoutInfoInput.Expiration,
                TotalPrice = basket.TotalPrice,
            };

            var responsePayment = await _paymentService.ReceivePayment(payment);

            if (!responsePayment)
            {
                return new OrderCreatedVM()
                {
                    Error = "Ödeme alınamadı!",
                    IsSuccessfull = false
                };
            }

            var orderCreateInput = new OrderCreateInput()
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = new AddressCreateInput()
                {
                    Province = checkoutInfoInput.Province,
                    District = checkoutInfoInput.District,
                    Line = checkoutInfoInput.Line,
                    Street = checkoutInfoInput.Street,
                    ZipCode = checkoutInfoInput.ZipCode,
                },
                OrderItems = basket.BasketItems.Select(x => new OrderItemCreateInput()
                {
                    ProductId = x.CourseId,
                    Price = x.GetCurrentPrice,
                    ProductName = x.CourseName,
                    PictureUrl = string.Empty,
                }).ToList()
            };

            var orderResponse = await _httpClient.PostAsJsonAsync("orders", orderCreateInput);

            if (!orderResponse.IsSuccessStatusCode)
            {
                return new OrderCreatedVM()
                {
                    Error = "Sipariş oluşturulurken bir hata oluştu!",
                    IsSuccessfull = false
                };
            }

            var orderCreatedVM = await orderResponse.Content.ReadFromJsonAsync<ResponseDTO<OrderCreatedVM>>();
            orderCreatedVM.Data.IsSuccessfull = true;
            await _basketService.Delete();

            return orderCreatedVM.Data;
        }

        public async Task<List<OrderVM>> GetOrders()
        {
            var response = await _httpClient.GetFromJsonAsync<ResponseDTO<List<OrderVM>>>("orders");
            return response.Data;
        }

        public async Task<OrderSuspendVM> SuspendOrder(CheckoutInfoInput checkoutInfoInput)
        {
            var basket = await _basketService.Get();

            var orderCreateInput = new OrderCreateInput()
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = new AddressCreateInput()
                {
                    Province = checkoutInfoInput.Province,
                    District = checkoutInfoInput.District,
                    Line = checkoutInfoInput.Line,
                    Street = checkoutInfoInput.Street,
                    ZipCode = checkoutInfoInput.ZipCode,
                },
                OrderItems = basket.BasketItems.Select(x => new OrderItemCreateInput()
                {
                    ProductId = x.CourseId,
                    Price = x.GetCurrentPrice,
                    ProductName = x.CourseName,
                    PictureUrl = string.Empty,
                }).ToList()
            };

            var payment = new PaymentInfoInput()
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                CVV = checkoutInfoInput.CVV,
                Expiration = checkoutInfoInput.Expiration,
                TotalPrice = basket.TotalPrice,
                Order = orderCreateInput,
            };

            var responsePayment = await _paymentService.ReceivePayment(payment);

            if (!responsePayment)
            {
                return new OrderSuspendVM()
                {
                    Error = "Ödeme alınamadı!",
                    IsSuccessfull = false
                };
            }

            await _basketService.Delete();
            return new OrderSuspendVM() { IsSuccessfull = true };
        }
    }
}
