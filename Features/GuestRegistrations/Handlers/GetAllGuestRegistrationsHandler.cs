using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CQRSExample.Data;
using CQRSExample.Models;
using CQRSExample.Features.GuestRegistrations.Queries;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CQRSExample.Features.GuestRegistrations.Handlers
{
    public class GetAllGuestRegistrationsHandler : IRequestHandler<GetAllGuestRegistrationsQuery, PaginatedResult<GuestRegistration>>
    {
        private readonly AppDbContext _context;

        public GetAllGuestRegistrationsHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<GuestRegistration>> Handle(GetAllGuestRegistrationsQuery request, CancellationToken cancellationToken)
        {
            var queryable = _context.GuestRegistrations.AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                queryable = queryable.Where(g =>
                    g.Name.ToLower().Contains(searchTerm) ||
                    g.PassportNumber.ToLower().Contains(searchTerm) ||
                    g.Nationality.ToLower().Contains(searchTerm) ||
                    g.Nrc.ToLower().Contains(searchTerm)
                );
            }

            if (!string.IsNullOrEmpty(request.SortField))
            {
                var sortOrder = request.SortOrder ?? "asc";
                queryable = queryable.OrderBy($"{request.SortField} {sortOrder}");
            }

            var totalCount = await queryable.CountAsync(cancellationToken);

            var registrations = await queryable
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<GuestRegistration>(registrations, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
