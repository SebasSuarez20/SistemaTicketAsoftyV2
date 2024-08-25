using SistemaTickets.Model.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaTickets.Model
{
    public class informationUser : dbUsername
    {
        public string? NameSupport { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public int? gender { get; set; }
        public int? typeIdentification { get; set; }
        public string? identification { get; set; }
        public string? bloodType { get; set; }
        public int? country { get; set; }
        public int? city { get; set; }
        public string? address { get; set; }
        public int? phone { get; set; }
        public DateTime? birthDate { get; set; }
        public int? emergencyContact { get; set; }
        public string? parentage { get; set; }
        public string? firstName { get; set; }
        public string? firstSurname { get; set; }
        public int? department { get; set; }
        public int? enabled { get; set; }
    }
}
