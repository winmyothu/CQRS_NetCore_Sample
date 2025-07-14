
using CQRSExample.Data;
using CQRSExample.Features.Products.Commands;
using CQRSExample.Features.Products.Handlers;
using CQRSExample.Features.Products.Queries;
using CQRSExample.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSExample.Tests.Features.Products
{
    [TestFixture]
    public class ProductTests
    {
        private AppDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Products")
                .Options;
            _context = new AppDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task CreateProductHandler_ShouldAddProduct()
        {
            // Arrange
            var handler = new CreateProductHandler(_context);
            var command = new CreateProductCommand("Test Product", 10.0m);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            var productInDb = await _context.Products.FindAsync(result);
            Assert.That(productInDb, Is.Not.Null);
            Assert.That(productInDb.Name, Is.EqualTo(command.Name));
            Assert.That(productInDb.Price, Is.EqualTo(command.Price));
        }

        [Test]
        public async Task GetProductByIdHandler_ShouldReturnProduct()
        {
            // Arrange
            var product = new Product { Name = "Test Product", Price = 15.50m };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var handler = new GetProductByIdHandler(_context);
            var query = new GetProductByIdQuery(product.Id);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(product.Id));
        }

        [Test]
        public async Task GetAllProductsHandler_ShouldReturnAllProducts()
        {
            // Arrange
            _context.Products.Add(new Product { Name = "Product 1", Price = 10.0m });
            _context.Products.Add(new Product { Name = "Product 2", Price = 20.0m });
            await _context.SaveChangesAsync();

            var handler = new GetAllProductsHandler(_context);
            var query = new GetAllProductsQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task UpdateProductHandler_ShouldUpdateProduct()
        {
            // Arrange
            var product = new Product { Name = "Old Name", Price = 5.0m };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var handler = new UpdateProductHandler(_context);
            var command = new UpdateProductCommand(product.Id, "New Name", 7.5m);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedProduct = await _context.Products.FindAsync(product.Id);
            Assert.That(updatedProduct, Is.Not.Null);
            Assert.That(updatedProduct.Name, Is.EqualTo("New Name"));
            Assert.That(updatedProduct.Price, Is.EqualTo(7.5m));
        }

        [Test]
        public async Task DeleteProductHandler_ShouldDeleteProduct()
        {
            // Arrange
            var product = new Product { Name = "Test Product", Price = 12.0m };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var handler = new DeleteProductHandler(_context);
            var command = new DeleteProductCommand(product.Id);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var deletedProduct = await _context.Products.FindAsync(product.Id);
            Assert.That(deletedProduct, Is.Null);
        }
    }
}
