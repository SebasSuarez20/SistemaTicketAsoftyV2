
using Org.BouncyCastle.Utilities;
using QRCoder;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SistemaTickets.Services.Jwt
{
    public static class authorizeServices
    {


        public static byte[] IV = Encoding.UTF8.GetBytes("_@3x565_9012@@_6"); // 16 bytes for AES-128
        public static byte[] Key = Encoding.UTF8.GetBytes("_@3x565_9012@@_6"); // 16 bytes for AES-128

        public static string GetUserName(IHttpContextAccessor _context)
        {
            var getUser = _context.HttpContext?.User.FindFirst(ClaimTypes.Name);
            return getUser?.Value ?? "";
        }

        public static int GetRoleUser(IHttpContextAccessor _context)
        {
            var getRol = _context.HttpContext?.User.FindFirst(ClaimTypes.Role);
            return int.Parse(getRol?.Value ?? "-1");
        }

        public static string GetIdHub(IHttpContextAccessor _context)
        {
            var idHub = _context.HttpContext?.User?.FindFirst("miHub");
            return idHub.Value.ToString() ?? "-1";
        }

        public static dynamic GenerateCodeQr()
        {

            byte[] randomBytes = new byte[16];
            Random rng = new Random();
            rng.NextBytes(randomBytes);

            string base64String = Convert.ToBase64String(randomBytes);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData dataQr = qrGenerator.CreateQrCode(base64String, QRCodeGenerator.ECCLevel.Q);

            PngByteQRCode imageQr = new PngByteQRCode(dataQr);
            byte[] base64Qr = imageQr.GetGraphic(20);

            return new {base64 = base64Qr,information= Encrypt(base64String)
        };
        }

        private static string Encrypt(string plainText)
        {

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

    }
}
