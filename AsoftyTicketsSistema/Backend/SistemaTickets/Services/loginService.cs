using Microsoft.IdentityModel.Tokens;
using SistemaTickets.Interface.IModel;
using SistemaTickets.Model;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Dynamic;
using SistemaTickets.Model.View;

namespace SistemaTickets.Services
{

    public class loginService : ILogin
    {

        private readonly IdbHandler<Users> _dbHandlerSupport;
        private readonly IdbHandler<ticketSupportViewChats> _db;
        private IConfiguration _config;


        public loginService(IdbHandler<Users> dbHandlerSupport, IdbHandler<ticketSupportViewChats> db ,IConfiguration config)
        {
            this._dbHandlerSupport = dbHandlerSupport;
            this._config = config;
            this._db = db;
        }

        public async Task<object> authLoginSupport(string user,string pswd)
        {
            try
            {
                dynamic response = new ExpandoObject();

                var resultAuth = await _dbHandlerSupport.GetAllAsyncForAll(s => s.nameUser == user && s.Password == pswd);

                if (resultAuth.Count() != 0)
                {
                    response.idControl = resultAuth.First().Idcontrol;
                    response.username = resultAuth.First()?.nameUser;
                    response.rolCode = resultAuth.First()?.RoleCode;
                    response.nameUser = resultAuth.First()?.NameSupport;
                    response.surName = resultAuth.First()?.Surname;
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
                new Claim(ClaimTypes.Role,roleCode),
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

        public async Task<object> spExample()
        {
            ticketSupportViewChats instance = new ticketSupportViewChats();
            instance.Consecutive = 1;
            return await _db.GetAllAsyncSp("GetChatsForConsecutive", instance);
        }
    }
}
