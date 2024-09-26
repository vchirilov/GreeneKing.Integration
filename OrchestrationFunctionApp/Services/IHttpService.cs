using OrchestrationFunctionApp.Models;
using System;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Services
{
    public interface IHttpService
    {
        Task GetRequest(Workflow workflow);
        Task PostRequest(Workflow workflow);
    }
}