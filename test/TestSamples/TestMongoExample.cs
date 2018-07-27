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
       public void TestMongo()
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

       [Fact]
       [Trait ("Category", "mongo")]
       public void TestInsert1Update1Delete1()
       {
        //Given
        MongoExampleController mongoExampleController = new MongoExampleController();

        //When
        mongoExampleController.insertSchool("school1");
        
        //Then
        mongoExampleController.CountBySchoolName("school1");
        Assert.Equal(1, mongoExampleController.Data);
        

        //When
        mongoExampleController.updateOneSchoolName("school1", "school2");
        
        //Then
        mongoExampleController.CountBySchoolName("school1");
        Assert.Equal(0, mongoExampleController.Data);

        mongoExampleController.CountBySchoolName("school2");
        Assert.Equal(1, mongoExampleController.Data);


        //When
        mongoExampleController.deleteOneSchoolByName("school2");

        //Then
        mongoExampleController.CountBySchoolName("school2");
        Assert.Equal(0, mongoExampleController.Data);
       }

       [Fact]
       [Trait ("Category", "mongo")]
       public void TestInsertManyUpdateManyDeleteMany()
       {
            //Given
            MongoExampleController mongoExampleController = new MongoExampleController();

            //When
            mongoExampleController.insertSchool("school1");
            mongoExampleController.insertSchool("school1");
            mongoExampleController.insertSchool("school1");
            
            //Then
            mongoExampleController.CountBySchoolName("school1");
            Assert.Equal(3, mongoExampleController.Data);
            

            //When
            mongoExampleController.updateManySchoolName("school1", "school2");
            
            //Then
            mongoExampleController.CountBySchoolName("school1");
            Assert.Equal(0, mongoExampleController.Data);

            mongoExampleController.CountBySchoolName("school2");
            Assert.Equal(3, mongoExampleController.Data);


            //When
            mongoExampleController.DeleteManySchoolByName("school2");

            //Then
            mongoExampleController.CountBySchoolName("school2");
            Assert.Equal(0, mongoExampleController.Data);
       }

    

    }
}