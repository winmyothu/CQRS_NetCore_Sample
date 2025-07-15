using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CQRSExample.Data;
using CQRSExample.Models;
using Microsoft.EntityFrameworkCore;
using CQRSExample.Features.Payments.Queries;

namespace CQRSExample.Features.Payments.Handlers
{
    public class GetAllPaymentTransactionsHandler : IRequestHandler<GetAllPaymentTransactionsQuery, IEnumerable<PaymentTransaction>>
    {
        private readonly AppDbContext _context;

        public GetAllPaymentTransactionsHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaymentTransaction>> Handle(GetAllPaymentTransactionsQuery request, CancellationToken cancellationToken)
        {
            return await _context.PaymentTransactions.Include(pt => pt.GuestRegistration).ToListAsync(cancellationToken);
        }
    }
}
