using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Application.Services;
using BasketService.Api.Core.Domain.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace BasketService.Api.Infrastructure.Repository
{
    public class BasketRepository(ILoggerFactory loggerFactory, ConnectionMultiplexer redis, IIdentityService identityService) : IBasketRepository
    {
        private readonly ILogger<BasketRepository> _logger = loggerFactory.CreateLogger<BasketRepository>();
        private readonly ConnectionMultiplexer _redis = redis;
        private readonly IDatabase _database = redis.GetDatabase();

        public async Task<bool> DeleteBasketAsync(Guid id)
        {
            return await _database.KeyDeleteAsync(id.ToString().ToLower());
        }

        public async Task<CustomerBasket?> GetBasketAsync(Guid id)
        {
            var data = await _database.StringGetAsync(id.ToString().ToLower());
            if (data.IsNullOrEmpty)
                return null;

            return JsonConvert.DeserializeObject<CustomerBasket>(data);
        }

        public IEnumerable<string> GetUsers()
        {
            var server = GetServer();
            var data = server.Keys();

            return data?.Select(k => k.ToString());
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var created = await _database.StringSetAsync(basket.UserId.ToString().ToLower(), JsonConvert.SerializeObject(basket));
            if (!created)
            {
                _logger.LogInformation("Problem occur persisting the item.");
                return null;
            }

            _logger.LogInformation("Basket item persisted succesfully.");

            return await GetBasketAsync(basket.UserId);
        }

        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }
    }
}
