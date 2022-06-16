using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseInMemoryDatabase("InMemDb")
);
// builder.Services.AddDbContext<AppDbContext>(
//     options =>options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")) );

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

if (builder.Environment.IsDevelopment()){
    Console.WriteLine("--> Generating swagger doc ...");
    builder.Services.AddSwaggerGen();
}
// builder.Services.AddSwaggerGen();

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

PrepareDb.PrepPopulation(app);

Console.WriteLine($"---> CommandService Endpont {builder.Configuration["CommandService"]}");

app.Run();
