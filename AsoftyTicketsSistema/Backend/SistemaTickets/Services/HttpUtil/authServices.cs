
using QRCoder;
using System.Security.Claims;

namespace SistemaTickets.Services.Jwt
{
    public static class authService
    {

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

        public static string  generateCodeQr()
        {

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData dataQr = qrGenerator.CreateQrCode("Hola stella", QRCodeGenerator.ECCLevel.Q);

            PngByteQRCode imageQr = new PngByteQRCode(dataQr);
            byte[] base64Qr = imageQr.GetGraphic(20);
            string resultBase64 = Convert.ToBase64String(base64Qr);

            return resultBase64;
        }
    }
}
