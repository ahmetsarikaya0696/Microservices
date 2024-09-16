namespace FreeCourse.Services.Basket.DTOs
{
    public class BasketDTO
    {
        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public List<BasketItemDTO> BasketItems { get; set; }
        public decimal TotalPrice => BasketItems.Sum(x => x.Price * x.Quantity);
    }
}
