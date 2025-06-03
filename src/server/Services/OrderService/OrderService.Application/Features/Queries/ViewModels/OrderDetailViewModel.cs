namespace OrderService.Application.Features.Queries.ViewModels
{
    public class OrderDetailViewModel
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; init; }
        public string Status { get; init; }
        public string Description { get; init; }
        public string Street { get; init; }
        public string City { get; init; }
        public string Zipcode { get; init; }
        public string Country { get; init; }
        public List<OrderItemViewModel> OrderItems { get; set; }
        public decimal Total { get; set; }
    }

    public class OrderItemViewModel
    {
        public Guid Id { get; set; }
        public string ItemName { get; init; }
        public int Units { get; init; }
        public double UnitPrice { get; init; }
        public string PictureUrl { get; init; }
    }
}
