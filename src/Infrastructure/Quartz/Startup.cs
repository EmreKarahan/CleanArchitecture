using Shared.Attributes;

namespace Infrastructure.Quartz;

public static class Startup
{
    public static IServiceCollection AddScheduledJob(this IServiceCollection services, IConfiguration configuration)
    {

        var scheduler = CreateScheduler();
        scheduler.JobFactory = new MyJobFactory(services.BuildServiceProvider());
        services.AddSingleton(scheduler);
        services.AddHostedService<QuartzHostedService>();
        services.AddQuartzmon();
        return services;
    }

    public static WebApplication UseScheduledJob(this WebApplication application)
    {
        var scheduler =  application.Services.GetService<IScheduler>();
        
        
        application.UseQuartzmon(new QuartzmonOptions() {Scheduler = scheduler});
        // application.UseQuartzmon(new QuartzmonOptions()
        // {
        //     Scheduler = StdSchedulerFactory.GetDefaultScheduler().Result
        // });

        return application;
    }

    private static IScheduler CreateScheduler()
    {
        
        
        var schedulerFactory = new StdSchedulerFactory();
        var scheduler = schedulerFactory.GetScheduler().Result;

        

        // var job = JobBuilder.Create<Infrastructure.Quartz.NOnbir.UpdateCategoryAttributeJob>()
        //     .WithIdentity(nameof(UpdateCategoryAttributeJob), "default")
        //     .Build();
        //
        //
        //
        // var trigger = TriggerBuilder.Create()
        //     .WithIdentity($"{nameof(UpdateCategoryAttributeJob)}_Trigger", "default")
        //     .ForJob(job)
        //     .StartNow()
        //     .WithCronSchedule("0 /1 * ? * *")
        //     .Build();
        //
        // scheduler.ScheduleJob(job, trigger);


        var jobs = GetJobs();

        foreach (Type jobType in jobs)
        {
            var jobAttribute = jobType.GetAttributeValue((ScheduledJobAttribute dna) => dna);
            
            if(jobAttribute == null)
                continue;
            
            var job = JobBuilder.Create(jobType)
                .WithIdentity($"{jobAttribute.IdentityName}_{jobAttribute.IdentityGroup}", jobAttribute.IdentityGroup)
                .Build();
            
        
        
            var trigger = TriggerBuilder.Create()
                .WithIdentity($"{jobAttribute.TriggerName}_{jobAttribute.TriggerGroup}_Trigger", jobAttribute.TriggerGroup)
                .ForJob(job)
                .StartNow()
                .WithCronSchedule(jobAttribute.CronSchedule)
                .Build();
            
            scheduler.ScheduleJob(job, trigger);
        }


        scheduler.Start();

        return scheduler;
    }

    private static List<Type> GetJobs()
    {
        var type = typeof(IJob);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p));
        return types.ToList();
    }
}