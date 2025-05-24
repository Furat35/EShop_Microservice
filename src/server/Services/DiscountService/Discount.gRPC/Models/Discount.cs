using CommonLibrary.Models;

namespace Discount.gRPC.Models
{
    public class Discount : IEntity<int>
    {
        public int Id { get; set; }
        public float Percentage { get; set; }
        public float Amount { get; set; }
        public ICollection<CatalogItem> Items { get; set; }
    }
}
