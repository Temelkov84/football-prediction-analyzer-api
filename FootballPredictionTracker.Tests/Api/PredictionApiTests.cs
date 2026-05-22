using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace FootballPredictionTracker.Tests.Api
{
    [TestFixture]
    public class PredictionApiTests
    {
        private WebApplicationFactory<Program>? factory;
        private HttpClient? client;

        [SetUp]
        public void Setup()
        {
            factory = new WebApplicationFactory<Program>();
            client = factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            client?.Dispose();
            factory?.Dispose();
        }

        [Test]
        public async Task GetWeeklyPredictions_ShouldReturnOk()
        {
            // Act
            HttpResponseMessage response =
                await client!.GetAsync("/api/Predictions/weekly");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}