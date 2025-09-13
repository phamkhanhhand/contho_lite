 
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
            //coz bcrypt has salt with has,cant equal ==
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPasswordFromDb);
        }

        public static bool Login(string username, string password)
        {
            var rs = false;

            var hashPass = HashPassword(password);


            //get from database by username, active...


            AuthDL dl = new AuthDL();
            var acc = dl.GetLoginInfoByUsername(username);

            // 1. Kiểm tra user/pass
            if (acc == null)
            {
                rs = false;
            }
            else if (VerifyPassword(password, acc.password_hash))
            {
                rs = true;
            }


            return rs;
        }


        
        public static bool Signin(string username, string password)
        {
            AuthDL dl = new AuthDL();
            var rs = false;

            var hashPass = HashPassword(password);

            
            //save username, password to db
            auth_accounts acc = new auth_accounts();
            acc.username = username;
            acc.password_hash = hashPass;

            dl.Signin(acc);
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

