namespace BuberDiner.Application.Common.Services;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}