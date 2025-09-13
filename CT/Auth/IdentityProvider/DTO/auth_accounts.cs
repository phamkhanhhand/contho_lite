 
namespace CT.Auth
{
    public class auth_accounts
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password_hash { get; set; } // Lưu mật khẩu đã băm bằng BCrypt
    }


}

