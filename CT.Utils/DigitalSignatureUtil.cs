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
    public class DigitalSignatureUtil
    {

        /// <summary>
        /// Khởi tạo RSA Key Pair (Khóa công khai và khóa riêng)
        //        Tạo một chuỗi dữ liệu để ký(Token hoặc dữ liệu bất kỳ)
        //Sử dụng khóa riêng để ký dữ liệu(Chữ ký số)
        //Xác minh chữ ký với khóa công khai
        /// </summary>
        public static void RSA_sign()
        {
            // Bước 1: Tạo khóa RSA
            using (RSA rsa = RSA.Create(2048))
            {
                // Xuất khóa công khai và khóa riêng
                RSAParameters privateKey = rsa.ExportParameters(true);
                RSAParameters publicKey = rsa.ExportParameters(false);

                // Bước 2: Tạo dữ liệu cần ký (Token)
                string tokenData = "This is some data to sign.";
                byte[] dataToSign = Encoding.UTF8.GetBytes(tokenData);

                // Bước 3: Ký dữ liệu bằng khóa riêng
                byte[] signature;
                   RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                {
                  //  rsaFormatter.SetKey(privateKey);
                    rsaFormatter.SetHashAlgorithm("SHA256");
                    signature = rsaFormatter.CreateSignature(dataToSign);
                }

                // Bước 4: Xác minh chữ ký với khóa công khai
                bool isVerified = false;
                RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter();
                {
                    rsaDeformatter.SetKey(rsa);
                    rsaDeformatter.SetHashAlgorithm("SHA256");
                    isVerified = rsaDeformatter.VerifySignature(dataToSign, signature);
                }

                // Hiển thị kết quả
                Console.WriteLine($"Signature Verified: {isVerified}");
            }
        }


        /// <summary>
        ///  ECDSA (Elliptic Curve Digital Signature Algorithm)
        /// </summary>
        static void ECDSA()
        {
            // Bước 1: Tạo khóa ECDSA
            using (ECDsa ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256))
            {
                // Xuất khóa công khai và khóa riêng
                ECParameters privateKey = ecdsa.ExportParameters(true);
                ECParameters publicKey = ecdsa.ExportParameters(false);

                // Bước 2: Tạo dữ liệu cần ký (Token)
                string tokenData = "This is some data to sign.";
                byte[] dataToSign = Encoding.UTF8.GetBytes(tokenData);

                // Bước 3: Ký dữ liệu bằng khóa riêng
                byte[] signature = ecdsa.SignData(dataToSign, HashAlgorithmName.SHA256);

                // Bước 4: Xác minh chữ ký với khóa công khai
                bool isVerified = ecdsa.VerifyData(dataToSign, signature, HashAlgorithmName.SHA256);

                // Hiển thị kết quả
                Console.WriteLine($"Signature Verified: {isVerified}");
            }
        }
        }
}
