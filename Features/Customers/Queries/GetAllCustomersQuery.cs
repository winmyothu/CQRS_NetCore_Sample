using MediatR;
using CQRSExample.Models;

namespace CQRSExample.Features.Customers.Queries
{
    public record GetAllCustomersQuery() : IRequest<List<Customer>>;
}
