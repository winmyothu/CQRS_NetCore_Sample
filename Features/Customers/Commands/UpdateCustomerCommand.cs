using MediatR;

namespace CQRSExample.Features.Customers.Commands
{
    public record UpdateCustomerCommand(int Id, string Name, string Email) : IRequest<bool>;
}
