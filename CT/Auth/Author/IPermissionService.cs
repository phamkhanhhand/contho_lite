
namespace CT.Auth
{

    public interface IPermissionService
    {
        Task<HashSet<string>> GetPermissionsAsync(string token);
        Task<HashSet<string>> GetDataContext(string token, string username);
    }


}

