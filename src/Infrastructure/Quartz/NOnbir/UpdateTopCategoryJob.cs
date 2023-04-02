using Application.MarketPlaces.N11.Commands;
using Shared.Attributes;
using ICategoryApiService = Minima.MarketPlace.NOnbir.Services.ICategoryApiService;

namespace Infrastructure.Quartz.NOnbir;

[ScheduledJob("UpdateTopCategoryJob", "N11", "UpdateTopCategoryJobTrigger", "N11", "0 /1 * ? * *")]
public class UpdateTopCategoryJob //: IJob, IDisposable
{
    readonly ICategoryApiService _categoryApiService;

    readonly IMediator _mediator;

    // boolean variable to ensure dispose
    // method executes only once
    private bool disposedValue;

    public UpdateTopCategoryJob(
        ICategoryApiService categoryApiService,
        IMediator mediator)
    {
        _categoryApiService = categoryApiService;
        _mediator = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await UpdateTopCategories();
    }

    private async Task UpdateTopCategories()
    {
        var response = _categoryApiService.GetTopLevelCategories();
        if (response.Result.Status != "success")
            return;

        foreach (var topLevelCategory in response.CategoryList)
        {
            if (topLevelCategory is null)
                continue;
            
            var createCategoryCommand = new CreateCategoryCommand
            {
                Name = topLevelCategory.Name, InternalId = topLevelCategory.Id
            };
            await _mediator.Send(createCategoryCommand);
        }
    }


    // Gets called by the below dispose method
    // resource cleaning happens here
    protected virtual void Dispose(bool disposing)
    {
        // check if already disposed
        if (!disposedValue)
        {
            if (disposing)
            {
                // free managed objects here
            }

            // free unmanaged objects here
            Console.WriteLine("The {0} has been disposed", this.GetType().Name);

            // set the bool value to true
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Invoke the above virtual
        // dispose(bool disposing) method
        Dispose(disposing: true);

        // Notify the garbage collector
        // about the cleaning event
        GC.SuppressFinalize(this);
    }

    ~UpdateTopCategoryJob() { Dispose(disposing: false); }
}