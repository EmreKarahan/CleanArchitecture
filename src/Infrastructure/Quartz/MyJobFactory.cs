using Quartz.Spi;

namespace Infrastructure.Quartz;

public class MyJobFactory : IJobFactory
{
    private readonly IServiceProvider _serviceProvider;

    public MyJobFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        return ActivatorUtilities.CreateInstance(_serviceProvider, bundle.JobDetail.JobType) as IJob;
    }
    
    public void ReturnJob(IJob job)
    {
        if (job is IDisposable disposableJob)
            disposableJob.Dispose();
    }
}