using MediatR;

namespace CQRSExample.Features.Customers.Commands
{

  public record CreateCustomerCommand(string Name, string Email) : IRequest<int>;
}
