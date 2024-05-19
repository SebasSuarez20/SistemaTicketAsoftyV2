using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using SistemaTickets.Data;
using SistemaTickets.Interface.IJwt;
using SistemaTickets.Model;
using SistemaTickets.Model.Abstract;
using SistemaTickets.Services;
using SistemaTickets.Services.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SistemaTickets.Repository
{
    public class RepositoryServices<T> : IdbHandler<T> where T : class
    {
        private readonly IHttpContextAccessor Icontext;
        private readonly appDbContext _context;
        private bool disposed = false;
        public string userName;
        public int rol;
        DateTime today;


        public RepositoryServices(appDbContext context, IHttpContextAccessor _Icontext)
        {
            Icontext = _Icontext;
            _context = context;
            this.userName = AuthService.GetUserName(Icontext);
            this.rol = AuthService.GetRoleUser(Icontext);
            this.today = DateTime.Now;
        }

        protected DbSet<T> entitySet => _context.Set<T>();

        public async Task<IEnumerable<T>> GetAllAsyncForAll(Expression<Func<T, bool>> _where = null)
        {
            try
            {
                var Sql = entitySet.AsQueryable();

                if ((!string.IsNullOrEmpty(this.userName) && typeof(T).Name != "consecticketview"
                    && typeof(T).Name != "TicketMapAndSupView"))
                {
                    Sql = Sql.Where(s => EF.Property<int>(s, "Idcontrol") == int.Parse(this.userName.ToString()));
                }

                if (typeof(T).Name != "consecticketview") Sql = Sql.Where(s => EF.Property<bool>(s, "Enabled") == true); // se valida que siempre sea true.

                if (_where != null) Sql = Sql.Where(_where);
                return await Sql.ToListAsync();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<T>> GetCodeAsyncAll(string nameSp)
        {
            try
            {
                return await entitySet.FromSqlRaw($"CALL {nameSp}()").ToListAsync();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
           
        }

        public async Task CreateAllAsync(T entity)
        {
            try
            {
                var property = entity.GetType().GetProperties(); // las propiedades de la entidad.
              
                //Verifica todo lo que no venga en null para tomar esos valores.
                var columns = property.Where(c => Nullable.GetUnderlyingType(c.PropertyType) == null);

                var Query = $"INSERT INTO {entity.GetType().Name} ({string.Join(", ", columns.Select(s=>s.Name))}) " +
                    $"VALUES ({string.Join(", ", columns.Select(c => $"@{c.Name}"))})";

                var result = columns.Select(p => new
                MySqlConnector.MySqlParameter($"@{p.Name}", p.Name != "UserName" ? $"{p.GetValue(entity).ToString()}" : 
                int.Parse(this.userName))).ToArray();

                _context.Database.ExecuteSqlRaw(Query, result);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
       
        public  async Task UpdateAsyncAll(T entity, object _wh)
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

                var set_ = $"{string.Join(", ", propery_wh.Where(s => s.Name != "Idcontrol" && s.Name != "UserName" && 
                s.Name != "Date_Update" && s.Name!="Enabled")
                  .Select(s => $"{s.Name} = {(s.PropertyType != typeof(int) ?  $"{s.GetValue(entity).ToString()}"
                  : s.GetValue(entity))}"))} {_whereTime.Replace("null", today.ToString("yyyy-MM-dd H:mm:ss"))}";


                string Sql = $"UPDATE {typeof(T).Name} SET {set_.Replace("''","null")}" +
                $" WHERE {dictionayWh_.Select(s => $"{s.Key} = @{s.Key} ").FirstOrDefault()}" +
                $" AND Enabled = True;";

                var paramsMysql = dictionayWh_.Select(s => new MySqlConnector.MySqlParameter(s.Key, s.Value)).ToArray();

                _context.Database.ExecuteSqlRaw(Sql, paramsMysql);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task Save() => await _context.SaveChangesAsync();

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
