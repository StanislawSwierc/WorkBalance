using WorkBalance.Infrastructure;

namespace WorkBalance.Domain
{
    public interface IDomainContext : IObjectContext
    {
        IObjectSet<Activity> Activities { get; }
        IObjectSet<ActivityTag> ActivityTags { get; }
        IObjectSet<Sprint> Sprints { get; }
    }
}
