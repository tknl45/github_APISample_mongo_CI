using Xunit;
using Xunit.Abstractions;
using System;

/// <summary>
/// 需要在 .csproj設定ProjectReference
/// 假設有一個..\APISample\APISample.csproj
/// </summary>
using APISample.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyFirstUnitTests.TestSamples {
    /// <summary>
    /// 需要在 .csproj設定ProjectReference
    /// 假設有一個..\APISample\APISample.csproj
    /// </summary>
    public class TestOtherProject {
        private readonly DemoController _demo;
        private readonly ITestOutputHelper _output;

        public TestOtherProject (ITestOutputHelper output) {
            _demo = new DemoController ();
            _output = output;
        }

        [Theory]
        [InlineData ("test1")]
        [InlineData ("five2")]
        [InlineData ("xxxx3")]
        [Trait ("Category", "API")]
        public void Test_demo_message (string message) {
            _demo.demo_message (message);
            _output.WriteLine(_demo.Message);
            Assert.True (_demo.Message.IndexOf (message) > -1);
        }

        [Fact]
        [Trait ("Category", "API")]
        public void Test_demo_data () {
            
            // my pretend dataset
            List<string> fields = new List<string>();
            // my 'columns'
            fields.Add("this_thing");
            fields.Add("that_thing");
            fields.Add("the_other");


            _demo.demo_data("hi",fields);
            Assert.True (_demo.Message == "hi");
        }



        /// <summary>
        /// 指定發生錯誤類型
        /// </summary>
        [Fact]
        [Trait ("Category", "API")]
        public void Test_demo_error () {
            //act
            Action act = () => _demo.demo_error("hi");
            Assert.Throws<ArgumentException>(act);
        }

        /// <summary>
        /// 不要null
        /// </summary>
        [Fact]
        [Trait ("Category", "API")]
        public void Test_demo_list_error_log () {
           
           //需要加入appsettings.json 給APISample.dll讀取mongo url
           _demo.demo_list_error_log();

            Assert.NotNull(_demo.Data);
        }


        /// <summary>
        /// 不要null
        /// </summary>
        [Fact(Skip = "不要驗redis")]
        [Trait ("Category", "API")]
        public void Test_Redis () {
           var key = "data";
           var value = "kkk";
           //需要加入appsettings.json 給APISample.dll讀取redis url
           _demo.RedisSet(key,value);
           _demo.RedisGet(key);

           string json = JsonConvert.SerializeObject(_demo);
            _output.WriteLine(json);
            Assert.True(value.Equals(_demo.Data));
            
        }
    }
}