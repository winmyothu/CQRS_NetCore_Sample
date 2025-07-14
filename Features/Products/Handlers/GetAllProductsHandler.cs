using CQRSExample.Data;
using CQRSExample.Models;
using MediatR;
using CQRSExample.Features.Products.Queries;
using Microsoft.EntityFrameworkCore;

namespace CQRSExample.Features.Products.Handlers
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<Product>>
    {
        private readonly AppDbContext _context;
        public GetAllProductsHandler(AppDbContext context) => _context = context;

        public async Task<List<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products.ToListAsync();
        }
    }
}
