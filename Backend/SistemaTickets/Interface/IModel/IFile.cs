namespace SistemaTickets.Interface.IModel
{
    public interface IFile
    {
        public Task<object> createFile(IFormFile file);
    }
}
