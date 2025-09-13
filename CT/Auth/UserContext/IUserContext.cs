
namespace CT.Auth
{
    public interface IUserContext
    {
        string UserId { get; set; }
        string Username { get; set; }
        string Email { get; set; }
        string[] Roles { get; set; }
        string[] Scopes { get; set; }
        string AccessToken { get; set; }
    }


}

