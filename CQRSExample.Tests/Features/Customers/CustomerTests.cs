
using CQRSExample.Data;
using CQRSExample.Features.Customers.Commands;
using CQRSExample.Features.Customers.Handlers;
using CQRSExample.Features.Customers.Queries;
using CQRSExample.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSExample.Tests.Features.Customers
{
    [TestFixture]
    public class CustomerTests
    {
        private AppDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Customers")
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
        public async Task CreateCustomerHandler_ShouldAddCustomer()
        {
            // Arrange
            var handler = new CreateCustomerHandler(_context);
            var command = new CreateCustomerCommand("Test Customer", "test@customer.com");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            var customerInDb = await _context.Customers.FindAsync(result);
            Assert.That(customerInDb, Is.Not.Null);
            Assert.That(customerInDb.Name, Is.EqualTo(command.Name));
            Assert.That(customerInDb.Email, Is.EqualTo(command.Email));
        }

        [Test]
        public async Task GetCustomerByIdHandler_ShouldReturnCustomer()
        {
            // Arrange
            var customer = new Customer { Name = "Test Customer", Email = "test@customer.com" };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var handler = new GetCustomerByIdHandler(_context);
            var query = new GetCustomerByIdQuery(customer.Id);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(customer.Id));
        }

        [Test]
        public async Task GetAllCustomersHandler_ShouldReturnAllCustomers()
        {
            // Arrange
            _context.Customers.Add(new Customer { Name = "Customer 1", Email = "c1@test.com" });
            _context.Customers.Add(new Customer { Name = "Customer 2", Email = "c2@test.com" });
            await _context.SaveChangesAsync();

            var handler = new GetAllCustomersHandler(_context);
            var query = new GetAllCustomersQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task UpdateCustomerHandler_ShouldUpdateCustomer()
        {
            // Arrange
            var customer = new Customer { Name = "Old Name", Email = "old@email.com" };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var handler = new UpdateCustomerHandler(_context);
            var command = new UpdateCustomerCommand(customer.Id, "New Name", "new@email.com");

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedCustomer = await _context.Customers.FindAsync(customer.Id);
            Assert.That(updatedCustomer, Is.Not.Null);
            Assert.That(updatedCustomer.Name, Is.EqualTo("New Name"));
            Assert.That(updatedCustomer.Email, Is.EqualTo("new@email.com"));
        }

        [Test]
        public async Task DeleteCustomerHandler_ShouldDeleteCustomer()
        {
            // Arrange
            var customer = new Customer { Name = "Test Customer", Email = "test@customer.com" };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var handler = new DeleteCustomerHandler(_context);
            var command = new DeleteCustomerCommand(customer.Id);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var deletedCustomer = await _context.Customers.FindAsync(customer.Id);
            Assert.That(deletedCustomer, Is.Null);
        }
    }
}
