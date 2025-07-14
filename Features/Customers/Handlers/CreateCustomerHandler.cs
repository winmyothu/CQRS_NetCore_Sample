using CQRSExample.Data;
using CQRSExample.Models;
using MediatR;
using CQRSExample.Features.Customers.Commands;
using Microsoft.EntityFrameworkCore;

namespace CQRSExample.Features.Customers.Handlers
{
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, int>
    {
        private readonly AppDbContext _context;
        public CreateCustomerHandler(AppDbContext context) => _context = context;

        public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                Name = request.Name,
                Email = request.Email
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return customer.Id;
        }
    }
}
