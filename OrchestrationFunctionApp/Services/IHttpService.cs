using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Services
{
    public interface IHttpService
    {
        Task SendGetRequest();
        Task SendPostRequest();
    }
}