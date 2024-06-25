
using SistemaTickets.Model.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaTickets.Model
{
    public class Users : dbUsername
    { 
          public string? nameUser { get; set; }
          public string? NameSupport { get; set; }
          public string? Surname { get; set; }
          public string? Identification { get; set; }
          public string? PhotoPerfil { get; set; }
          public string? Password { get; set; } 
          public string? Email { get;set; }
          public string? HasConnection { get; set; }
          [Column]
          public int? themeColor { get; set; } 
          public int? RoleCode { get; set; }
          [Column]
          public int? Enabled { get;  set; }

    }
}
