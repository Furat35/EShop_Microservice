namespace BasketService.Api.Core.Domain.Models
{
    public class CustomerBasket
    {
        public Guid UserId { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

        public CustomerBasket()
        {

        }

        public CustomerBasket(Guid userId)
        {
            UserId = userId;
        }
    }
}
