using FreeCourse.Services.Order.Application.Commands;
using FreeCourse.Services.Order.Application.Queries;
using FreeCourse.Shared.BaseController;
using FreeCourse.Shared.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Order.API.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrdersController(IMediator mediator, ISharedIdentityService sharedIdentityService)
        {
            _mediator = mediator;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var response = await _mediator.Send(new GetOrdersByUserIdQuery() { UserId = _sharedIdentityService.GetUserId });
            return CreateActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderCommand createOrderCommand)
        {
            var response = await _mediator.Send(createOrderCommand);
            return CreateActionResult(response);
        }


    }
}
