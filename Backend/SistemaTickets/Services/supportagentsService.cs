﻿using Microsoft.IdentityModel.Tokens;
using SistemaTickets.Interface.IModel;
using SistemaTickets.Model;
using System.Security.Claims;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Dynamic;

namespace SistemaTickets.Services
{
    public class supportagentsService : ISupportagent
    {

        private readonly IdbHandler<supportagents> _dbHandlerSupport;
        private IConfiguration _config;

        public supportagentsService(IdbHandler<supportagents> dbHandlerSupport,IConfiguration config)
        {
            this._dbHandlerSupport = dbHandlerSupport;
            this._config = config;
        }

        public async Task<object> authLoginSupport(string user,string pswd)
        {
            try
            {
                dynamic response = new ExpandoObject();

                var resultAuth = await _dbHandlerSupport.GetAllAsyncForAll(s => s.userName == user && s.Password == pswd);

                if (resultAuth.Count() != 0)
                {
                    response.username = resultAuth.First()?.userName;
                    response.rolCode = resultAuth.First()?.RoleCode;
                    response.nameUser = resultAuth.First()?.NameSupport;
                    response.photo = resultAuth.First()?.PhotoPerfil;
                    response.token = generateToken(resultAuth.First()?.RoleCode.ToString(), resultAuth.First().Idcontrol.ToString());
                    response.status = 200;
                    response.message = $"Ingreso correctamente el usuario: {resultAuth.First()?.NameSupport}";
                    return response;
                }
                response.status = 400;
                response.message = "Error: No se encontró ningún información sobre el usuario.";
                return response;
            }catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public string generateToken(string roleCode,string usernameFk)
        {

            var claims = new[]
         {
                new Claim(ClaimTypes.Name,usernameFk),
                new Claim(ClaimTypes.Role,roleCode)
            };

            var strlKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JWT:Key").Value));
            var strlPswd = new SigningCredentials(strlKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(60),
                    signingCredentials: strlPswd
            );

            string rstToken = new JwtSecurityTokenHandler().WriteToken(token);


            return rstToken;

        }
    }
}
