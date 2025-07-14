using MediatR;
using CQRSExample.Data;
using Microsoft.EntityFrameworkCore;
using CQRSExample.Features.Customers.Commands;

namespace CQRSExample.Features.Customers.Handlers
{
    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, bool>
    {
        private readonly AppDbContext _context;

        public UpdateCustomerHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (customer == null)
            {
                return false;
            }

            customer.Name = request.Name;
            customer.Email = request.Email;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
