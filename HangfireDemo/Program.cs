using Hangfire;
using Hangfire.Dashboard;
using Hangfire.MemoryStorage;
using HangfireDemo.Jobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangfire(config =>
    config.UseMemoryStorage());
builder.Services.AddHangfireServer();
builder.Services.AddTransient<LogJob>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Hangfire Demo API",
        Version = "v1"
    });
});

builder.Services.AddControllers();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hangfire Demo v1"));
}


// Dashboard do Hangfire 
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    IgnoreAntiforgeryToken = true,
    Authorization = new[] { new  NoAuthFilter() },
    AsyncAuthorization = new[] { new HangfireDashboardAsyncAuthorizationFilter() }
});

app.MapControllers();
app.Run();

public class HangfireDashboardAsyncAuthorizationFilter : IDashboardAsyncAuthorizationFilter
{
    public Task<bool> AuthorizeAsync(DashboardContext context) => Task.FromResult(true);
}

public class NoAuthFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context) => true;
}
