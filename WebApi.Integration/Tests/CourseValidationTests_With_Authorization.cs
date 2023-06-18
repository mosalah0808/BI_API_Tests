using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApi.Integration.Services;
using WebApi.Models;
using Xunit;

namespace WebApi.Integration.Tests
{
    public class CourseValidationTests_With_Authorization: IClassFixture<TestFixture>
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private readonly string _cookie;

        private readonly CourseService _courseService;
        public CourseValidationTests_With_Authorization(TestFixture testFixture)
        {
            _httpClient = new HttpClient();
            var configuration = testFixture.Configuration;
            _baseUri = configuration["BaseUri"];
            _cookie = testFixture.AuthCookie;
            _courseService = new CourseService();
        }
        
        [Fact]
        public async Task IfPriceIsZero_PostCourseShouldReturnError()
        {
            //Arrange 
            var courseModel = new CourseModel
            {
                Name = Guid.NewGuid().ToString(),
                Price = 0
            };

            //Act
            _httpClient.DefaultRequestHeaders.Add("cookie", _cookie); //RPRY лучше вызывать метод сервиса, где куки подкладывается уже под капотом. Здесь и ниже
            var response = await _httpClient.PostAsJsonAsync($"{_baseUri}/course", courseModel);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var responseMessage = await response.Content.ReadAsStringAsync();
            Assert.Equal(Errors.Поле_Price_должно_быть_больше_нуля, responseMessage);
        }

        [Fact]
        public async Task IfNameIsEmpty_PostCourseShouldReturnError() //RPRY: можно сделать параметризованный тест, подставляя не только "" но и NULL
        {
            //Arrange 
            var courseModel = new CourseModel
            {
                Name = "",
                Price = (new Random()).Next(int.MaxValue)
            };

            //Act
            _httpClient.DefaultRequestHeaders.Add("cookie", _cookie);
            var response = await _httpClient.PostAsJsonAsync($"{_baseUri}/course", courseModel); 

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var responseMessage = await response.Content.ReadAsStringAsync();
            Assert.Equal(Errors.Поле_Name_не_должно_быть_пустым, responseMessage);
        }

        [Fact]
        public async Task IfInitialParametersAreSetCorrectly_PostCourseShouldCreateLessonSuccessfully()
        {
            //Arrange
            var initialCourseModel = new AddCourseModel
            {
                Name = "course_name",
                Price = (new Random()).Next(int.MaxValue)
            };

            //Act
            var addCourseResponse = await _courseService.AddCourseInternalAsync(initialCourseModel, _cookie);

            //Assert
            Assert.Equal(HttpStatusCode.OK, addCourseResponse.StatusCode);
            var courseId = int.Parse(await addCourseResponse.Content.ReadAsStringAsync());
            var course = await _courseService.GetCourseAsync(courseId);
            Assert.Equal(initialCourseModel.Name, course.Name);
            Assert.Equal(initialCourseModel.Price, course.Price);
        }

        [Fact]
        public async Task IfCourseIsEdited_NewFieldValuesShouldBeSavedSuccessfully()
        {
            //Arrange
            var newCourseModel = new AddCourseModel
            {
                Name = Guid.NewGuid().ToString(),
                Price = (new Random()).Next(int.MaxValue)
            };

           var courseId = await _courseService.AddCourseAsync(newCourseModel, _cookie);
            var editCourseModel = new AddCourseModel
            {
                Name = Guid.NewGuid().ToString(),
                Price = (new Random()).Next(int.MaxValue)
            }; 
            //Act
            var editCourseResponse = await _courseService.EditCourseInternalAsync(courseId, editCourseModel, _cookie);

            //Assert
            Assert.Equal(HttpStatusCode.OK, editCourseResponse.StatusCode);
            var course = await _courseService.GetCourseAsync(courseId);
            Assert.Equal(editCourseModel.Name, course.Name);
            Assert.Equal(editCourseModel.Price, course.Price);
        }

        [Fact]
        public async Task IfCourseIsDeleted_Field_Deleted_ChangedOnTrue_Succesfully()
        {
            //Arrange
            var newCourseModel = new AddCourseModel
            {
                Name = Guid.NewGuid().ToString(),
                Price = (new Random()).Next(int.MaxValue)
            };

            var courseId = await _courseService.AddCourseAsync(newCourseModel, _cookie);
           
            //Act
            var deletedCourseResponse = await _courseService.DeleteCourseInternalAsync(courseId, _cookie);

            //Assert
            Assert.Equal(HttpStatusCode.OK, deletedCourseResponse.StatusCode);
            var course = await _courseService.GetCourseAsyncDel(courseId);
            Assert.True(course.Deleted);
            
        }

        [Fact]
        public async Task CourseList_ShouldBe_OrderedById_Correctly()
        {
            //Arrange
            var newCourseModel1 = new AddCourseModel
            {
                Name = Guid.NewGuid().ToString(),
                Price = (new Random()).Next(int.MaxValue)
            };
            var newCourseModel2 = new AddCourseModel
            {
                Name = Guid.NewGuid().ToString(),
                Price = (new Random()).Next(int.MaxValue)
            };
            int page = 1;
            int itemsPerPage = 50;

            var courseId1 = await _courseService.AddCourseAsync(newCourseModel1, _cookie); //RPRY Добавлять новые сущности можно если их нет  
            var courseId2 = await _courseService.AddCourseAsync(newCourseModel2, _cookie); //RPRY в Act должен быть один вызов. Здесь и ниже

            //Act 
            var response = await _courseService.GetCourseListInternalAsync(page, itemsPerPage, _cookie);
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(courseId2- courseId1==1);
            var course = JsonConvert.DeserializeObject<List<CourseModel>>(await response.Content.ReadAsStringAsync());
            Assert.True(course[course.Count-1].Name.Equals(newCourseModel2.Name)); //RPRY не факт что выведутся те сущности, которые были добавлены. На самом деле выводятся те, которые идут первыми в БД 
            Assert.True(course[course.Count - 2].Name.Equals(newCourseModel1.Name));
        }

        [Fact]
        public async Task Paging_InCourseList_OnFirstPage_And_SecondPage_ShouldBe_Worked_Correctly()
        {
            //Arrange
           
            int page = 2;
            int itemsPerPage = 4;

            //Act 
            var response = await _courseService.GetCourseListInternalAsync(page, itemsPerPage, _cookie);
            var response2 = await _courseService.GetCourseListInternalAsync(page+1, itemsPerPage, _cookie);

            //Assert
            var course_page1_last = await _courseService.GetCourseAsync(page*itemsPerPage);
            var course_page2_last = await _courseService.GetCourseAsync((page+1)*itemsPerPage);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

            
            var course_page1 = JsonConvert.DeserializeObject<List<CourseModel>>(await response.Content.ReadAsStringAsync());
            var course_page2 = JsonConvert.DeserializeObject<List<CourseModel>>(await response2.Content.ReadAsStringAsync());
            Assert.True(course_page1[course_page1.Count-1].Name.Equals(course_page1_last.Name));
            Assert.True(course_page1[course_page1.Count-1].Price.Equals(course_page1_last.Price));
            Assert.True(course_page2[course_page2.Count-1].Name.Equals(course_page2_last.Name));
            Assert.True(course_page2[course_page2.Count-1].Price.Equals(course_page2_last.Price));
        }

        [Fact]
        public async Task Paging_InCourseList_Should_Contains_Correct_Items()
        {
            //Arrange

            int page = 1;
            int itemsPerPage = 5;

            //Act 
            var response = await _courseService.GetCourseListInternalAsync(page, itemsPerPage, _cookie);
           
            //Assert
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var course_page1 = JsonConvert.DeserializeObject<List<CourseModel>>(await response.Content.ReadAsStringAsync());
           
            Assert.Equal(course_page1.Count,itemsPerPage);
                        
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task ItemsPerPageAlgorithmShouldReturnCorrectData_RPRY(int page) //RPRY: см. мой вариант после добавления в АПИ Id
        {
            //Arrange
            var itemsPerPage = 2;
            await _courseService.EnsureThatCoursesListAvailableAsync(page * itemsPerPage, _cookie);

            //Act
            var courses = await _courseService.GetCoursesPerPageAsync(page, itemsPerPage, _cookie);

            //Assert
            Assert.Equal(itemsPerPage, courses.Count);
            var actualListIds = courses.Select(c => c.Id).ToList();
            var orderedActualListIds = actualListIds.OrderBy(id => id).ToList();
            Assert.Equal(actualListIds, orderedActualListIds);
        }
    }
}