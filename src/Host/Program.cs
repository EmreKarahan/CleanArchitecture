using Application;
using Application.Common.Interfaces;
using Domain.Entities.NOnbir;
using Host;
using Infrastructure;
using Infrastructure.Persistence;
using Infrastructure.Quartz;
using Minima.MarketPlace.NOnbir;
using Minima.Trendyol.Client;
using Minima.Trendyol.Client.Constants;
using Quartz.Logging;

var builder = WebApplication.CreateBuilder(args);

LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebUIServices();

builder.Services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));

// builder.Services.AddHangfire(config =>
//     config.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("HangfireConnection")));

var settings = builder.Configuration.GetSection("TrendyolSettings").Get<TrendyolSettings>();
builder.Services.AddTrendyolClient(settings);

NOnbirSettings nOnbirSettings = builder.Configuration.GetSection("NOnbirSetting").Get<NOnbirSettings>();
builder.Services.AddNOnbirClient(nOnbirSettings);

//builder.Services.AddHangfireServer();
builder.Services.AddScheduledJob(builder.Configuration);
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();

    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwaggerUi3(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});

app.UseRouting();
TimeZoneInfo.FindSystemTimeZoneById("Europe/Istanbul");
app.UseScheduledJob();
//app.UseHangfireDashboard();


// BackgroundJob.Enqueue<CleanArchitecture.Infrastructure.NOnbir.UpdateTopCategoryJob>(emailJob =>
//     emailJob.UpdateTopCategories());


using (var scope = app.Services.CreateScope())
{
    var categoryRepository = scope.ServiceProvider.GetService<IRepository<Category>>();

    var categories = categoryRepository.GetAllBy(p => p.ParentId is null).ToList();
    // foreach (Category item in categories)
    // {
     // BackgroundJob.Enqueue<UpdateSubCategoryJob>(emailJob =>
     //     emailJob.GetSubCategories(1000210));
    //Thread.Sleep(1000);
    //}

    //var deepestCategories = categoryRepository.GetAllBy(p => p.IsDeepest).ToList();

    // foreach (Category attribute in deepestCategories)
    // {
    //     BackgroundJob.Enqueue<CleanArchitecture.Infrastructure.NOnbir.UpdateCategoryAttributeJob>(emailJob =>
    //         emailJob.GetCategoryAttributes(attribute.InternalId));
    //     //Thread.Sleep(1000);
    // }
}


app.Run();