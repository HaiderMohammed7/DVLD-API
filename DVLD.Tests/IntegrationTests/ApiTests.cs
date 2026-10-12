using DVLD.Application.Features.Tests.DTOs;
using DVLD.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Tests.IntegrationTests
{
    public class ApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;
        private const string TestJwtSecret = "this-is-a-test-secret-key-for-integration-tests-123456";
        
        public ApiTests(CustomWebApplicationFactory factory)
        {
            Environment.SetEnvironmentVariable("JWT_SECRET", TestJwtSecret);
            _client = factory.CreateClient();
            _factory = factory;
        }

        private string CreateTestToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestJwtSecret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]{ new Claim(ClaimTypes.NameIdentifier, CustomWebApplicationFactory.TestAuthUserId.ToString())};

            var token = new JwtSecurityToken(
                issuer: "AuthService",
                audience: "DVLD",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Fact]
        public async Task GetUsers_WithoutAuthentication_ShouldReturnUnauthorized()
        {
            var response = await _client.GetAsync("/api/users");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetUsers_WithValidToken_ShouldNotReturnUnauthorized()
        {
            var token = CreateTestToken();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/users");

            Assert.NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetUsers_WithAdminToken_ShouldReturnOk()
        {
            var token = CreateTestToken();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/users");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ScheduleTest_FirstVisionTest_ShouldCreateAppointment()
        {
            var token = CreateTestToken();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var dto = new ScheduleTestDto
            {
                LocalDrivingLicenseApplicationID = _factory.TestLocalDrivingLicenseApplicationId,

                TestTypeID = _factory.TestVisionTestTypeId,

                AppointmentDate = DateTime.UtcNow.AddDays(1)
            };

            var response = await _client.PostAsJsonAsync("api/test-appointments/schedule", dto);

            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode,$"Status: {response.StatusCode}\nResponse: {responseBody}");

            using var scope = _factory.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<DVLDDbContext>();

            var appointment = await db.TestAppointments.FirstOrDefaultAsync(x =>
                    x.LocalDrivingLicenseApplicationID ==_factory.TestLocalDrivingLicenseApplicationId
                    &&
                    x.TestTypeID == _factory.TestVisionTestTypeId);

            Assert.NotNull(appointment);
            Assert.False(appointment.IsLocked);
            Assert.Equal(10, appointment.PaidFees);
        }
    }
}