using SistemaTickets.Model.Abstract;
using System.ComponentModel.DataAnnotations;

namespace SistemaTickets.Model
{
    public class supportagents
    {
         [Key]
          public int Idcontrol { get; set; }
          public string userName { get; set; }
          public string NameSupport { get; set; }
          public string? Surname { get; set; }
          public string? Identification { get; set; }
          public string? PhotoPerfil { get; set; }
          public string? Password { get; set; }
          public string? Email { get;set; }
          public int? RoleCode { get; set; }
          public bool? Enabled { get; set; }

    }
}
