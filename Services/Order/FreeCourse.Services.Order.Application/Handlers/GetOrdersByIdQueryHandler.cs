using FreeCourse.Services.Order.Application.DTOs;
using FreeCourse.Services.Order.Application.Mapping;
using FreeCourse.Services.Order.Application.Queries;
using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FreeCourse.Services.Order.Application.Handlers
{
    public class GetOrdersByIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, ResponseDTO<List<OrderDTO>>>
    {
        private readonly OrderDbContext _orderDbContext;

        public GetOrdersByIdQueryHandler(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task<ResponseDTO<List<OrderDTO>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderDbContext.Orders.Include(x => x.OrderItems)
                                                     .Where(x => x.BuyerId == request.UserId)
                                                     .ToListAsync();

            if (!orders.Any()) ResponseDTO<List<OrderDTO>>.Success(Enumerable.Empty<OrderDTO>().ToList(), 200);

            var orderDTOs = ObjectMapper.Mapper.Map<List<OrderDTO>>(orders);

            return ResponseDTO<List<OrderDTO>>.Success(orderDTOs, 200);
        }
    }
}
