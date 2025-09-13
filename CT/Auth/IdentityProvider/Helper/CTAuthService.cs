 
namespace CT.Auth
{

    public static class CTAuthService
    {
        public static string HashPassword(string plainPassword)
        {
            // Tự động sinh salt và hash
            return BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }
        public static bool VerifyPassword(string plainPassword, string hashedPasswordFromDb)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPasswordFromDb);
        }

        public static bool Login(string username, string password)
        {
            var rs = false;

            var hashPass = HashPassword(password);

            //get from database by username, active...
            var hashDB = "";

            // 1. Kiểm tra user/pass
            if (username == "admin") //&&&hashDB
            {
                rs = true;
            }

            return rs;
        }


        
        public static bool Signin(string username, string password)
        {
            var rs = false;

            var hashPass = HashPassword(password);

            
            //save username, password to db
            rs = true;


            return rs;
        }


        public static void Save(string token, string username)
        { 
        }

        public static void Revoke(string token)
        { 
        }

        public static string? GetUsernameByToken(string token)
        {
            //from db
            return "admin";
        }

        public static bool IsValid(string token)
        {

            //from db
            return true;
        }


    }

}

