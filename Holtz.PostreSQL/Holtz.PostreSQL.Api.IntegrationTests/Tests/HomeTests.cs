﻿using Holtz.PostreSQL.Api.IntegrationTests.Setup;
using System.Net;

namespace Holtz.PostreSQL.Api.IntegrationTests.Tests
{
    public class HomeTests : IntegrationTest
    {
        public HomeTests(ApiWebApplicationFactory fixture) : base(fixture) { }

        [Fact]
        public async Task GET_Home_ShouldBeOk()
        {
            // Act
            var response = await _client.GetAsync("/api/home");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
