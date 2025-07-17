using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CQRSExample.Data;
using CQRSExample.Models;
using CQRSExample.Features.Payments.Queries;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

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
            var queryable = _context.PaymentTransactions.AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                queryable = queryable.Where(p =>
                    p.Status.ToLower().Contains(searchTerm) ||
                    p.Amount.ToString().Contains(searchTerm)
                );
            }

            if (!string.IsNullOrEmpty(request.SortField))
            {
                var sortOrder = request.SortOrder ?? "asc";
                queryable = queryable.OrderBy($"{request.SortField} {sortOrder}");
            }

            var totalCount = await queryable.CountAsync(cancellationToken);

            var payments = await queryable
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<PaymentTransaction>(payments, totalCount, request.PageNumber, request.PageSize);
        }
    }
}

