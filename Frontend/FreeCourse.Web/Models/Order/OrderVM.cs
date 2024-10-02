namespace FreeCourse.Web.Models.Order
{
    public class OrderVM
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string BuyerId { get; set; }
        public List<OrderItemVM> OrderItems { get; set; }
    }
}
