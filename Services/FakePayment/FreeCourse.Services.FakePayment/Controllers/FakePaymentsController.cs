using FreeCourse.Services.FakePayment.Models;
using FreeCourse.Shared.BaseController;
using FreeCourse.Shared.DTOs;
using FreeCourse.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.FakePayment.Controllers
{
    public class FakePaymentsController : BaseController
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public FakePaymentsController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> ReceivePayment(PaymentDTO paymentDTO)
        {
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));

            var createOrderMessageCommand = new CreateOrderMessageCommand()
            {
                BuyerId = paymentDTO.Order.BuyerId,
                Province = paymentDTO.Order.Address.Province,
                District = paymentDTO.Order.Address.District,
                Street = paymentDTO.Order.Address.Street,
                ZipCode = paymentDTO.Order.Address.ZipCode,
                Line = paymentDTO.Order.Address.Line,
                OrderItems = paymentDTO.Order.OrderItems.Select(x => new OrderItem()
                {
                    PictureUrl = x.PictureUrl,
                    Price = x.Price,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName
                }).ToList(),
            };

            await sendEndpoint.Send(createOrderMessageCommand);

            return CreateActionResult(ResponseDTO<NoContentDTO>.Success(200));
        }
    }
}
