using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Services
{
    public class HttpService : IHttpService
    {
        public Task SendGetRequest()
        {
            return Task.CompletedTask;
        }

        public Task SendPostRequest()
        {
            return Task.CompletedTask;
        }
    }
}
