using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Helpers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<TDbContext>(TDbContext context) : ControllerBase
        where TDbContext : class
    {
        protected TDbContext Context { get; } = context;

    }
}
