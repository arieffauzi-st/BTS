using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BTS.Models;

namespace BTS.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private string _token;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://94.74.86.174:8080/api/");
        }

        public async Task<(bool Success, string Token)> Login(string username, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("login", new { username, password });
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (!string.IsNullOrEmpty(result?.Token))
                {
                    _token = result.Token;
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                    return (true, result.Token);
                }
            }
            return (false, null);
        }

        public async Task<bool> Register(string email, string username, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("register", new { email, username, password });
            return response.IsSuccessStatusCode;
        }

        // 3. API untuk membuat checklist
        public async Task<ChecklistModel> CreateChecklist(string name)
        {
            EnsureLoggedIn();
            var response = await _httpClient.PostAsJsonAsync("checklist", new { name });
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ChecklistModel>();
            }
            throw new HttpRequestException("Failed to create checklist");
        }

        // 4. API untuk menghapus checklist
        public async Task<bool> DeleteChecklist(int checklistId)
        {
            EnsureLoggedIn();
            var response = await _httpClient.DeleteAsync($"checklist/{checklistId}");
            return response.IsSuccessStatusCode;
        }

        // 5. API untuk menampilkan checklist-checklist yang sudah dibuat
        public async Task<ChecklistModel[]> GetAllChecklists()
        {
            EnsureLoggedIn();
            var response = await _httpClient.GetAsync("checklist");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ChecklistModel[]>();
            }
            throw new HttpRequestException("Failed to get checklists");
        }

        // 6. API Detail Checklist
        public async Task<ChecklistModel> GetChecklistDetail(int checklistId)
        {
            EnsureLoggedIn();
            var response = await _httpClient.GetAsync($"checklist/{checklistId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ChecklistModel>();
            }
            throw new HttpRequestException("Failed to get checklist detail");
        }

        // 7. API untuk membuat item-item to-do di dalam checklist
        public async Task<ChecklistItemModel> AddChecklistItem(int checklistId, string itemName)
        {
            EnsureLoggedIn();
            var response = await _httpClient.PostAsJsonAsync($"checklist/{checklistId}/item", new { itemName });
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ChecklistItemModel>();
            }
            throw new HttpRequestException("Failed to add checklist item");
        }

        // 8. API detail item
        public async Task<ChecklistItemModel> GetChecklistItemDetail(int checklistId, int itemId)
        {
            EnsureLoggedIn();
            var response = await _httpClient.GetAsync($"checklist/{checklistId}/item/{itemId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ChecklistItemModel>();
            }
            throw new HttpRequestException("Failed to get checklist item detail");
        }

        // 9. API untuk mengubah item-item di dalam checklist
        public async Task<ChecklistItemModel> UpdateChecklistItem(int checklistId, int itemId, string newName)
        {
            EnsureLoggedIn();
            var response = await _httpClient.PutAsJsonAsync($"checklist/{checklistId}/item/{itemId}", new { itemName = newName });
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ChecklistItemModel>();
            }
            throw new HttpRequestException("Failed to update checklist item");
        }

        // 10. API untuk mengubah status dari item di dalam checklist
        public async Task<ChecklistItemModel> UpdateItemStatus(int checklistId, int itemId)
        {
            EnsureLoggedIn();
            var response = await _httpClient.PutAsync($"checklist/{checklistId}/item/{itemId}/status", null);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ChecklistItemModel>();
            }
            throw new HttpRequestException("Failed to update item status");
        }

        // 11. API untuk menghapus item dari checklist
        public async Task<bool> DeleteChecklistItem(int checklistId, int itemId)
        {
            EnsureLoggedIn();
            var response = await _httpClient.DeleteAsync($"checklist/{checklistId}/item/{itemId}");
            return response.IsSuccessStatusCode;
        }

        private void EnsureLoggedIn()
        {
            if (string.IsNullOrEmpty(_token))
            {
                throw new UnauthorizedAccessException("You must login first.");
            }
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }
}
