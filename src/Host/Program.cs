using Application;
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


var settings = builder.Configuration.GetSection("TrendyolSettings").Get<TrendyolSettings>();
builder.Services.AddTrendyolClient(settings);

NOnbirSettings? nOnbirSettings = builder.Configuration.GetSection("NOnbirSetting").Get<NOnbirSettings>();
builder.Services.AddNOnbirClient(nOnbirSettings);

var app = builder.Build();

app.UseInfrastructureServices();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();

    // Initialise and seed database
    using var scope = app.Services.CreateScope();
    var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
    await initializer.InitialiseAsync();
    await initializer.SeedAsync();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseScheduledJob();

app.UseSwaggerUi3(settings =>
{
});


app.Run();