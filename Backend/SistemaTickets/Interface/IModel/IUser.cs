namespace SistemaTickets.Interface.IModel
{
    public interface IUser
    {
        public Task<int> updateThemeDefault(int themeColor);
    }
}
