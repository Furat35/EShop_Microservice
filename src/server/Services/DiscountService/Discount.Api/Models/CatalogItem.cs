using CommonLibrary.Models;

namespace Discount.Api.Models
{
    public class CatalogItem : IEntity<int>
    {
        public int Id { get; set; }
        public int DiscountId { get; set; }
        public Discount Discount { get; set; }
    }
}
