
namespace UCABPagaloTodoMS.Core.Services.Firebase
{
    public interface IFirebase
    {
        Task<string> ReadFileContentsAsync(string objectName);
    }
}
