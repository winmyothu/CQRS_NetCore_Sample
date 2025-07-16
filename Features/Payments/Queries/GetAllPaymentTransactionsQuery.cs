using MediatR;
using System.Collections.Generic;
using CQRSExample.Models;

namespace CQRSExample.Features.Payments.Queries
{
    public record GetAllPaymentTransactionsQuery(int PageNumber, int PageSize) : IRequest<PaginatedResult<PaymentTransaction>>;
}
