using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WishListApp.Models;
using WishListApp.Repository;

namespace WishListUnitTestes.WishRepositoryTest
{
    public class WishListRepositoryTestes
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        private readonly Mock<IOptions<ApiSettings>> _mockApiSettings = new Mock<IOptions<ApiSettings>>();

        public WishListRepositoryTestes()
        {
            // Настройка ApiSettings
            _mockApiSettings.Setup(options => options.Value).Returns(new ApiSettings
            {
                RequestURL = "https://localhost:7043/api/WishItem"
            });

            // Настройка HttpContextAccessor и HttpContext
            var mockHttpContext = new DefaultHttpContext();
            var mockCookies = new Mock<IResponseCookies>();

            // Инициализация контекста для Cookies и Session
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext);
            mockHttpContext.Session = new Mock<ISession>().Object;

            // Настройка работы с куками
            mockCookies
                .Setup(c => c.Append(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>()));

            // Настройка HttpClient и ответа от API
            var mockTokenResponse = new JwtTokenResponse
            {
                token = "testToken"
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(mockTokenResponse), Encoding.UTF8, "application/json")
                });
        }

        public void SetupHttpContextAccessor()
        {
            // Мок для сессии
            var mockSession = new Mock<ISession>();

            // Настройка сессии для возвращения токена при вызове TryGetValue
            byte[] tokenBytes = Encoding.UTF8.GetBytes("testToken");
            mockSession.Setup(s => s.TryGetValue("authToken", out tokenBytes)).Returns(true);

            // Мок для HttpContext
            var mockHttpContext = new DefaultHttpContext();
            mockHttpContext.Session = mockSession.Object;

            // Настройка IHttpContextAccessor для возврата нашего mockHttpContext
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext);
        }

        
        public void SetupFakeContentWithResponse(string expectedJsonResponce)
        {
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedJsonResponce, Encoding.UTF8, "application/json")
                });
        }


        [Fact]
        public async Task LogInTest()
        {
            //Arange - Varibles, classes, mocks
            HttpClient httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            var wishRepository = new WishRepository(_mockApiSettings.Object, httpClient, _mockHttpContextAccessor.Object);
            

            //Act 
            Func<Task> act = async () => await wishRepository.LogIn(" ", " ");

            // Assert - проверяем, что метод не вызывает исключений и выполняется успешно
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task GetWishItemTest()
        {
            //Arange - Varibles, classes, mocks
            var expectedWishItem = new WishItem { Id = 45, Title = "buy new car AAA", Description = "Porshe 911" };
            var jsonResponse = JsonSerializer.Serialize(expectedWishItem);

            SetupFakeContentWithResponse(jsonResponse);
            SetupHttpContextAccessor();

            HttpClient httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            var wishRepository = new WishRepository(_mockApiSettings.Object, httpClient, _mockHttpContextAccessor.Object);

            //Act 
            var result = await wishRepository.GetWishItem(45);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(expectedWishItem.Id);
            result.Title.Should().Be(expectedWishItem.Title);
            result.Description.Should().Be(expectedWishItem.Description);
            result.Should().BeOfType<WishItem>();
        }


        [Fact]
        public async Task GetWishItemsTest()
        {
            //Arange
            var expectedWishItemList = new List<WishItem>
            {
                new WishItem
                {
                    Id = 1,
                    Title = "test",
                    Description = "test"
                },
                new WishItem
                {
                    Id= 2,
                    Title = "test",
                    Description = "test"
                },
                new WishItem
                {
                    Id= 3,
                    Title = "test",
                    Description = "test"
                },
                new WishItem
                {
                    Id = 4,
                    Title = "test",
                    Description = "test"
                }
            };

            var jsonResponse = JsonSerializer.Serialize(expectedWishItemList);

            SetupFakeContentWithResponse(jsonResponse);
            SetupHttpContextAccessor();


            HttpClient httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            var wishRepository = new WishRepository(_mockApiSettings.Object, httpClient, _mockHttpContextAccessor.Object);

            //Act 
            var result = await wishRepository.GetWishItems();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<WishItem>>();
        }


        [Fact]
        public async Task DeleteWishItemTest()
        {
            SetupHttpContextAccessor();


            HttpClient httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            var wishRepository = new WishRepository(_mockApiSettings.Object, httpClient, _mockHttpContextAccessor.Object);

            var result = wishRepository.DeleteWishItem(2);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateWishItemTest()
        {
            var expectedWishItem = new WishItem { Id = 45, Title = "buy new car AAA", Description = "Porshe 911" };
            var jsonResponse = JsonSerializer.Serialize(expectedWishItem);

            SetupFakeContentWithResponse(jsonResponse);
            SetupHttpContextAccessor();

            HttpClient httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            var wishRepository = new WishRepository(_mockApiSettings.Object, httpClient, _mockHttpContextAccessor.Object);


            var result = wishRepository.CreateWishItem(expectedWishItem);

            result.Should().NotBeNull();
        }


        [Fact]
        public async Task UpdateWishItemTest()
        {
            var expectedWishItem = new WishItem { Id = 45, Title = "buy new car AAA", Description = "Porshe 911" };
            var jsonResponse = JsonSerializer.Serialize(expectedWishItem);

            SetupFakeContentWithResponse(jsonResponse);
            SetupHttpContextAccessor();

            HttpClient httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            var wishRepository = new WishRepository(_mockApiSettings.Object, httpClient, _mockHttpContextAccessor.Object);

            var result = wishRepository.UpdateWishItem(expectedWishItem);

            result.Should().NotBeNull();
        }

    }
}
