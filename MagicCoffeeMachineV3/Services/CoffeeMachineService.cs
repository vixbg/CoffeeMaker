namespace MagicCoffeeMachineV3.Services
{
    using MagicCoffeeMachineV3.Enums;
    using MagicCoffeeMachineV3.Interfaces;
    using System.ComponentModel;
    using System.Reflection;
    using Container = Models.Container;

    public class CoffeeMachineService : ICoffeeMachineService
    {
        public IPersistenceService PersistenceService;
        public IHeaterService HeaterService;
        public ICoffeeGrinderService CoffeeGrinderService;
        public ICloudService CloudService;
        private CoffeeMachineStatus Status;
        private Queue<string> MessageQueue = new Queue<string>();
        private readonly int MilkPortion = 1;
        private readonly int BrewingTimeMiliseconds = 2000;
        private readonly int TunrOffTimeMiliseconds = 1500;
        private readonly int MaxMilkAmount = 5;
        private readonly int MaxBeansAmount = 10;

        public CoffeeMachineService(IPersistenceService persistenceService, IHeaterService heaterService, ICoffeeGrinderService coffeeGrinderService, ICloudService cloudService)
        {
            CoffeeGrinderService = coffeeGrinderService;
            HeaterService = heaterService;
            this.PersistenceService = persistenceService;
            Status = CoffeeMachineStatus.Off;
            CloudService = cloudService;
        }

        public async Task PowerOnOff()
        {
            if (Status == CoffeeMachineStatus.Off)
            {
                await TurnOn();
            }
            else
            {
                await TurnOff();
            }
        }

        public async Task TurnOn()
        {
            if (Status != CoffeeMachineStatus.StandBy)
            {
                MessageQueue.Enqueue("Machine is turning ON...");
            }

            if (!HeaterService.IsWaterHeated())
            {
                MessageQueue.Enqueue("Machine is preheating water.");
                await HeaterService.HeaterOnAsync();
            }

            Status = CoffeeMachineStatus.On;
            MessageQueue.Enqueue("Machine is ON and READY.");
        }

        public async Task TurnOff()
        {
            Status = CoffeeMachineStatus.Off;
            MessageQueue.Enqueue("Machine is turning OFF...");
            await Task.Delay(TunrOffTimeMiliseconds);
            MessageQueue.Enqueue("Machine is OFF");
        }

        public void StandBy()
        {
            Status = CoffeeMachineStatus.StandBy;
            MessageQueue.Enqueue("Machine is on STANDBY.");
        }

        public async Task MakeCoffee(BeverageType beverageType)
        {
            if (Status == CoffeeMachineStatus.StandBy)
            {
                await TurnOn();
            }
            else if (Status != CoffeeMachineStatus.On)
            {
                MessageQueue.Enqueue("Machine is not ON. Please turn on the machine first.");
                return;
            }

            var container = PersistenceService.GetContainer();
            if (container.BeansAmount < 1)
            {
                MessageQueue.Enqueue("Not enough beans to make coffee. Please refill beans.");
                await CloudService.NotifyMaintenanceNeededAsync();
                return;
            }

            if (beverageType == BeverageType.CoffeeWithMilk && container.MilkAmount < 1)
            {
                MessageQueue.Enqueue("Not enough milk to make coffee with milk. Please refill milk.");
                await CloudService.NotifyMilkRefillNeededAsync();
                return;
            }

            if (!HeaterService.IsWaterHeated())
            {
                MessageQueue.Enqueue("Heating water...");
                await HeaterService.HeaterOnAsync();
            }

            MessageQueue.Enqueue("Grinding beans...");
            var changedContainer = await CoffeeGrinderService.GrindBeans(container);

            if (beverageType == BeverageType.CoffeeWithMilk)
            {
                changedContainer.MilkAmount -= MilkPortion;
            }

            MessageQueue.Enqueue("Preparing your drink...");
            await Task.Delay(BrewingTimeMiliseconds);
            PersistenceService.UpdateContainer(changedContainer);

            MessageQueue.Enqueue($"Your {GetEnumDescription(beverageType)} is ready!");
            StandBy();
        }

        public Container GetContainerStatus()
        {
            return PersistenceService.GetContainer();
        }

        public void RefillContainer(string containerType)
        {
            var container = PersistenceService.GetContainer();
            switch (containerType.ToLower())
            {
                case "beans":
                    container.BeansAmount = MaxBeansAmount;
                    MessageQueue.Enqueue("Beans container refilled.");
                    break;
                case "milk":
                    container.MilkAmount = MaxMilkAmount;
                    MessageQueue.Enqueue("Milk container refilled.");
                    break;
                default:
                    MessageQueue.Enqueue("Invalid container type.");
                    break;
            }
            PersistenceService.UpdateContainer(container);
        }

        public IEnumerable<string> RetrieveMessages()
        {
            List<string> messages = new List<string>();
            while (MessageQueue.Count > 0)
            {
                messages.Add(MessageQueue.Dequeue());
            }
            return messages;
        }

        private static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString())!;

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
