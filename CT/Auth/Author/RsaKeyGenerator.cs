
using System.Security.Cryptography;
using System.Text;


namespace CT.Auth
{


    /*
     * util gen key
     * RsaKeyGenerator.Export();
     * */
    class RsaKeyGenerator
    {
        public static void Export()
        {
            using var rsa = RSA.Create(2048); // Tạo RSA key 2048-bit

            // Export PRIVATE KEY dưới dạng PKCS#8
            var privateKeyBytes = rsa.ExportPkcs8PrivateKey();
            var privatePem = ExportToPem("PRIVATE KEY", privateKeyBytes);
            File.WriteAllText("Keys/private.pem", privatePem);

            // Export PUBLIC KEY (SubjectPublicKeyInfo)
            var publicKeyBytes = rsa.ExportSubjectPublicKeyInfo();
            var publicPem = ExportToPem("PUBLIC KEY", publicKeyBytes);
            File.WriteAllText("Keys/public.pem", publicPem);

            Console.WriteLine("✅ RSA key pair generated and saved to 'Keys/' folder.");
        }

        // Hàm export chuỗi base64 có định dạng PEM
        private static string ExportToPem(string label, byte[] bytes)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"-----BEGIN {label}-----");

            string base64 = Convert.ToBase64String(bytes);
            int offset = 0;
            const int LineLength = 64;
            while (offset < base64.Length)
            {
                int lineEnd = Math.Min(LineLength, base64.Length - offset);
                builder.AppendLine(base64.Substring(offset, lineEnd));
                offset += lineEnd;
            }

            builder.AppendLine($"-----END {label}-----");
            return builder.ToString();
        }
    }



}

