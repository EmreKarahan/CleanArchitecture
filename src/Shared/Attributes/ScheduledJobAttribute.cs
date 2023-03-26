namespace Shared.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ScheduledJobAttribute : Attribute
{
    public string IdentityName { get; set; }
    public string IdentityGroup { get; set; }
    public string TriggerName { get; set; }
    public string TriggerGroup { get; set; }
    public string CronSchedule { get; set; }
    
    public ScheduledJobAttribute(string identityName, string identityGroup, string triggerName, string triggerGroup, string cronSchedule)
    {
        IdentityName = identityName;
        IdentityGroup = identityGroup;
        TriggerName = triggerName;
        TriggerGroup = triggerGroup;
        CronSchedule = cronSchedule;
    }
}
