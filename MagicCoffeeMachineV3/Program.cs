using MagicCoffeeMachineV3.Interfaces;
using MagicCoffeeMachineV3.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var containerStateFilePath = builder.Configuration.GetValue<string>("ContainerStateFilePath");
builder.Services.AddSingleton<IPersistenceService>(provider => new PersistenceService(containerStateFilePath!));
builder.Services.AddSingleton<ICoffeeMachineService, CoffeeMachineService>();
builder.Services.AddSingleton<IHeaterService, HeaterService>();
builder.Services.AddSingleton<ICoffeeGrinderService, CoffeeGrinderService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
