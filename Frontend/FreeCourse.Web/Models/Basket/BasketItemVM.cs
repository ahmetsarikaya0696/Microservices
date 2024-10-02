namespace FreeCourse.Web.Models.Basket
{
    public class BasketItemVM
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;

        private decimal? DiscountAppliedPrice;

        public decimal GetCurrentPrice => DiscountAppliedPrice != null ? DiscountAppliedPrice.Value : Price;

        public void AppliedDiscount(decimal discountPrice)
        {
            DiscountAppliedPrice = discountPrice;
        }
    }
}
