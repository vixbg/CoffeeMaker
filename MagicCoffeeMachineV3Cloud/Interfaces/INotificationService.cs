namespace MagicCoffeeMachineV3Cloud.Interfaces
{
    public interface INotificationService
    {
        void NotifyMaintenanceNeeded(string message);
        void NotifyMilkRefillNeeded();
    }
}
