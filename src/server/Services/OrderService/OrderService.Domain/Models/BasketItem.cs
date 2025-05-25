namespace OrderService.Domain.Models
{
    public class BasketItem
    {
        public string Id { get; init; }

        public int ItemId { get; init; }

        public string ItemName { get; init; }

        public decimal UnitPrice { get; init; }

        public decimal OldUnitPrice { get; init; }

        public int Quantity { get; init; }

        public string PictureUrl { get; init; }
    }
}
