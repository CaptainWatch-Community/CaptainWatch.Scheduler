using CaptainWatch.API.ServiceClients;
using CaptainWatch.Scheduler;
using CaptainWatch.Scheduler.Services.Movies;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHangfire((sp, config) =>
{
	config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddHangfireServer();

//Register a common HTTP client
var apiBaseAddress = builder.Configuration["CaptainWatchAPI:BaseAddress"] ?? throw new Exception("Missing configuration key : CaptainWatchAPI:BaseAddress");
var apiAuthorization = builder.Configuration["CaptainWatchAPI:Authorization"] ?? throw new Exception("Missing configuration key : CaptainWatchAPI:Authorization");
builder.Services.AddHttpClient("CaptainWatchAPIClient", client =>
{
	client.BaseAddress = new Uri(apiBaseAddress);
	client.DefaultRequestHeaders.Add("Authorization", apiAuthorization);
});


//Register specific HTTP clients by reusing the common client
builder.Services.AddScoped<IMoviesReadHttpClient>(sp =>
	new MoviesReadHttpClient(sp.GetRequiredService<IHttpClientFactory>().CreateClient("CaptainWatchAPIClient")));

builder.Services.AddScoped<IMoviesWriteHttpClient>(sp =>
	new MoviesWriteHttpClient(sp.GetRequiredService<IHttpClientFactory>().CreateClient("CaptainWatchAPIClient")));


//dependency injection for services
builder.Services.AddScoped<MovieWriteService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
	DashboardTitle = "CaptainWatch.Scheduler (1.0.1)",
});
ConfigRecurringJobs.ConfigureJobs();
app.Run();
