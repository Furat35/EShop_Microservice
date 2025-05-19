namespace BasketService.Api.Core.Application.Services
{
    public interface IIdentityService
    {
        Guid? GetUserId();
        string GetUsername();
    }
}
