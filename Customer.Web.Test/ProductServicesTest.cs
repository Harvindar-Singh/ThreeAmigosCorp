using Customer.Web.Product.Services;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Xunit;
using Assert = Xunit.Assert;

namespace Customer.Web.Test
{
    public class ProductServicesTest
    {
        private Mock<HttpMessageHandler> CreateHttpMock(HttpStatusCode expectedCode,
                                                        string expectedJson)
        {
            var response = new HttpResponseMessage
            {
                StatusCode = expectedCode
            };
            if (expectedJson != null)
            {
                response.Content = new StringContent(expectedJson,
                                                     Encoding.UTF8,
                                                     "application/json");
            }
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response)
                .Verifiable();
            return mock;
        }

        private IProductServices CreateProductsService(HttpClient client)
        {
            var mockConfiguration = new Mock<IConfiguration>(MockBehavior.Strict);
            mockConfiguration.Setup(c => c["WebServices:Products:BaseURL"])
                             .Returns("http://example.com/api/products/1");
            return new ProductServices(client, mockConfiguration.Object);
        }

        /*[Fact]
        public async Task GetProductAsync_WithValid_ShouldOkEntity()
        {
            // Arrange
            var expectedResult = new ProductDto { ProductId = 1, BrandId = 1, ProductName = "Rippled Screen Protector", CategoryName = "Screen Protectors", CategoryId = 1, Description = "For his or her sensory pleasure. Fits few known smartphones.", BrandName = "iStuff-R-Us", InStock = true, Price = 8.77 };
            var expectedJson = JsonConvert.SerializeObject(expectedResult);
            var expectedUri = new Uri("http://example.com/api/products/1");
            var mock = CreateHttpMock(HttpStatusCode.OK, expectedJson);
            var client = new HttpClient(mock.Object);
            var service = CreateProductsService(client);

            // Act
            var result = await service.GetProductAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.ProductId, result.ProductId);
            // FIXME: could assert other result property values
            mock.Protected()
                .Verify("SendAsync",
                        Times.Once(),
                        ItExpr.Is<HttpRequestMessage>(
                            req => req.Method == HttpMethod.Get
                                   && req.RequestUri == expectedUri),
                        ItExpr.IsAny<CancellationToken>()
                        );
        }*/

        /* [Fact]
         public async Task GetProductAsync_WithInvalid_ShouldReturnNull()
         {
             // Arrange
             var expectedUri = new Uri("http://example.com/api/products/1");
             var mock = CreateHttpMock(HttpStatusCode.NotFound, null);
             var client = new HttpClient(mock.Object);
             var service = CreateProductsService(client);

             // Act
             var result = await service.GetProductAsync(100);

             // Assert
             Assert.Null(result);
             mock.Protected()
                 .Verify("SendAsync",
                         Times.Once(),
                         ItExpr.Is<HttpRequestMessage>(
                             req => req.Method == HttpMethod.Get
                                    && req.RequestUri == expectedUri),
                         ItExpr.IsAny<CancellationToken>()
                         );
         }
        */
        [Fact]
         public async Task GetProductAsync_OnHttpBad_ShouldThrow()
         {
             // Arrange
             var expectedUri = new Uri("http://example.com/api/products/1");
             var mock = CreateHttpMock(HttpStatusCode.ServiceUnavailable, null);
             var client = new HttpClient(mock.Object);
             var service = CreateProductsService(client);

             // Act & Assert
             await Assert.ThrowsAsync<HttpRequestException>(
                 () => service.GetProductAsync(1));
         }
 
        /*[Fact]
        public async Task GetProductAsync_WithNull_ShouldReturnAll()
        {
            // Arrange
            var expectedResult = new ProductDto[]
            {
                new ProductDto { ProductId= 1, BrandId = 1, ProductName = "Rippled Screen Protector",  CategoryName = "Screen Protectors", CategoryId = 1, Description = "For his or her sensory pleasure. Fits few known smartphones.", BrandName = "iStuff-R-Us", InStock = true, Price = 8.77 },
           new ProductDto { ProductId = 2, BrandId = 3, ProductName = "Wrap It and Hope Cover", CategoryName = "Covers", CategoryId = 2, Description = "Poor quality fake faux leather cover loose enough to fit any mobile device.", BrandName = "Soggy Sponge", InStock = true, Price = 3.46 },
           new ProductDto { ProductId = 3, BrandId = 2, ProductName = "Chocolate Cover", CategoryName = "Screen Protectors", CategoryId = 2, Description = "Purchase you favourite chocolate and use the provided heating element to melt it into the perfect cover for your mobile device.", BrandName = "Soggy Sponge", InStock = false, Price = 8.77 },
           new ProductDto { ProductId = 4, BrandId = 3, ProductName = "Water Bath Case", CategoryName = "Case", CategoryId = 3, Description = "Place your device within the water-tight container, fill with water and enjoy the cushioned protection from bumps and bangs.", BrandName = "Soggy Sponge", InStock = true, Price = 11.46 },
           new ProductDto { ProductId = 5, BrandId = 3, ProductName = "Smartphone Car Holder", CategoryName = "Accessories", CategoryId = 4, Description = "Keep you smartphone handsfree with this large assembly that attaches to your rear window wiper (Hatchbacks only).", BrandName = "iStuff-R-Us", InStock = true, Price = 90.82 },
           new ProductDto { ProductId = 6, BrandId = 3, ProductName = "Sticky Tape Sport Armband", CategoryName = "Accessories", CategoryId = 4, Description = "Keep your device on your arm with this general purpose sticky tape.", BrandName = "Soggy Sponge", InStock = true, Price = 2.23 },
           new ProductDto { ProductId = 7, BrandId = 1, ProductName = "Cloth Cover", CategoryName = "Covers", CategoryId = 2, Description = "Lamely adapted used and dirty teatowel. Guaranteed fewer than two holes.", BrandName = "iStuff-R-Us", InStock = true, Price = 2.9 }

            };
            var expectedJson = JsonConvert.SerializeObject(expectedResult);
            var expectedUri = new Uri("http://example.com/api/product");
            var mock = CreateHttpMock(HttpStatusCode.OK, expectedJson);
            var client = new HttpClient(mock.Object);
            var service = CreateProductsService(client);

            // Act
            var result = (await service.GetProductsAsync()).ToArray();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Length, result.Length);
            for (int i = 0; i < result.Length; i++)
            {
                Assert.Equal(expectedResult[i].ProductId, result[i].ProductId);
                // FIXME: could assert other result property values
            }
            mock.Protected()
                .Verify("SendAsync",
                        Times.Once(),
                        ItExpr.Is<HttpRequestMessage>(
                            req => req.Method == HttpMethod.Get
                                   && req.RequestUri == expectedUri),
                        ItExpr.IsAny<CancellationToken>()
                        );}*/

    }
}
