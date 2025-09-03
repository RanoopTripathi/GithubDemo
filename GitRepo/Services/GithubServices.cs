using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GitRepo.Services
{
    public class GithubServices
    {
        private readonly HttpClient _httpClient;
        private readonly string _token = ""; // ⚠️ Store in appsettings or secret manager
        private readonly string _baseUrl = "https://api.github.com/repos/RanoopTripathi/GithubDemo";

        public GithubServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MyApp"); // GitHub requires User-Agent
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        public async Task<string> ListBranches(string owner, string repo)
        {
            var url = string.Format(_baseUrl + "/branches", owner, repo);
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> CreateBranch(string owner, string repo, string newBranch, string fromBranch)
        {
            // Step 1: Get SHA of source branch
            var refUrl = string.Format(_baseUrl, owner, repo) + $"/git/ref/heads/{fromBranch}";
            var refResponse = await _httpClient.GetStringAsync(refUrl);

            using var jsonDoc = JsonDocument.Parse(refResponse);
            var sha = jsonDoc.RootElement.GetProperty("object").GetProperty("sha").GetString();

            // Step 2: Create new branch
            var createUrl = string.Format(_baseUrl, owner, repo) + "/git/refs";
            var body = new
            {
                @ref = $"refs/heads/{newBranch}",
                sha = sha
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(createUrl, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

    }

}

