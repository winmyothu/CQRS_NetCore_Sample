using MediatR;

namespace CQRSExample.Features.Payments.Queries;

public record GetYearlyRegistrationFeesQuery : IRequest<List<YearlyRegistrationFeeDto>>;

public record YearlyRegistrationFeeDto(string Year, decimal TotalFee);