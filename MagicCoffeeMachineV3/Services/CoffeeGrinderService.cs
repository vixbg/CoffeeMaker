namespace MagicCoffeeMachineV3.Services
{
    using MagicCoffeeMachineV3.Interfaces;
    using MagicCoffeeMachineV3.Models;

    public class CoffeeGrinderService : ICoffeeGrinderService
    {
        private readonly int GrinderCoffeePortion = 1;
        private readonly int GrindingTimeMiliseconds = 2000;

        public CoffeeGrinderService() { }

        public async Task<Container> GrindBeans(Container container)
        {
            if (container.BeansAmount > 0)
            {
                await Task.Delay(GrindingTimeMiliseconds);
                container.BeansAmount -= GrinderCoffeePortion;
            }


            return container;
        }
    }
}
