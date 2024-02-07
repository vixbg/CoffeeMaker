namespace MagicCoffeeMachineV3Cloud.Services
{
    using MagicCoffeeMachineV3Cloud.Interfaces;

    public class NotificationService : INotificationService
    {
        public void NotifyMaintenanceNeeded(string message)
        {
            Console.WriteLine($"Maintenance notification received: {message}");
        }

        public void NotifyMilkRefillNeeded()
        {
            Console.WriteLine("Milk refill notification received.");
        }
    }
}
