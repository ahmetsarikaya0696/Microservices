using FreeCourse.Services.Order.Application.Commands;
using FreeCourse.Services.Order.Application.DTOs;
using FreeCourse.Services.Order.Domain.OrderAggregate;
using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ResponseDTO<CreatedOrderDTO>>
    {
        private readonly OrderDbContext _orderDbContext;

        public CreateOrderCommandHandler(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task<ResponseDTO<CreatedOrderDTO>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var newAddress = new Address(request.Address.Province, request.Address.District, request.Address.Street, request.Address.ZipCode, request.Address.Line);

            var newOrder = new Domain.OrderAggregate.Order(request.BuyerId, newAddress);

            request.OrderItems.ForEach(orderItem =>
            {
                newOrder.AddOrderItem(orderItem.ProductId, orderItem.ProductName, orderItem.Price, orderItem.PictureUrl);
            });

            await _orderDbContext.Orders.AddAsync(newOrder);

            await _orderDbContext.SaveChangesAsync();

            return ResponseDTO<CreatedOrderDTO>.Success(new CreatedOrderDTO() { OrderId = newOrder.Id }, 200);
        }
    }
}
