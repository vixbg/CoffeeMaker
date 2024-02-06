namespace MagicCoffeeMachineV3.Interfaces
{
    using MagicCoffeeMachineV3.Models;

    public interface IPersistenceService
    {
        Container GetContainerState();

        bool UpdateContainerState(Container containerState);
    }
}
