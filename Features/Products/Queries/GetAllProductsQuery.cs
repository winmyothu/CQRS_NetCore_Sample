using CQRSExample.Models;
using MediatR;

namespace CQRSExample.Features.Products.Queries
{
    public record GetAllProductsQuery() : IRequest<List<Product>>;
}
