using FreeCourse.Web.Handlers;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Extensions
{
    public static class ServiceExtension
    {
        public static void AddHttpClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceApiSettings = configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();

            services.AddHttpClient<IIdentityService, IdentityService>();
            services.AddHttpClient<IClientCredentialTokenService, ClientCredentialTokenService>();
            services.AddHttpClient<IUserService, UserService>(options =>
            {
                options.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri);
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IBasketService, BasketService>(options =>
            {
                options.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Basket.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<ICatalogService, CatalogService>(options =>
            {
                options.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            services.AddHttpClient<IPhotoStockService, PhotoStockService>(options =>
            {
                options.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.PhotoStock.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            services.AddHttpClient<IDiscountService, DiscountService>(options =>
            {
                options.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Discount.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IPaymentService, PaymentService>(options =>
            {
                options.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.FakePayment.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IOrderService, OrderService>(options =>
            {
                options.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Order.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
        }
    }
}
