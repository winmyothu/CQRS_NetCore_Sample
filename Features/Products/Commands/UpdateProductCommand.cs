using MediatR;

namespace CQRSExample.Features.Products.Commands
{
    public record UpdateProductCommand(int Id, string Name, decimal Price) : IRequest<bool>;
}
