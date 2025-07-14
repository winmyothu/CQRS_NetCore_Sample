using MediatR;

namespace CQRSExample.Features.Products.Commands
{
    public record CreateProductCommand(string Name, decimal Price) : IRequest<int>;
}
