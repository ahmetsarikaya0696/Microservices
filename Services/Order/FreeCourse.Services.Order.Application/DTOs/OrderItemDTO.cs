﻿namespace FreeCourse.Services.Order.Application.DTOs
{
    public class OrderItemDTO
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
    }
}
