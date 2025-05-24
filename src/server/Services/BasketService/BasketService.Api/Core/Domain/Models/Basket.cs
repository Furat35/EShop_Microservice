namespace BasketService.Api.Core.Domain.Models
{
    public class Basket
    {
        public Guid UserId { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

        public Basket()
        {

        }

        public Basket(Guid userId)
        {
            UserId = userId;
        }
    }
}
