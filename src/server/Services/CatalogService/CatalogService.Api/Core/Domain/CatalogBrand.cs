using CommonLibrary.Models;

namespace CatalogService.Api.Core.Domain
{
    public class CatalogBrand : IEntity<int>
    {
        public int Id { get; set; }
        public string Brand { get; set; }
    }
}
