using MagicCoffeeMachineV3.Interfaces;
using MagicCoffeeMachineV3.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var containerStateFilePath = builder.Configuration.GetValue<string>("ContainerStateFilePath");
builder.Services.AddSingleton<IPersistenceService>(provider => new PersistenceService(containerStateFilePath!));
builder.Services.AddSingleton<ICoffeeMachineService, CoffeeMachineService>();
builder.Services.AddSingleton<IHeaterService, HeaterService>();
builder.Services.AddSingleton<ICoffeeGrinderService, CoffeeGrinderService>();
builder.Services.AddHttpClient<ICloudService, CloudService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CoffeeMachine}/{action=Index}/{id?}");

app.Run();
