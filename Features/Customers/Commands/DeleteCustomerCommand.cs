using MediatR;

namespace CQRSExample.Features.Customers.Commands
{
    public record DeleteCustomerCommand(int Id) : IRequest<bool>;
}
