using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySqlX.XDevAPI.Common;
using SistemaTickets.Data;
using SistemaTickets.Model;
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
        private string whereClouse;
        private string sql;

        public repositoryServices(appDbContext context, IHttpContextAccessor _Icontext)
        {
            Icontext = _Icontext;
            _context = context;
            this.userName = authorizeServices.GetUserName(Icontext);
            this.rol = authorizeServices.GetRoleUser(Icontext);
            this.whereClouse = "";
            this.sql = "";
        }

        protected DbSet<T> entitySet => _context.Set<T>();

        public async Task<IEnumerable<T>> GetAllAsyncForAllWithClouse(int isFlag,T? w=null)
        {
            try
            {
                bool isWhere = false;
                string clouse = "";

                if (isFlag % 2 == 0) clouse = $"AND IdControl = {this.userName}";

                if (w != null)
                {
                    var property = w.GetType().GetProperties();
                    var valueField = property.Where(s => s.GetValue(w) != null);
                    Dictionary<string, object?> whereDictionary = new Dictionary<string, object?>();

                    foreach (var c in valueField)
                    {
                        var getValue = valueField.Where(v => v.Name == c.Name).Select(s => s.GetValue(w)).First();
                        whereDictionary.Add(c.Name, getValue);
                    }

                    if (whereDictionary.Count() > 0)
                    {

                    whereClouse = $" WHERE ( " +
                                 $"{string.Join(" AND ", whereDictionary.Select(s => $"{s.Key} = '{s.Value}'"))}" +
                                 $" {clouse} AND Enabled = TRUE )";
                    }
                    else
                    {
                        whereClouse = $" WHERE (Enabled = TRUE {clouse} )";
                    }

                }
                else
                {
                    whereClouse = $" WHERE (Enabled = TRUE {clouse} )";
                }

                sql = $" SELECT * FROM {typeof(T).Name} {whereClouse}";

                return await entitySet.FromSqlRaw(sql).ToListAsync();
            }catch(Exception ex)
            {
                exceptionFolder(ex, "GetAllAsyncForAllWithClouse");
                return null;
            }
        }
        public async Task<IEnumerable<T>> GetAllAsyncForAllNotEnabled()
        {
            try
            {
                sql = $"SELECT * FROM {typeof(T).Name}";
                return await entitySet.FromSqlRaw(sql).ToListAsync();
            }catch(Exception ex)
            {
                exceptionFolder(ex, "GetAllAsyncForAllNotEnabled");
                return null;
            }
        }
        public async Task<IEnumerable<T>> GetAllAsyncForAllWithRol(T w = null)
        {
            try
            {
                bool isWhere = false;
                string clouse = "";

                if ((rol == 2)) clouse = $"AND AssignedTo = {this.userName}";
                else if((rol ==3)) clouse = $"AND Username = {this.userName}";

                if (w != null)
                {
                    var property = w.GetType().GetProperties();
                    var valueField = property.Where(s => s.GetValue(w) != null);
                    Dictionary<string, object?> whereDictionary = new Dictionary<string, object?>();

                    foreach (var c in valueField)
                    {
                        var getValue = valueField.Where(v => v.Name == c.Name).Select(s => s.GetValue(w)).First();
                        whereDictionary.Add(c.Name, getValue);
                    }

                    if (whereDictionary.Count() > 0)
                    {

                        whereClouse = $" WHERE ( " +
                                     $"{string.Join(" AND ", whereDictionary.Select(s => $"{s.Key} = '{s.Value}'"))}" +
                                     $"AND Enabled = TRUE ";
                    }
                    else
                    {
                        whereClouse = $" WHERE (  Enabled = TRUE ";
                    }

                }
                else
                {
                    whereClouse = $" WHERE ( Enabled = TRUE ";
                }

                sql = $" SELECT * FROM {typeof(T).Name} {whereClouse.Trim()} {clouse})".Trim();

                return await entitySet.FromSqlRaw(sql).ToListAsync();
            }
            catch (Exception ex)
            {
                exceptionFolder(ex, "GetAllAsyncForAllWithRol");
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
                     if (typeOf.Contains("DateTime"))
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

                    if (typeOf.Contains("DateTime"))
                    {
                        DateTime date = (DateTime)getValue;
                        queryStrl.Add(c.Name,$"'{date.ToString("yyyy-MM-dd H:mm:ss")}'");
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
                $" AND Enabled = TRUE;";

                var paramsMysql = dictionayWh_.Select(s => new MySqlConnector.MySqlParameter(s.Key, s.Value)).ToArray();

                await _context.Database.ExecuteSqlRawAsync(Sql, paramsMysql);
            }catch(Exception ex)
            {
               exceptionFolder(ex,"UpdateAsync");
            }
        }
        public async Task<int>? UpdateForField(string field, object value)
        {
            string sql = $"UPDATE {typeof(T).Name} SET {field}='{value}' WHERE Idcontrol=@Idcontrol AND Enabled = TRUE;";

            try{

                var sqlParams = new MySqlConnector.MySqlParameter[]{
                new MySqlConnector.MySqlParameter("@idControl",this.userName)
            };

                int response = await _context.Database.ExecuteSqlRawAsync(sql, sqlParams);
                return response;
            }catch(Exception ex)
            {
                exceptionFolder(ex, "UpdateForField");
                return -1;
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
        private void exceptionFolder(Exception ex,string method)
        {
            string folderPath = @"C:/Logs";
            DateTime today = DateTime.Now;

            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

  
            string logFileName = $"{today.ToString("yyyy_MM_dd")}.txt";
            string logFilePath = Path.Combine(folderPath, logFileName);

 
            using (StreamWriter sw = new StreamWriter(logFilePath, append: true))
            {
                sw.WriteLine($"{today} || ${ex.Message} || ${method}");
                sw.WriteLine();
                sw.Close();
            }
        }

    }
}
