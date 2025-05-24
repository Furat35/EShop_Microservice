using BasketService.Api.ExternalRestServices.Interfaces;
using Consul;

namespace BasketService.Api.ExternalRestServices
{
    public class CatalogService(IConsulClient consulClient, HttpClient httpClient, IConfiguration configuration) : ICatalogService
    {
       public async Task GetDiscountsByItemIds(string ids)
        {
            var services = consulClient.Health.Service(configuration["ConsulServiceNames:BasketService"]);
            var serviceEntries = services.Result.Response;
            var randomServiceIndex = new Random().Next(0, serviceEntries.Length);
            var serviceEntry = serviceEntries[randomServiceIndex];
            if (serviceEntry is null)
                throw new Exception($"Service '{configuration["ConsulServiceNames:BasketService"]}' not found in Consul");
            var address = serviceEntry.Service.Address;
            var port = serviceEntry.Service.Port;
            var requestEndpoint = new Uri($"http://{address}:{port}/{ids}");
            var response = await httpClient.GetAsync(requestEndpoint);
        }

    }
}
