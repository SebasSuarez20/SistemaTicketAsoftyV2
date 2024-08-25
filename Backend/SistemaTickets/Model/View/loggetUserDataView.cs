using SistemaTickets.Model.Abstract;

namespace SistemaTickets.Model.View
{
    public class loggetUserDataView 
    {
        public int? Idcontrol { get; set; }
        public string? NameUser { get; set; }
        public int? RoleCode { get; set; }
        public bool? ThemeColor { get; set; }
        public string? NameSupport {  get; set; }
        public string? Surname {  get; set; }
        public string? PhotoPerfil { get; set; }
        public string? Password { get; set; }
        public bool? Enabled { get; set; }  
    }
}
