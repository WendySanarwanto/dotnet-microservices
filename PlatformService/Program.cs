using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

// NOTE: Comment these lines before running : dotnet-ef migrations command
// ---------------------
if (builder.Environment.IsDevelopment()){
    Console.WriteLine("--> Generating swagger doc ...");
    builder.Services.AddSwaggerGen();
    Console.WriteLine("--> Using In-Memory database ...");
    builder.Services.AddDbContext<AppDbContext>(options => 
        options.UseInMemoryDatabase("InMemDb"));
} else if (builder.Environment.IsProduction()) {
// ---------------------
    Console.WriteLine("--> Using SQL Server database ...");
    builder.Services.AddDbContext<AppDbContext>(
        options =>options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsSqlConnection")) 
    );
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// NOTE: Comment this line before running : dotnet-ef migrations command
PrepareDb.PrepPopulation(app);

Console.WriteLine($"---> CommandService Endpont {builder.Configuration["CommandService"]}");

app.Run();
