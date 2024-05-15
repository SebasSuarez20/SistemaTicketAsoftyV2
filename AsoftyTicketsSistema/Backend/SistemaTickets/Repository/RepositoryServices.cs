using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using MySql.Data.MySqlClient;
using SistemaTickets.Data;
using SistemaTickets.Interface.IJwt;
using SistemaTickets.Model;
using SistemaTickets.Services;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SistemaTickets.Repository
{
    public class RepositoryServices<t> : IdbHandler<t> where t : class
    {

        private readonly appDbContext _context;
        private bool disposed = false;
        private readonly IJwt _authJwt;
        public string userName;
        public DateTime today = DateTime.Now;

        public RepositoryServices(appDbContext context,IJwt authJwt)
        {
            _context = context;
            _authJwt = authJwt;
            this.userName = _authJwt.GetUserName();
            
        }

        protected DbSet<t> entitySet => _context.Set<t>();

        public async Task<IEnumerable<t>> GetAllAsyncForAll(Expression<Func<t, bool>> _where = null)
        {
            var Sql = entitySet.AsQueryable();

            if ((!string.IsNullOrEmpty(this.userName) && typeof(t).Name != "consecticketview" 
                && typeof(t).Name != "TicketMapAndSupView"))
            {
                Sql = Sql.Where(s => EF.Property<int>(s, "Idcontrol") == int.Parse(this.userName.ToString()));
            }

            if (typeof(t).Name!= "consecticketview") Sql = Sql.Where(s => EF.Property<bool>(s, "Enabled") == true); // se valida que siempre sea true.

            if (_where != null) Sql = Sql.Where(_where);
            return await Sql.ToListAsync();
        }

        public async Task<IEnumerable<t>> GetCodeAsyncAll(string nameSp)
        {
            return await entitySet.FromSqlRaw($"CALL {nameSp}()").ToListAsync();
        }

        public async Task CreateAllAsync(t entity)
        {
            var property = entity.GetType().GetProperties(); // las propiedades de la entidad.
            var columns = property.Where(columns =>
            columns.Name != "Idcontrol").Select(s => s.Name);

            var Query = $"INSERT INTO {entity.GetType().Name} ({string.Join(", ", columns)}) " +
                $"VALUES ({string.Join(", ", property.
                Where(p => p.Name != "Idcontrol"
                && p.Name != "RegistrationDate").Select(p => $"@{p.Name}"))})";

            var result = property.
                Where(columns => columns.Name != "Idcontrol"
                && columns.Name != "RegistrationDate").Select(p => new
            MySqlConnector.MySqlParameter($"@{p.Name}", p.Name!= "UserName" ?  
            p.GetValue(entity) : int.Parse(this.userName))).ToArray();

            _context.Database.ExecuteSqlRaw(Query, result);
        }
       
        public  async Task UpdateAsyncAll(t entity, object _wh)
        {

            try
            {

                Dictionary<string,object> dictionayWh_ = new Dictionary<string, object>();
                var propery_wh = entity.GetType().GetProperties();

                var _ = _wh.GetType().GetProperties().Where(s => s.GetValue(_wh) != null)
                    .Select(p => new { Name = p.Name, Value = p.GetValue(_wh) });

                //Validamos si hay un campo adicional en el modelo con el Date_Update 
                //Que nos ayudara a validar en que momento se actualizo la informacion.
                var _whereTime = $"{propery_wh.Where(s => s.Name == "Date_Update")
                    .Select(s => $",{s.Name} = 'null'").FirstOrDefault()}";

                foreach (var property in _) { dictionayWh_.Add(property.Name,property.Value); }

                var set_ = $"{string.Join(", ", propery_wh.Where(s => s.Name != "Idcontrol" && s.Name != "UserName" && s.Name != "Date_Update")
               .Select(s => $"{s.Name} = {(s.PropertyType == typeof(string) && !string.IsNullOrEmpty((string?)s.GetValue(entity)) ?
               $"'{s.GetValue(entity)}'" : s.PropertyType == typeof(string) && string.IsNullOrEmpty((string?)s.GetValue(entity)) ? "null" 
               : s.GetValue(entity))}"))} {_whereTime.Replace("null", today.ToString("yyyy-MM-dd H:mm:ss"))}";


                string Sql = $"UPDATE {typeof(t).Name} SET {set_}" +
                $" WHERE {dictionayWh_.Select(s => $"{s.Key} = @{s.Key} ").FirstOrDefault()}" +
                $" AND Enabled = True;";

                var paramsMysql = dictionayWh_.Select(s => new MySqlConnector.MySqlParameter(s.Key, s.Value)).ToArray();

                _context.Database.ExecuteSqlRaw(Sql, paramsMysql);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();

        }

        public void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                _context.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

       
    }
}
