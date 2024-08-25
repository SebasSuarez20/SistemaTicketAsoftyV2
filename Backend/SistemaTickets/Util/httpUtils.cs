using Microsoft.IdentityModel.Tokens;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using System;

namespace SistemaTickets.Util
{
    public static class httpUtils
    {
        private static IConfiguration _configuration;
        public static string key = "";

        public static string generateToken(string roleCode = null, string usernameFk = null)
        {

            string typeName = !string.IsNullOrEmpty(usernameFk) ? ClaimTypes.Name : "";
            string typeRol = !string.IsNullOrEmpty(roleCode) ? ClaimTypes.Role : "";

            var claims = new[]
            {
                new Claim(typeName,usernameFk ?? ""),
                new Claim(typeRol,roleCode ?? ""),
            };


            var strlKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var strlPswd = new SigningCredentials(strlKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(60),
                    signingCredentials: strlPswd
            );

            string rstToken = new JwtSecurityTokenHandler().WriteToken(token);
            return rstToken;
        }

        public static string convertToSha256(string sendTo)
        {
            // Calcular el hash SHA-256 de los datos ordenados
            byte[] bytes = Encoding.UTF8.GetBytes(sendTo);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(bytes);
                // Convertir el hash a una cadena hexadecimal para obtener el resultado cifrado
                string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hashString;
            }
        }

        public static Data response(object information =null,string message = null,int status=StatusCodes.Status200OK)
        {
            return new Data
            {
                 data = information,
                 message = message,
                 status = status
            };
        }
    }

    public class Data{
        public object data { get; set; }
        public string? message {  get; set; }
        public int? status { get; set; }
    }

}
