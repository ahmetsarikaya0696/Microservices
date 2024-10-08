﻿using FreeCourse.Services.Order.Application.DTOs;
using FreeCourse.Shared.DTOs;
using MediatR;

namespace FreeCourse.Services.Order.Application.Queries
{
    public class GetOrdersByUserIdQuery : IRequest<ResponseDTO<List<OrderDTO>>>
    {
        public string UserId { get; set; }
    }
}
