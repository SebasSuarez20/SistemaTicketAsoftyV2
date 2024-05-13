using SistemaTickets.Model;

namespace SistemaTickets.Interface.IModel
{
    public interface ILogin
    {
        public Task<object> authLoginSupport(string user, string pswd);
  
    }
}
