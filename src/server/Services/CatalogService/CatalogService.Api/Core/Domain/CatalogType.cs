using CommonLibrary.Models;

namespace CatalogService.Api.Core.Domain
{
    public class CatalogType : IEntity<int>
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }
}
