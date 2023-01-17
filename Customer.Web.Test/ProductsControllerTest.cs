using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Customer.Web.Controllers;
using Customer.Web.Product.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace Customer.Web.Test
{

    public class ProductsControllerTest
    {
        private ProductDto[] GetTestProducts() => new ProductDto[] {
           new ProductDto { ProductId= 1, BrandId = 1, ProductName = "Rippled Screen Protector",  CategoryName = "Screen Protectors", CategoryId = 1, Description = "For his or her sensory pleasure. Fits few known smartphones.", BrandName = "iStuff-R-Us", InStock = true, Price = 8.77 },
           new ProductDto { ProductId = 2, BrandId = 3, ProductName = "Wrap It and Hope Cover", CategoryName = "Covers", CategoryId = 2, Description = "Poor quality fake faux leather cover loose enough to fit any mobile device.", BrandName = "Soggy Sponge", InStock = true, Price = 3.46 },
           new ProductDto { ProductId = 3, BrandId = 2, ProductName = "Chocolate Cover", CategoryName = "Screen Protectors", CategoryId = 2, Description = "Purchase you favourite chocolate and use the provided heating element to melt it into the perfect cover for your mobile device.", BrandName = "Soggy Sponge", InStock = false, Price = 8.77 },
           new ProductDto { ProductId = 4, BrandId = 3, ProductName = "Water Bath Case", CategoryName = "Case", CategoryId = 3, Description = "Place your device within the water-tight container, fill with water and enjoy the cushioned protection from bumps and bangs.", BrandName = "Soggy Sponge", InStock = true, Price = 11.46 },
           new ProductDto { ProductId = 5, BrandId = 3, ProductName = "Smartphone Car Holder", CategoryName = "Accessories", CategoryId = 4, Description = "Keep you smartphone handsfree with this large assembly that attaches to your rear window wiper (Hatchbacks only).", BrandName = "iStuff-R-Us", InStock = true, Price = 90.82 },
           new ProductDto { ProductId = 6, BrandId = 3, ProductName = "Sticky Tape Sport Armband", CategoryName = "Accessories", CategoryId = 4, Description = "Keep your device on your arm with this general purpose sticky tape.", BrandName = "Soggy Sponge", InStock = true, Price = 2.23 },
           new ProductDto { ProductId = 7, BrandId = 1, ProductName = "Cloth Cover", CategoryName = "Covers", CategoryId = 2, Description = "Lamely adapted used and dirty teatowel. Guaranteed fewer than two holes.", BrandName = "iStuff-R-Us", InStock = true, Price = 2.9 }

        };
        [Fact]
        public async Task GetIndex_WithInvalidModelState_ShouldBadResult()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductsController>>();
            var mockProducts = new Mock<IProductServices>();
            var controller = new ProductsController(mockLogger.Object,
                                                   mockProducts.Object);
            controller.ModelState.AddModelError("Something", "Something");

            // Act
            var result = await controller.Index();

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task GetIndex_WhenBadServiceCall_ShouldViewEmptyEnumerable()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductsController>>();
            var mockProducts = new Mock<IProductServices>(MockBehavior.Strict);
            mockProducts.Setup(r => r.GetProductsAsync())
                       .ThrowsAsync(new Exception())
                       .Verifiable();
            var controller = new ProductsController(mockLogger.Object,
                                                   mockProducts.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(
                viewResult.ViewData.Model);
            Assert.Empty(model);
            mockProducts.Verify(r => r.GetProductsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetDetails_WithInvalidModelState_ShouldBadResult()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductsController>>();
            var mockProducts = new Mock<IProductServices>();
            var controller = new ProductsController(mockLogger.Object,
                                                   mockProducts.Object);
            controller.ModelState.AddModelError("Something", "Something");

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetDetails_WithNullId_ShouldBadResult()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductsController>>();
            var mockProducts = new Mock<IProductServices>();
            var controller = new ProductsController(mockLogger.Object,
                                                   mockProducts.Object);

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetDetails_WhenBadServiceCall_ShouldInternalError()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductsController>>();
            var mockProducts = new Mock<IProductServices>(MockBehavior.Strict);
            mockProducts.Setup(r => r.GetProductAsync(3))
                       .ThrowsAsync(new Exception())
                       .Verifiable();
            var controller = new ProductsController(mockLogger.Object,
                                                   mockProducts.Object);

            // Act
            var result = await controller.Details(3);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status503ServiceUnavailable,
                         statusCodeResult.StatusCode);
            mockProducts.Verify(r => r.GetProductAsync(3), Times.Once);
        }

        [Fact]
        public async Task GetDetails_WithUnknownId_ShouldNotFound()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductsController>>();
            var mockProducts = new Mock<IProductServices>(MockBehavior.Strict);
            mockProducts.Setup(r => r.GetProductAsync(13))
                       .ReturnsAsync((ProductDto)null)
                       .Verifiable();
            var controller = new ProductsController(mockLogger.Object,
                                                   mockProducts.Object);

            // Act
            var result = await controller.Details(13);

            // Assert
            var statusCodeResult = Assert.IsType<NotFoundResult>(result);
            mockProducts.Verify(r => r.GetProductAsync(13), Times.Once);
        }

        [Fact]
        public async Task GetDetails_WithId_ShouldViewServiceObject()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ProductsController>>();
            var mockProducts = new Mock<IProductServices>(MockBehavior.Strict);
            var expected = GetTestProducts().First();
            mockProducts.Setup(r => r.GetProductAsync(expected.ProductId))
                       .ReturnsAsync(expected)
                       .Verifiable();
            var controller = new ProductsController(mockLogger.Object,
                                                   mockProducts.Object);

            // Act
            var result = await controller.Details(expected.ProductId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductDto>(viewResult.ViewData.Model);
            Assert.Equal(expected.ProductId, model.ProductId);
            // FIXME: could assert other result property values here

            mockProducts.Verify(r => r.GetProductAsync(expected.ProductId), Times.Once);
        }
    }
}
