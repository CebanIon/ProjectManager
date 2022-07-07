using ProjectManager.Application.Common.Interfaces;

namespace ProjectManager.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
