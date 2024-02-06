namespace MagicCoffeeMachineV3.Interfaces
{
    using MagicCoffeeMachineV3.Models;

    public interface ICoffeeGrinderService
    {
        Container GrindBeans(Container containerState);
    }
}
