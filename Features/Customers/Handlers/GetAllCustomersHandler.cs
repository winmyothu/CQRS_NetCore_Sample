using CQRSExample.Data;
using CQRSExample.Models;
using MediatR;
using CQRSExample.Features.Customers.Queries;
using Microsoft.EntityFrameworkCore;

namespace CQRSExample.Features.Customers.Handlers
{
    public class GetAllCustomersHandler : IRequestHandler<GetAllCustomersQuery, List<Customer>>
    {
        private readonly AppDbContext _context;
        public GetAllCustomersHandler(AppDbContext context) => _context = context;

        public async Task<List<Customer>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Customers.ToListAsync();
        }
    }
}
