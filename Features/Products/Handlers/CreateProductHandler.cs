using CQRSExample.Data;
using CQRSExample.Models;
using MediatR;
using CQRSExample.Features.Products.Commands;

namespace CQRSExample.Features.Products.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly AppDbContext _context;
        public CreateProductHandler(AppDbContext context) => _context = context;

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product { Name = request.Name, Price = request.Price };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }
    }
}
