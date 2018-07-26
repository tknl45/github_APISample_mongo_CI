using System.Collections.Generic;
using System.Threading;
using APISample.Controllers;
using Xunit;

namespace test.TestSamples
{
    public class TestMongoExample
    {


       [Fact]
       [Trait ("Category", "mongo")]
       public void TestMongoCreate()
       {
            //Given
            MongoExampleController mongoExampleController = new MongoExampleController();
            
            //When
            mongoExampleController.drop();
            mongoExampleController.create();
       
            mongoExampleController.list();
            var list = (List<School>)mongoExampleController.Data;
            
            //Then
            Assert.Equal(138, list.Count);



            //When   
            mongoExampleController.list("1","1");
            list = (List<School>)mongoExampleController.Data;
        
            //Then
            Assert.Equal(19, list.Count);



            //When 
            mongoExampleController.pagein("文山");
            list = (List<School>)mongoExampleController.Data;
            var msg = mongoExampleController.Message;

            //Then
            Assert.Equal(10, list.Count);
            Assert.Contains("18", msg);

            //When 
            mongoExampleController.countByRegion();
            var list2 = (List<Dictionary<string,string>>)mongoExampleController.Data;

            //Then
            Assert.Equal(12, list2.Count);
            Assert.Equal("19", list2[0]["total"]);


       }

    

    }
}