using Shared.Attributes;
using Application.Common.Extensions;
using Infrastructure.Quartz.NOnbir;
using Mapster;

namespace Infrastructure.Quartz;

public static class Startup
{
    public static IServiceCollection AddScheduledJob(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddQuartz(q =>
        // {
        //     // base Quartz scheduler, job and trigger configuration
        // });


        services.AddScoped<Infrastructure.Quartz.NOnbir.UpdateCategoryAttributeJob>();
        services.AddScoped<Infrastructure.Quartz.NOnbir.UpdateSubCategoryJob>();
        services.AddScoped<Infrastructure.Quartz.NOnbir.UpdateTopCategoryJob>();
        services.AddScoped<Infrastructure.Quartz.Trendyol.UpdateCategoryAttribute>();
        services.AddScoped<Infrastructure.Quartz.Trendyol.UpdateCategoryJob>();

        
        services.AddQuartzmon();
        return services;
    }
    
    public static WebApplication UseScheduledJob(this WebApplication application)
    {
        var scheduler = CreateScheduler(application);
        application.UseQuartzmon(new QuartzmonOptions()
        {
            Scheduler = scheduler
        });
        
        return application;
    }

    private static IScheduler CreateScheduler(WebApplication webApplication)
    {
        var schedulerFactory = new StdSchedulerFactory();
        var scheduler = schedulerFactory.GetScheduler().Result;


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

