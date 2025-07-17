using CQRSExample.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CQRSExample.Features.GuestRegistrations.Queries;

namespace CQRSExample.Features.GuestRegistrations.Handlers;

public class GetMonthlyRegistrationsHandler : IRequestHandler<GetMonthlyRegistrationsQuery, List<MonthlyRegistrationDto>>
{
    private readonly AppDbContext _context;

    public GetMonthlyRegistrationsHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MonthlyRegistrationDto>> Handle(GetMonthlyRegistrationsQuery request, CancellationToken cancellationToken)
    {
        var monthlyRegistrationsRaw = await _context.GuestRegistrations
            .GroupBy(g => new { g.RegistrationDate.Year, g.RegistrationDate.Month })
            .Select(g => new 
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Count = g.Count()
            })
            .ToListAsync(cancellationToken);

        return monthlyRegistrationsRaw
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .Select(x => new MonthlyRegistrationDto(
                new DateTime(x.Year, x.Month, 1).ToString("MMM yyyy"),
                x.Count
            ))
            .ToList();
    }
}