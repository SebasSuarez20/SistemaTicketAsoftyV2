namespace SistemaTickets.Interface.IModel
{
    public interface ISupportagent
    {
        public Task<object> authLoginSupport(string user, string pswd);

       
    }
}
