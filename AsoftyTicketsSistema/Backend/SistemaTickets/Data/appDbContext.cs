using Microsoft.EntityFrameworkCore;
using SistemaTickets.Model;
using SistemaTickets.Model.View;

namespace SistemaTickets.Data
{
    public class appDbContext : DbContext
    {

        public appDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Users> Users { get; set; }
        public DbSet<ticketssupport> ticketssupport { get; set; }
        public DbSet<consecticketview> consecticketview { get; set; }
        public DbSet<TicketMapAndSupView> TicketMapAndSupView { get; set; }
        public DbSet<codeGeneric> CodeGeneric { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<codeGeneric>().HasNoKey();
        }
    }
}
