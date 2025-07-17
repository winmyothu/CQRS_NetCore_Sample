using MediatR;

namespace CQRSExample.Features.GuestRegistrations.Queries;

public record GetMonthlyRegistrationsQuery : IRequest<List<MonthlyRegistrationDto>>;

public record MonthlyRegistrationDto(string Month, int Count);