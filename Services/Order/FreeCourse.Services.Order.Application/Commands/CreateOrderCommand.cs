using FreeCourse.Services.Order.Application.DTOs;
using FreeCourse.Shared.DTOs;
using MediatR;

namespace FreeCourse.Services.Order.Application.Commands
{
    public class CreateOrderCommand : IRequest<ResponseDTO<CreatedOrderDTO>>
    {
        public string BuyerId { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
        public AddressDTO Address { get; set; }
    }
}
