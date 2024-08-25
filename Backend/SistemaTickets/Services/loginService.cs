using SistemaTickets.Interface.IModel;
using SistemaTickets.Model;
using System.Dynamic;
using SistemaTickets.Model.View;
using SistemaTickets.Services.HttpUtil;
using SistemaTickets.Util;

namespace SistemaTickets.Services
{

    public class loginService : ILogin
    {

        private readonly IdbHandler<Users> _dbHandlerSupport;
        private readonly IdbHandler<ticketSupportViewChats> _db;
        private readonly IdbHandler<loggetUserDataView> _dbHandlerloggetUserDataView;
        private IConfiguration _config;


        public loginService(IdbHandler<Users> dbHandlerSupport, IdbHandler<ticketSupportViewChats> db, IConfiguration config, IdbHandler<loggetUserDataView> dbHandlerloggetUserDataView)
        {
            this._dbHandlerSupport = dbHandlerSupport;
            this._dbHandlerloggetUserDataView = dbHandlerloggetUserDataView;
            this._config = config;
            this._db = db;
            _config = config;
            httpUtils.key = _config.GetSection("JWT:Key").Value;
        }

        public async Task<object> authLoginSupport(string user, string pswd)
        {
            try
            {
                dynamic response = new ExpandoObject();

                var resultAuth = await _dbHandlerloggetUserDataView.GetAllAsyncForAllWithClouse((int)logicalNode.False, new loggetUserDataView { NameUser = user, Password = pswd });

                if (resultAuth.Count() != 0)
                {
                    response.idControl = resultAuth.First().Idcontrol;
                    response.username = resultAuth.First()?.NameUser;
                    response.rolCode = resultAuth.First()?.RoleCode;
                    response.themeColor = resultAuth.First().ThemeColor;
                    response.nameUser = resultAuth.First()?.NameSupport;
                    response.surName = resultAuth.First()?.Surname;
                    response.photo = resultAuth.First()?.PhotoPerfil;
                    response.token = httpUtils.generateToken(resultAuth.First()?.RoleCode.ToString(), resultAuth.First().Idcontrol.ToString());
                    response.status = 200;
                    response.message = $"Ingreso correctamente el usuario: {resultAuth.First()?.NameUser}";
                    return response;
                }
                response.status = 400;
                response.message = "Error: No se encontró ningún información sobre el usuario.";
                return response;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}

