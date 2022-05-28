using Codecool.SecureApi.Tests.Extensions;
using Codecool.SecureAPI.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace Codecool.SecureApi.Tests
{
    public class UserControllerTests : BaseUserControllerTest
    {
        HttpClient httpClient;
        string _token;
        public UserControllerTests()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            httpClient = webAppFactory.CreateDefaultClient();
        }
        [SetUp]
        public async Task Setup()
        {
            _token =  await CreateToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }
        [Test]
        public async Task GetAllUser_ResultOK()
        {
            //Act
            var response = await httpClient.GetAsync("api/user");
           
            //Assert
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<List<UserViewModel>>(responseString);
            Assert.IsTrue(actual.Count > 0);
        }
        [Test]
        public async Task GetUserByID_ResultOK()
        {
            //Act
            var response = await httpClient.GetAsync("api/user/1");

            //Assert
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<UserViewModel>(responseString);
            Assert.IsTrue(actual.Id==1);
        }
        [Test]
        public async Task AddUser_ResultOK()
        {        
            // Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "api/user");
            var formModel = new Dictionary<string, string>
             {
               { "id", "2" },
               { "name", "New Person" },
               { "role", "Admin" }
             };
            postRequest.Content = new FormUrlEncodedContent(formModel);
            
            // Act
            var response = await httpClient.SendAsync(postRequest);
            
            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<UserViewModel>(responseString);
            var user = GetExpectedUser("Resources.UserControllerTest.user.json");
            actual.Should().BeEquivalentTo(user);
        }

        public async Task<string> CreateToken()
        {
            // Arrange
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "api/auth/login");
            var formModel = new Dictionary<string, string>
             {
               { "name", "New Person" },
               { "password", "123456" }
             };

            postRequest.Content = new FormUrlEncodedContent(formModel);
            // Act
            var response = await httpClient.SendAsync(postRequest);
            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
       
    }
}