using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using src.Models;
using Xunit;

namespace reputationmanagement.web.IntegrationTests;

public class UserControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _client;

    public UserControllerIntegrationTests(TestingWebAppFactory<Program> factory)
        => _client = factory.CreateClient();

    protected async Task AuthenticateAsync()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
    }

    private async Task<string> GetJwtAsync()
    {
        var response = await _client.PostAsJsonAsync("api/auth/sign_in", new UserLoginModel
        {
            Email = "test@example.com",
            Password = "T35T1NG.m3"
        });
        // var registrationResponse = await response.Content.ReadAsAsync();
        // return registrationResponse.Token;
        Console.WriteLine(response);
        return response.Content.ToString();

    }

    [Fact]
    public async Task Greet_WhenCalled_ReturnMessage()
    {
        //Arrange
        await AuthenticateAsync();

        //Act
        var response = await _client.GetAsync("api/postedreviews");

        //Assert
        response.EnsureSuccessStatusCode();
    }


}