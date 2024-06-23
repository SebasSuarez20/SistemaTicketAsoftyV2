using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySqlX.XDevAPI.Common;
using SistemaTickets.Data;
using SistemaTickets.Services;
using SistemaTickets.Services.Jwt;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace SistemaTickets.Repository
{
    public class repositoryServices<T> : IdbHandler<T> where T : class
    {
        private readonly IHttpContextAccessor Icontext;
        private readonly appDbContext _context;
        private bool disposed = false;
        protected string userName;
        protected int rol;

        public repositoryServices(appDbContext context, IHttpContextAccessor _Icontext)
        {
            Icontext = _Icontext;
            _context = context;
            this.userName = authorizeServices.GetUserName(Icontext);
            this.rol = authorizeServices.GetRoleUser(Icontext);
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
                exceptionFolder(ex,"GetAllAsync");
                return null;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsyncSp(string nameSp, T e)
        {
            try
            {
                List<object> resultE = new List<object>();
                var property = e.GetType().GetProperties();


                var taskResult = property
                  .Select(s => new { Value = s.GetValue(e) })
              .Where(x => x.Value != null);

                var p = $"CALL {nameSp}({string.Join(",", taskResult.Select(x => $"{x.Value}"))})";

                return await entitySet.FromSqlRaw($"CALL {nameSp}({string.Join(",", taskResult.Select(x => $"{x.Value}"))})").ToListAsync();
            }
            catch(Exception ex){
                exceptionFolder(ex, "GetSpAllAsync");
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
                exceptionFolder(ex,"GetSPAsync");
                return null;
            }
           
        }

        public async Task CreateAllAsync(T entity)
        {
            try
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                var property = entity.GetType().GetProperties(); // las propiedades de la entidad.

                //Verifica todo lo que no venga en null para NO tomar esos valores.
                var columns = property.Where(c => c.GetValue(entity) != null);

                foreach (var c in columns)
                {
                    var getValue = columns.Where(s => s.Name == c.Name).Select(s => s.GetValue(entity)).First() ?? "null";
                    var typeOf = columns.Where(s => s.Name == c.Name).Select(s => s.PropertyType.GenericTypeArguments.Count()!=0 ? s.PropertyType.GenericTypeArguments[0].Name : s.PropertyType.Name).First();
                     if (c.Name.StartsWith("date"))
                    {
                        DateTime date = (DateTime)getValue;
                        result.Add(c.Name, date.ToString("yyyy-MM-dd H:mm:ss"));
                    }
                    else
                    {
                        result.Add(c.Name, typeOf.Contains("String") ? $"{getValue.ToString()}" : getValue);
                    }
                }
                //Le agregamos el username para que siempre en la consulta aparezca.
                result.Add("Username", this.userName);

                var Query = $"INSERT INTO {entity.GetType().Name} ({string.Join(", ", result.Select(s=>s.Key))}) " +
                    $"VALUES ({string.Join(", ", result.Select(s => $"@{s.Key}"))})";

                var parameters=  result.Select(d => new MySqlConnector.MySqlParameter($"@{d.Key}",d.Value)).ToArray();

                await _context.Database.ExecuteSqlRawAsync(Query, parameters);
            }catch(Exception ex)
            {
                exceptionFolder(ex,"InsertAsync");
            }
        }
      
        public  async Task UpdateAsyncAll(T entity, object _wh)
        {

            try
            {
                Dictionary<string,object> dictionayWh_ = new Dictionary<string, object>();
                Dictionary<string,object> queryStrl = new Dictionary<string, object>();
                var property_wh = entity.GetType().GetProperties();

                var ignoreFields = property_wh.
                  Where(s => s.CustomAttributes.Count() >= 1)
                 .Where(s => s.CustomAttributes.First().AttributeType.Name == "KeyAttribute"
                 || s.CustomAttributes.Last().AttributeType.Name == "ColumnAttribute").
                 Select(s => s.Name).ToList();


                 foreach (var c in property_wh)
                {
                    var getValue = property_wh.Where(s => s.Name == c.Name).Select(s => s.GetValue(entity))?.First() ?? "";
                    var typeOf = property_wh.Where(x=>x.Name == c.Name).Select(x=>x.PropertyType.GenericTypeArguments.Count()!=0 ? x.PropertyType.GenericTypeArguments[0].Name : x.PropertyType.Name).First();

                    if (c.Name.StartsWith("date"))
                    {
                        DateTime date = (DateTime)getValue;
                        queryStrl.Add(c.Name, date.ToString("yyyy-MM-dd H:mm:ss"));
                    }
                    else
                    {
                        queryStrl.Add(c.Name, typeOf.Contains("String") ? string.IsNullOrEmpty((string)getValue) ? "''" : $"'{getValue.ToString()}'" : getValue);
                    }
                }

                var _ = _wh.GetType().GetProperties().Where(s => s.GetValue(_wh) != null)
                    .Select(p => new { Name = p.Name, Value = p.GetValue(_wh) });

                foreach (var property in _) { dictionayWh_.Add(property.Name,property.Value); }

                var set_ = $"{string.Join(", ", queryStrl.Where(s => !ignoreFields.Contains(s.Key))
                  .Select(s => $"{s.Key} = {(s.Value)}"))}";

                string Sql = $"UPDATE {typeof(T).Name} SET {set_.Replace("''","null")}" +
                $" WHERE {dictionayWh_.Select(s => $"{s.Key} = @{s.Key}").FirstOrDefault()}" +
                $" AND Enabled = True;";

                var paramsMysql = dictionayWh_.Select(s => new MySqlConnector.MySqlParameter(s.Key, s.Value)).ToArray();

                await _context.Database.ExecuteSqlRawAsync(Sql, paramsMysql);
            }catch(Exception ex)
            {
               exceptionFolder(ex,"UpdateAsync");
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

        public  void exceptionFolder(Exception ex,string action)
        {
            string folderPath = @"C:/Logs";
            DateTime today = DateTime.Now;

            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

  
            string logFileName = $"{today.ToString("yyyy_MM_dd")}.txt";
            string logFilePath = Path.Combine(folderPath, logFileName);

 
            using (StreamWriter sw = new StreamWriter(logFilePath, append: true))
            {
                sw.WriteLine($"{today} || ${ex.Message} || ${action}");
                sw.WriteLine();
                sw.Close();
            }
        }

       
    }
}
