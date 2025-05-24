using System.ComponentModel.DataAnnotations;

namespace BasketService.Api.Core.Domain.Models
{
    public class BasketItem : IValidatableObject
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (Quantity < 1)
                results.Add(new ValidationResult("Invalid number of units", new[] { "Quantity" }));

            return results;
        }
    }
}
