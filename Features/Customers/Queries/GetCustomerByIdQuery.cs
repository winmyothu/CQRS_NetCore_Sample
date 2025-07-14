using MediatR;
using CQRSExample.Models;

namespace CQRSExample.Features.Customers.Queries
{
    public record GetCustomerByIdQuery(int Id) : IRequest<Customer>;
}
