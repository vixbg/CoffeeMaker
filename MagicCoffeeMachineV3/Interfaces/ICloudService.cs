namespace MagicCoffeeMachineV3.Interfaces
{
    public interface ICloudService
    {
        Task NotifyMaintenanceNeededAsync();
        Task NotifyMilkRefillNeededAsync();
    }
}
