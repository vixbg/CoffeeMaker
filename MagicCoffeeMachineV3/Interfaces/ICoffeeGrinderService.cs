namespace MagicCoffeeMachineV3.Interfaces
{
    using MagicCoffeeMachineV3.Models;

    public interface ICoffeeGrinderService
    {
        Task<Container> GrindBeans(Container containerState);
    }
}
