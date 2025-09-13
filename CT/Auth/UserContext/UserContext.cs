

namespace CT.Auth
{
    public class UserContext : IUserContext
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string[] Roles { get; set; } = Array.Empty<string>();
        public string[] Scopes { get; set; } = Array.Empty<string>();
        public string AccessToken { get; set; } = string.Empty;
    }

}

