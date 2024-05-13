using SistemaTickets.Interface.IJwt;
using SistemaTickets.Interface.IModel;
using SistemaTickets.Model;
using System.Dynamic;
using System.IO;

namespace SistemaTickets.Services
{
    public class fileService : IFile
    {

        private readonly IdbHandler<Users> _dbHandlerUser;
        private readonly IConfiguration _configuration;
        private readonly IJwt _authJwt;

        public fileService(IConfiguration configuration,IJwt authJwt, IdbHandler<Users> dbHandlerUser)
        {

            _configuration = configuration;
            _authJwt = authJwt;
            _dbHandlerUser = dbHandlerUser;
        }

        public async Task<object> createFile(IFormFile file)
        {
            dynamic response = new ExpandoObject();

            bool isFlag = true;
            
            try
            {
                string pathfile = $"Support{this._authJwt.GetUserName()}";
                string random = $"{Guid.NewGuid().ToString()}.jpg";
                string strlPath = _configuration["pathFile:path"].Replace("\\","/") + $"/{pathfile}";
                List<Users> taskUser =(List<Users>) await _dbHandlerUser.GetAllAsyncForAll();
                string combinePath = Path.Combine(strlPath, random);

                if (!Directory.Exists(strlPath))
                {
                    isFlag = false;
                    Directory.CreateDirectory(strlPath);
                    taskUser.First().PhotoPerfil = pathfile+$"/{random}";
                    await _dbHandlerUser.UpdateAsyncAll(taskUser.First(),new Users { Idcontrol = int.Parse(this._authJwt.GetUserName())});
                    createDirectoryAndFile(file, combinePath);
                }
                else{
                    if (taskUser.First()?.PhotoPerfil != null)
                    {
                     string fileDeletePath = $"{strlPath}/{taskUser.First()?.PhotoPerfil.Split('/')[1]}";
                     File.Delete($"{fileDeletePath}");
                     taskUser.First().PhotoPerfil = pathfile + $"/{random}";
                     await _dbHandlerUser.UpdateAsyncAll(taskUser.First(), new Users { Idcontrol = int.Parse(this._authJwt.GetUserName()) });
                     createDirectoryAndFile(file,combinePath);
                    }
                }

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
