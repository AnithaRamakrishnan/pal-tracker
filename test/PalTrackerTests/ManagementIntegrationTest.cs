﻿using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Xunit;

namespace PalTrackerTests
{
    [Collection("Integration")]
    public class ManagementIntegrationTest
    {
        private HttpClient _testClient;

        private async void Init()
        {
            _testClient = await IntegrationTestServer.GetHttpClient();
        }
        public ManagementIntegrationTest()
        {
            Environment.SetEnvironmentVariable("MYSQL__CLIENT__CONNECTIONSTRING", DbTestSupport.TestDbConnectionString);
            DbTestSupport.ExecuteSql("TRUNCATE TABLE time_entries");
            Init();
        }

        [Fact]
        public void HasHealth()
        {
            var response = _testClient.GetAsync("/actuator/health").Result;
            var responseBody = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("UP", responseBody["status"]);
            Assert.Equal("UP", responseBody["diskSpace"]["status"]);
            Assert.Equal("UP", responseBody["timeEntry"]["status"]);
        }

        [Fact]
        public void HasInfo()
        {
            var response = _testClient.GetAsync("/actuator/info").Result;
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
