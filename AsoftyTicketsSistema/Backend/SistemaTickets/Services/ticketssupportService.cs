using Newtonsoft.Json;
using SistemaTickets.Interface.IJwt;
using SistemaTickets.Interface.IModel;
using SistemaTickets.Model;
using SistemaTickets.Model.View;
using SistemaTickets.Services.Jwt;
using System.Dynamic;

namespace SistemaTickets.Services
{
    public class ticketssupportService : IticketsSupport
    {
        private readonly IHttpContextAccessor Icontext;
        private readonly IdbHandler<ticketssupport> _dbHandlerTickets;
        private readonly IdbHandler<consecticketview> _dbHandlerconsecTicket;
        private readonly IdbHandler<TicketMapAndSupView> _dbHandlerTicketMapAndSupView;
        private IConfiguration _config;
        private int Rol;
        private int User;


        public ticketssupportService(IdbHandler<ticketssupport> dbHandlerTickets,
            IConfiguration config,IdbHandler<consecticketview> dbHandlerconsecTicket,
            IdbHandler<TicketMapAndSupView> dbHandlerTicketMapAndSupView, IHttpContextAccessor _Icontext) {
            Icontext = _Icontext;
             _dbHandlerTickets = dbHandlerTickets;
             _config = config;
            _dbHandlerconsecTicket = dbHandlerconsecTicket;
            _dbHandlerTicketMapAndSupView = dbHandlerTicketMapAndSupView;
            Rol = AuthService.GetRoleUser(Icontext);
            User = int.Parse(AuthService.GetUserName(Icontext));
        }

        public async Task<object> createTickets(CreateTicketModel model)
        {

            dynamic response = new ExpandoObject();
            try
            {
                ticketssupport modelHeader = new ticketssupport();

                dynamic jsonObject = JsonConvert.DeserializeObject(model.header);

                var lastConsecutive = await _dbHandlerconsecTicket.GetAllAsyncForAll();
                var filesSupport = $"{this._config["pathFile:pathCompany"].Replace("\\", "/")}/Company{User}";
          
                modelHeader.Consecutive = (int)(lastConsecutive.Count() != 0 ? lastConsecutive.First().consecutive : 0)+1;
                modelHeader.Aerea = jsonObject.aerea;
                modelHeader.Title = jsonObject.title;
                modelHeader.Description = jsonObject.description;
                modelHeader.Priority = jsonObject.priority;
                modelHeader.PhotoDescription = jsonObject.photoDescription;
                modelHeader.Status =Status.Open;

                await _dbHandlerTickets.CreateAllAsync(modelHeader);
                var DirectoryForDate = $"{filesSupport}/Observations{modelHeader.Consecutive}";

                if (model.files!=null)
                {
                   
                    if (!Directory.Exists(filesSupport)) Directory.CreateDirectory(filesSupport);

                    if (!Directory.Exists(DirectoryForDate)) Directory.
                            CreateDirectory(DirectoryForDate);

                    foreach (var file in model.files)
                    {

                        var pathCombine = Path.Combine(DirectoryForDate, file.FileName);
                        using (var stream = new FileStream(pathCombine, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            stream.Close();
                        }
                    }
                }
                response.status = 200;
                return response;
            }
            catch (Exception ex)
            {
                response.status = 400;
                response.messageError = ex.Message;
                return response;
            }
        }

        public async Task<object> getAllMapAndSup()
        {
            try
            {
                var respontForRol = await _dbHandlerTicketMapAndSupView.
               GetAllAsyncForAll((Rol == 2) ? s => s.AssignedTo == User :
               (Rol == 3) ? s => s.UserName == User : null);

                return respontForRol.Select(s => new
                {
                    No = s.Consecutive,
                    Nombre = s.Title,
                    Descripcion = s.Subject,
                    Estado = s.Status,
                    Asignacion = (Rol == 1) ? s.AssignedTo ?? 0 : -1,
                    Username = s.UserName
                }).ToList();
            }catch(Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
