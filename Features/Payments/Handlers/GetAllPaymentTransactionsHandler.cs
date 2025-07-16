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
    public class GetAllPaymentTransactionsHandler : IRequestHandler<GetAllPaymentTransactionsQuery, PaginatedResult<PaymentTransaction>>
    {
        private readonly AppDbContext _context;

        public GetAllPaymentTransactionsHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<PaymentTransaction>> Handle(GetAllPaymentTransactionsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.PaymentTransactions.Include(pt => pt.GuestRegistration).AsQueryable();

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<PaymentTransaction>(items, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
