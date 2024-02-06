namespace MagicCoffeeMachineV3.Interfaces
{
    using MagicCoffeeMachineV3.Models;

    public interface IPersistenceService
    {
        Container GetContainer();

        bool UpdateContainer(Container containerState);
    }
}
