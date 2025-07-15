using MediatR;
using System.Collections.Generic;
using CQRSExample.Models;

namespace CQRSExample.Features.Payments.Queries
{
    public record GetAllPaymentTransactionsQuery() : IRequest<IEnumerable<PaymentTransaction>>;
}
