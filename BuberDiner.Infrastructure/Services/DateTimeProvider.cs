using BuberDiner.Application.Common.Services;

namespace BuberDiner.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}