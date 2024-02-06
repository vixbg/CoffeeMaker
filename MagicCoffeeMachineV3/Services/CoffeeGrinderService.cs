namespace MagicCoffeeMachineV3.Services
{
    using MagicCoffeeMachineV3.Interfaces;
    using MagicCoffeeMachineV3.Models;

    public class CoffeeGrinderService : ICoffeeGrinderService
    {
        private readonly int _grinderCoffeePortion = 1;

        public CoffeeGrinderService() { }

        public Container GrindBeans(Container container)
        {
            if (container.BeansAmount > 0)
            {
                container.BeansAmount -= _grinderCoffeePortion;
            }

            return container;
        }
    }
}
