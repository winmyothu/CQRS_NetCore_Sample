using MediatR;
using CQRSExample.Models;

namespace CQRSExample.Features.Products.Queries
{
    public record GetProductByIdQuery(int Id) : IRequest<Product>;
}
