using SistemaTickets.Interface.IModel;
using SistemaTickets.Model;
using SistemaTickets.Services.HttpUtil;
using SistemaTickets.Services.Jwt;
using System.Dynamic;

namespace SistemaTickets.Services
{
    public class fileService : IFile
    {

        private readonly IdbHandler<Users> _dbHandlerUser;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor Icontext;
        private string user;

        public fileService(IConfiguration configuration,IdbHandler<Users> dbHandlerUser, IHttpContextAccessor icontext)
        {

            _configuration = configuration;
            _dbHandlerUser = dbHandlerUser;
            Icontext = icontext;
            user = authorizeServices.GetUserName(Icontext);
        }

        public async Task<object> createFile(IFormFile file)
        {
            dynamic response = new ExpandoObject();
            bool isFlag = true;
            
            try
            {
                string random = $"{Guid.NewGuid().ToString()}.jpg";
                string strlPath = _configuration["pathFile:path"].Replace("\\","/");
                List<Users> taskUser =(List<Users>) await _dbHandlerUser.GetAllAsyncForAllWithClouse((int)logicalNode.True);
                string combinePath = Path.Combine(strlPath, $"{taskUser?.First()?.PhotoPerfil ?? "user"}").Replace("\\","/");


                if (File.Exists(combinePath))
                {
                    isFlag = false;
                    string fileDeletePath = $"{strlPath}/{taskUser.First()?.PhotoPerfil}";
                    File.Delete($"{fileDeletePath}");
                }
                

                taskUser.First().PhotoPerfil = $"/{random}".Replace("/", "");
                combinePath = Path.Combine(strlPath, $"{random}").Replace("\\", "/");
                await _dbHandlerUser.UpdateAsyncAll(taskUser.First(), new Users { Idcontrol = int.Parse(user) });
                 createDirectoryAndFile(file, combinePath);

                response.status = 200;
                response.message = $"Success: {(isFlag ? "Creado" : "Actualizado")} correctamente";
                return response;
            }
            catch (Exception ex)
            {
                response.status = 400;
                response.message = $"Error : {ex.Message}";
                return response;
            }

            return null;
        }

        public async Task createDirectoryAndFile(IFormFile file,string combinePath)
        {
            try
            {
                using (var stream = new FileStream(combinePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    stream.Close();
                }
            }catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
        
   
    }
}
