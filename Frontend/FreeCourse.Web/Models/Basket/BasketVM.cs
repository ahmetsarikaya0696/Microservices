namespace FreeCourse.Web.Models.Basket
{
    public class BasketVM
    {
        public BasketVM()
        {
            _basketItems = new List<BasketItemVM>();
        }

        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public int? DiscountRate { get; set; }
        private List<BasketItemVM> _basketItems;
        public List<BasketItemVM> BasketItems
        {
            get
            {
                if (HasDiscount)
                {
                    _basketItems.ForEach(item =>
                    {
                        var discountPrice = item.Price * ((decimal)DiscountRate.Value / 100);
                        item.AppliedDiscount(Math.Round(item.Price - discountPrice, 2));
                    });
                }

                return _basketItems;
            }
            set
            {
                _basketItems = value;
            }
        }
        public decimal TotalPrice => BasketItems.Sum(x => x.GetCurrentPrice * x.Quantity);
        public bool HasDiscount => !string.IsNullOrEmpty(DiscountCode) && DiscountRate.HasValue;

        public void CancelDiscount()
        {
            DiscountCode = null;
            DiscountRate = null;
        }

        public void ApplyDiscount(string discountCode, int discountRate)
        {
            DiscountCode = discountCode;
            DiscountRate = discountRate;
        }
    }
}
