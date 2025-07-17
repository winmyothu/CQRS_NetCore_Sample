using CQRSExample.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CQRSExample.Features.Payments.Queries;

namespace CQRSExample.Features.Payments.Handlers;

public class GetYearlyRegistrationFeesHandler : IRequestHandler<GetYearlyRegistrationFeesQuery, List<YearlyRegistrationFeeDto>>
{
    private readonly AppDbContext _context;

    public GetYearlyRegistrationFeesHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<YearlyRegistrationFeeDto>> Handle(GetYearlyRegistrationFeesQuery request, CancellationToken cancellationToken)
    {
        var yearlyFees =  _context.PaymentTransactions
            .GroupBy(pt => pt.TransactionDate.Year)
            .AsEnumerable() // Perform client-side evaluation for subsequent operations
            .Select(g => new YearlyRegistrationFeeDto(
                g.Key.ToString(),
                g.Sum(pt => pt.Amount)
            ))
            .OrderBy(x => x.Year).ToList();

        return yearlyFees;
    }
}