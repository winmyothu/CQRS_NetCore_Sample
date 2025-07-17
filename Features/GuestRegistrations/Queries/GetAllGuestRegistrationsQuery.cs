using MediatR;
using CQRSExample.Models;

namespace CQRSExample.Features.GuestRegistrations.Queries
{
    public class GetAllGuestRegistrationsQuery : IRequest<PaginatedResult<GuestRegistration>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SearchTerm { get; set; }
        public string? SortField { get; set; }
        public string? SortOrder { get; set; }

        public GetAllGuestRegistrationsQuery(int pageNumber, int pageSize, string? searchTerm, string? sortField, string? sortOrder)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchTerm = searchTerm;
            SortField = sortField;
            SortOrder = sortOrder;
        }
    }
}

