using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrchestrationFunctionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpService> _logger;

        public HttpService(IHttpClientFactory httpClientFactory, ILogger<HttpService> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }

        public async Task GetRequest(Workflow workflow)
        {
            var response = await _httpClient.GetAsync(workflow.ServiceURL);

            if (response.IsSuccessStatusCode)
            {
                _ = await response.Content.ReadAsStringAsync();                
            }
            else
            {
                _logger.LogError($"Service {workflow.ServiceURL} failed");
            }
        }

        public Task PostRequest(Workflow workflow)
        {
            return Task.CompletedTask;
        }
    }
}
