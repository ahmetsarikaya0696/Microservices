using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserVM> GetUser()
        {
            return await _httpClient.GetFromJsonAsync<UserVM>("/api/Users/GetUser");
        }
    }
}
