//using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CT.Utils
{


    /// <summary>
    /// Chữ ký số (ngược với mã hóa bất đối xứng)
    /// </summary>
    public class EncryptMessageUtil
    {



        public static void Main(string[] args)
        {
            // Tạo đối tượng RSA
            using (RSA rsa = RSA.Create())
            {
                // Sinh cặp khóa công khai và khóa bí mật
                rsa.KeySize = 2048; // Chọn kích thước khóa (2048 bits là phổ biến)

                // Lấy khóa công khai
                string publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
                Console.WriteLine("Public Key: " + publicKey);

                // Lấy khóa bí mật
                string privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
                Console.WriteLine("Private Key: " + privateKey);

                // Thông điệp cần mã hóa
                string message = "Hello, RSA!";
                Console.WriteLine("Original message: " + message);

                // Mã hóa thông điệp bằng khóa công khai
                byte[] encryptedMessage = EncryptMessage(message, publicKey);
                Console.WriteLine("Encrypted message: " + Convert.ToBase64String(encryptedMessage));

                // Giải mã thông điệp bằng khóa bí mật
                string decryptedMessage = DecryptMessage(encryptedMessage, privateKey);
                Console.WriteLine("Decrypted message: " + decryptedMessage);
            }
        }

        // Hàm mã hóa thông điệp bằng khóa công khai
        public static byte[] EncryptMessage(string message, string publicKey)
        {
            using (RSA rsa = RSA.Create())
            {
                // Chuyển đổi chuỗi Base64 thành byte[]
                rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);

                // Chuyển thông điệp thành byte[]
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);

                // Mã hóa thông điệp
                return rsa.Encrypt(messageBytes, RSAEncryptionPadding.OaepSHA256);
            }
        }

        // Hàm giải mã thông điệp bằng khóa bí mật
        public static string DecryptMessage(byte[] encryptedMessage, string privateKey)
        {
            using (RSA rsa = RSA.Create())
            {
                // Chuyển đổi chuỗi Base64 thành byte[]
                rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);

                // Giải mã thông điệp
                byte[] decryptedBytes = rsa.Decrypt(encryptedMessage, RSAEncryptionPadding.OaepSHA256);

                // Chuyển byte[] thành chuỗi
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }

    }
}
