namespace MagicCoffeeMachineV3.Services
{
    using MagicCoffeeMachineV3.Enums;
    using MagicCoffeeMachineV3.Interfaces;

    public class CoffeeMachineService : ICoffeeMachineService
    {
        public IPersistenceService _persistenceService;
        public IHeaterService _heaterService;
        public ICoffeeGrinderService _coffeeGrinderService;
        private CoffeeMachineStatus _status;
        private readonly int _milkPortion = 1;

        public CoffeeMachineService(IPersistenceService persistenceService, IHeaterService heaterService, ICoffeeGrinderService coffeeGrinderService)
        {
            _coffeeGrinderService = coffeeGrinderService;
            _heaterService = heaterService;
            _persistenceService = persistenceService;
            _status = CoffeeMachineStatus.Off;
        }

        public async void TurnOn()
        {
            Console.WriteLine("Machine is turning ON...");

            if (!_heaterService.IsWaterHeated())
            {
                Console.WriteLine("Machine is preheating water.");
                await _heaterService.HeaterOnAsync();
            }

            _status = CoffeeMachineStatus.On;
            Console.WriteLine("Machine is ON and READY.");
        }

        public void TurnOff()
        {
            _status = CoffeeMachineStatus.Off;
            Console.WriteLine("Machine is OFF.");
        }

        public void StandBy()
        {
            _status = CoffeeMachineStatus.StandBy;
            Console.WriteLine("Machine is on STANDBY.");
        }

        public async Task MakeCoffee(BeverageType beverageType)
        {
            if (_status != CoffeeMachineStatus.On)
            {
                Console.WriteLine("Machine is not ON. Please turn on the machine first.");
                return;
            }
            else if (_status == CoffeeMachineStatus.StandBy)
            {
                TurnOn();
            }

            var container = _persistenceService.GetContainerState();
            if (container.BeansAmount < 1)
            {
                // TODO: Order from Cloud
                Console.WriteLine("Not enough beans to make coffee. Please refill beans.");
                return;
            }

            if (beverageType == BeverageType.CoffeeWithMilk && container.MilkAmount < 1)
            {
                // TODO: Order from Cloud
                Console.WriteLine("Not enough milk to make coffee with milk. Please refill milk.");
                return;
            }

            var isWaterHeated = _heaterService.IsWaterHeated();

            if (!isWaterHeated)
            {
                Console.WriteLine("Heating water...");
                await _heaterService.HeaterOnAsync();
            }            

            Console.WriteLine("Grinding beans...");
            container = _coffeeGrinderService.GrindBeans(container);

            if (beverageType == BeverageType.CoffeeWithMilk)
            {
                container.MilkAmount -= _milkPortion;
            }

            _persistenceService.UpdateContainerState(container);

            Console.WriteLine($"Your {beverageType} is ready!");
            StandBy();
        }


        public void RefillBeans()
        {
            throw new NotImplementedException();
        }

        public void RefillMilk()
        {
            throw new NotImplementedException();
        }

    }
}
