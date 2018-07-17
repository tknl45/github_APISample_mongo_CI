using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;


/// <summary>
/// 需要在 .csproj設定ProjectReference
/// 假設有一個..\APISample\APISample.csproj
/// </summary>
namespace MyFirstUnitTests.TestSamples
{
    public class TestRestApi
    {

        private readonly TestServer _server;
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;


        public TestRestApi(ITestOutputHelper output)
        {
            var builder = new WebHostBuilder()
               .UseStartup<APISample.Startup>()
               .UseApplicationInsights();;

            _server = new TestServer(builder);

            _client = _server.CreateClient();

            _output = output;
        }


        //Testing HTTP Posts

        [Fact]
        [Trait ("Category", "RestAPI")]
        public async Task Test_demo_data()
        {    
            string message = "135456";

            var formData = new Dictionary<string, string>
            {
                {"FirstName", "Sarah"},
                {"LastName", "Smith"}     
            };

            string json = JsonConvert.SerializeObject(formData, Formatting.Indented);

            //CONTENT-TYPE header
            var httpContent = new StringContent(json,
                                    Encoding.UTF8, 
                                    "application/json");

            HttpRequestMessage postRequest = new HttpRequestMessage(HttpMethod.Post, $"/api/Demo/demo_data?message={message}")
            {
                Content = httpContent
            };
           
           //ACCEPT header
            _client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            

            var response = await _client.SendAsync(postRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            _output.WriteLine(responseString);
            response.EnsureSuccessStatusCode();

            
            Assert.Contains(message, responseString);
            // Additional asserts could go here 
        }




        [Theory]
        [InlineData ("test1")]
        [InlineData ("five2")]
        [InlineData ("xxxx3")]
        [Trait ("Category", "RestAPI")]
        public async Task Test_demo_message (string message) {
            HttpResponseMessage response = await _client.GetAsync($"/api/Demo/demo_message?message={message}");

            response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            _output.WriteLine(responseString);
            Assert.Contains(message, responseString);
        }

        /// <summary>
        /// 指定發生錯誤類型
        /// </summary>
        [Fact]
        [Trait ("Category", "RestAPI")]        
        public async Task Test_demo_error () {
            var message = "111222333";
          
            HttpResponseMessage response = await _client.GetAsync($"/api/Demo/demo_error?message={message}");

            response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            _output.WriteLine(responseString);

             Assert.Contains("ERROR", responseString);
        }

        /// <summary>
        /// 不要null
        /// </summary>
        [Fact]
        [Trait ("Category", "RestAPI")]
        public async Task Test_demo_list_error_log () {
           
           //需要加入appsettings.json 給APISample.dll讀取mongo url
           HttpResponseMessage response = await _client.GetAsync($"/api/Demo/demo_list_error_log");

            response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            _output.WriteLine(responseString);

            Assert.Contains("xx", responseString);
        }

        /// <summary>
        /// 不要null
        /// </summary>
        [Fact]
        [Trait ("Category", "API")]
        public void Test_Redis () {
           var key = "data";
           var value = "kkk";
           //需要加入appsettings.json 給APISample.dll讀取mongo url
           _demo.RedisSet(key,value);
           _demo.RedisGet(key);

            Assert.True(value.Equals(_demo.Data));
        }
    }
}