using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace CT.Auth
{
    public static class CTJwtHelper
    {
        //để ra ngoài lấy cache tránh bị mở file confilict
        private static RsaSecurityKey SigningKey;



        public static string GenerateAccessToken(string username)
        {

            if (SigningKey == null)
            {

                // 1. Load private key để ký token
                var privateKeyText = System.IO.File.ReadAllText("Keys/private.pem");
                var rsa = RSA.Create();
                rsa.ImportFromPem(privateKeyText.ToCharArray());


                SigningKey = new RsaSecurityKey(rsa);

            }


            // 2. Tạo các claims
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Admin") // Hardcoded, bạn có thể lấy từ DB
        };

            // 3. Tạo thông tin token
            var token = new JwtSecurityToken(
                issuer: null, // bạn có thể đặt thành "your-app" nếu muốn kiểm tra
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(SigningKey, SecurityAlgorithms.RsaSha256)
            );


            // 4. Trả về token string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public static string GenerateSecureRefreshToken()
        {
            //var refreshToken = Guid.NewGuid().ToString(); 
            //return refreshToken;

            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);

        }

    }


}

