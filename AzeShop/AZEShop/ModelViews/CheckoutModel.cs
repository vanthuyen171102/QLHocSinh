namespace AzeShop.ModelViews
{
    public class CheckoutModel
    {
        public int CustomerId { get; set; }

        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int City { get; set; }
        public int District { get; set; }
        public int Ward { get; set; }
        public string? Note { get; set; }
    }
}
