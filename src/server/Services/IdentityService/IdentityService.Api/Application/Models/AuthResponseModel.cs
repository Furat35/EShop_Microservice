namespace IdentityService.Api.Application.Models
{
    public class AuthResponseModel
    {
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
