using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.SecureApi.Tests
{
    public class AuthenticationControlletTest
    {
        HttpClient httpClient;
        public AuthenticationControlletTest()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            httpClient = webAppFactory.CreateDefaultClient();
        }
        [Test]
        public async Task CreateNewToken_ResultOK()
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
            responseString.Should().NotBeEmpty();
        }
    }
}
