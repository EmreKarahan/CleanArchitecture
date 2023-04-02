using Application;
using Host;
using Infrastructure;
using Infrastructure.Quartz;
using Quartz.Logging;

var builder = WebApplication.CreateBuilder(args);

LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebUIServices();

var app = builder.Build();

app.UseInfrastructureServices(app.Environment);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
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