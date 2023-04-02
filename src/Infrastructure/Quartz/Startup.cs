using Domain.Entities.NOnbir;
using Infrastructure.Quartz.NOnbir;
using Shared.Attributes;

namespace Infrastructure.Quartz;

public static class Startup
{
    public static IServiceCollection AddScheduledJob(this IServiceCollection services, IConfiguration configuration)
    {
        var scheduler = CreateScheduler(services);
        scheduler.JobFactory = new MyJobFactory(services.BuildServiceProvider());
        services.AddSingleton(scheduler);
        services.AddHostedService<QuartzHostedService>();
        services.AddQuartzmon();
        return services;
    }

    public static WebApplication UseScheduledJob(this WebApplication application)
    {
        var scheduler = application.Services.GetService<IScheduler>();
        application.UseQuartzmon(new QuartzmonOptions() {Scheduler = scheduler});
        return application;
    }

    private static IScheduler CreateScheduler(IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();

        var categoryRepository = provider.GetService<IRepository<Category>>();
        var n11TopCategories = categoryRepository.GetAllBy(p => p.InternalParentId is null).OrderByDescending(o => o.Id)
            .ToList();
        var n11DeepestCategories = categoryRepository.GetAllBy(p => p.IsDeepest).ToList();

        var schedulerFactory = new StdSchedulerFactory();
        var scheduler = schedulerFactory.GetScheduler().Result;

        var jobs = GetJobs();

        foreach (Type jobType in jobs)
        {
            var jobAttribute = jobType.GetAttributeValue((ScheduledJobAttribute dna) => dna);

            if (jobAttribute == null)
                continue;

            // if (jobType == typeof(UpdateSubCategoryJob))
            // {
            //
            //     var minuteAdd = 0;
            //     foreach (var categoryItem in n11TopCategories)
            //     {
            //         
            //         var job = JobBuilder.Create(jobType)
            //             .WithIdentity($"{jobAttribute.IdentityName}_{categoryItem.InternalId}_{jobAttribute.IdentityGroup}", jobAttribute.IdentityGroup)
            //             .UsingJobData("parentCategoryId", categoryItem.InternalId)
            //             .Build();
            //     
            //     
            //     
            //         var trigger = TriggerBuilder.Create()
            //             .WithIdentity($"{jobAttribute.TriggerName}_{categoryItem.InternalId}_{jobAttribute.TriggerGroup}_Trigger", jobAttribute.TriggerGroup)
            //             .ForJob(job)
            //             //.StartNow()
            //             .StartAt(DateTime.Now.AddSeconds(minuteAdd))
            //             .WithSchedule(SimpleScheduleBuilder.RepeatMinutelyForever(20))
            //             //.WithCronSchedule(jobAttribute.CronSchedule)
            //             .Build();
            //         scheduler.ScheduleJob(job, trigger);
            //         minuteAdd +=10;
            //     }
            // }
            //
            // else if (jobType  == typeof(UpdateCategoryAttributeJob))
            // {
            //     var minuteAdd = 0;
            //     foreach (var categoryItem in n11DeepestCategories)
            //      {
            //          var job = JobBuilder.Create(jobType)
            //              .WithIdentity($"{jobAttribute.IdentityName}_{categoryItem.InternalId}_{jobAttribute.IdentityGroup}", jobAttribute.IdentityGroup)
            //              .UsingJobData("categoryId", categoryItem.InternalId)
            //              .Build();
            //     
            //     
            //     
            //          var trigger = TriggerBuilder.Create()
            //              .WithIdentity($"{jobAttribute.TriggerName}_{categoryItem.InternalId}_{jobAttribute.TriggerGroup}_Trigger", jobAttribute.TriggerGroup)
            //              .ForJob(job)
            //              //.StartNow()
            //              //.WithCronSchedule(jobAttribute.CronSchedule)
            //              .StartAt(DateTime.Now.AddSeconds(minuteAdd))
            //              .WithSchedule(SimpleScheduleBuilder.RepeatMinutelyForever(20))
            //              //.WithCronSchedule(jobAttribute.CronSchedule)
            //              .Build();
            //          scheduler.ScheduleJob(job, trigger);
            //          minuteAdd +=10;
            //      }
            // }
            //else
            //{
            var job = JobBuilder.Create(jobType)
                .WithIdentity($"{jobAttribute.IdentityName}_{jobAttribute.IdentityGroup}", jobAttribute.IdentityGroup)
                .Build();


            var triggerBuilder = TriggerBuilder.Create()
                .WithIdentity($"{jobAttribute.TriggerName}_{jobAttribute.TriggerGroup}_Trigger",
                    jobAttribute.TriggerGroup)
                .ForJob(job)
                .WithCronSchedule(jobAttribute.CronSchedule,
                    x => x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Europe/Istanbul")));
            if (jobAttribute.StartNow)
            {
                triggerBuilder.WithSimpleSchedule(x => x.WithMisfireHandlingInstructionFireNow());
                triggerBuilder.StartAt(DateTimeOffset.Now.AddSeconds(10));
                //triggerBuilder.StartNow();
            }

            var trigger = triggerBuilder.Build();


            scheduler.ScheduleJob(job, trigger);
            
            
            // }
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