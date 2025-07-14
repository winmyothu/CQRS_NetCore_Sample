using MediatR;
using CQRSExample.Data;
using CQRSExample.Models;
using Microsoft.EntityFrameworkCore;
using CQRSExample.Features.Customers.Queries;

namespace CQRSExample.Features.Customers.Handlers
{
    public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, Customer?>
    {
        private readonly AppDbContext _context;

        public GetCustomerByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        }
    }
}
