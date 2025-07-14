using MediatR;
using CQRSExample.Data;
using CQRSExample.Models;
using Microsoft.EntityFrameworkCore;
using CQRSExample.Features.Products.Queries;

namespace CQRSExample.Features.Products.Handlers
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product?>
    {
        private readonly AppDbContext _context;

        public GetProductByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        }
    }
}
