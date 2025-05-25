using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Domain.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace BasketService.Api.Infrastructure.Repository
{
    public class BasketRepository(ILoggerFactory loggerFactory, ConnectionMultiplexer redis) : IBasketRepository
    {
        private readonly ILogger<BasketRepository> _logger = loggerFactory.CreateLogger<BasketRepository>();
        private readonly ConnectionMultiplexer _redis = redis;
        private readonly IDatabase _database = redis.GetDatabase();

        public async Task<bool> DeleteBasketAsync(Guid id)
        {
            return await _database.KeyDeleteAsync(id.ToString().ToLower());
        }

        public async Task<Basket?> GetBasketAsync(Guid id)
        {
            var data = await _database.StringGetAsync(id.ToString().ToLower());
            if (data.IsNullOrEmpty)
                return null;

            return JsonConvert.DeserializeObject<Basket>(data);
        }

        public IEnumerable<string> GetUsers()
        {
            var server = GetServer();
            var data = server.Keys();

            return data?.Select(k => k.ToString());
        }

        public async Task<bool> UpdateBasketAsync(Basket basket)
        {
            var created = await _database.StringSetAsync(basket.UserId.ToString().ToLower(), JsonConvert.SerializeObject(basket));
            if (!created)
            {
                _logger.LogInformation("Problem occur persisting the item.");
                return false;
            }

            _logger.LogInformation("Basket item persisted succesfully.");

            return created;
        }

        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }
    }
}
