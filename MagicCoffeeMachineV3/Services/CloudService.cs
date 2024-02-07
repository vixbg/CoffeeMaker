namespace MagicCoffeeMachineV3.Services
{
    using MagicCoffeeMachineV3.Interfaces;
    using Newtonsoft.Json;
    using System.Text;

    public class CloudService : ICloudService
    {
        private readonly HttpClient HttpClient;
        private readonly string CloudApiBaseUrl;

        public CloudService(HttpClient httpClient, IConfiguration configuration)
        {
            HttpClient = httpClient;
            CloudApiBaseUrl = configuration.GetValue<string>("CloudApiBaseUrl")!;
        }

        public async Task NotifyMaintenanceNeededAsync()
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(new { message = "Machine is out of coffee beans" }),
                Encoding.UTF8,
                "application/json"
            );

            var response = await HttpClient.PostAsync(
                $"{CloudApiBaseUrl}/api/notifications/maintenance",
                content);

            response.EnsureSuccessStatusCode();
        }

        public async Task NotifyMilkRefillNeededAsync()
        {
            var response = await HttpClient.PostAsync(
                $"{CloudApiBaseUrl}/api/notifications/milk-refill",
                new StringContent(""));

            response.EnsureSuccessStatusCode();
        }
    }

}
