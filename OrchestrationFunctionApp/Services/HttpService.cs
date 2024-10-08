using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        public async Task PostRequest(Workflow workflow)
        {
            _httpClient.DefaultRequestHeaders.Add("aeg-sas-key", "value");

            var content = new StringContent(workflow.Body.ToString(), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(workflow.ServiceURL, content);
        }
    }
}
