using OrderService.Domain.SeedWork;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Domain.AggregateModels.OrderAggregate
{
    public class OrderItem : BaseEntity, IValidatableObject
    {
        public int ProductId { get; private set; }

        public string ProductName { get; private set; }

        public string PictureUrl { get; private set; }

        public decimal UnitPrice { get; private set; }

        public int Units { get; private set; }

        protected OrderItem()
        {

        }

        public OrderItem(int productId, string productName, decimal unitPrice, string pictureUrl, int units = 1)
        {
            ProductId = productId;

            ProductName = productName;
            UnitPrice = unitPrice;
            Units = units;
            PictureUrl = pictureUrl;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (Units <= 0)
                results.Add(new ValidationResult("Invalid number of units", new[] { "Units" }));

            return results;
        }
    }
}
