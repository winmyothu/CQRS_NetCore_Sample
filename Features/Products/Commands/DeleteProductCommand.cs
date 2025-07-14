using MediatR;

namespace CQRSExample.Features.Products.Commands
{
    public record DeleteProductCommand(int Id) : IRequest<bool>;
}
