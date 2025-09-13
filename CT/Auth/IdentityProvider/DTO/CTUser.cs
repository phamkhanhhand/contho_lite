 
namespace CT.Auth
{
    public class CTUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; } // Lưu mật khẩu đã băm bằng BCrypt
    }


}

