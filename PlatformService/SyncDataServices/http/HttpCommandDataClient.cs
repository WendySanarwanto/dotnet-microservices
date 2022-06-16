using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http {
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpCommandDataClient> _logger;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration, 
            ILogger<HttpCommandDataClient> logger)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task SendPlatformToCommand(PlatformReadDto platformReadDto)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platformReadDto),
                Encoding.UTF8,
                "application/json"
            );

            var response = 
                await _httpClient.PostAsync($"{_configuration["CommandService"]}", httpContent);

            if (response.IsSuccessStatusCode) {
                _logger.LogInformation("---> Sync POST to CommandService was OK!");
            } else {
                _logger.LogInformation("---> Sync POST to CommandService was Failing.");
            }

        }
    }
}