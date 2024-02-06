namespace MagicCoffeeMachineV3.Services
{
    using MagicCoffeeMachineV3.Interfaces;
    using MagicCoffeeMachineV3.Models;

    public class CoffeeGrinderService : ICoffeeGrinderService
    {
        public IPersistenceService _persistenceService;
        private readonly int _grinderCoffeePortion = 1;

        public CoffeeGrinderService(IPersistenceService persistenceService)
        {
            _persistenceService = persistenceService;
        }

        public Container GrindBeans(Container containerState)
        {
            if (containerState.BeansAmount > 0)
            {
                containerState.BeansAmount -= _grinderCoffeePortion;                
            }
            return containerState;
        }
    }
}
