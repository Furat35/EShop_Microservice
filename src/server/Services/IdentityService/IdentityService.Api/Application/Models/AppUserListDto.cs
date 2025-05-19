namespace IdentityService.Api.Application.Models
{
    public class AppUserListDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
    }
}
