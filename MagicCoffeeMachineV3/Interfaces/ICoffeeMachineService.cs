namespace MagicCoffeeMachineV3.Interfaces
{
    using MagicCoffeeMachineV3.Enums;
    using MagicCoffeeMachineV3.Models;

    public interface ICoffeeMachineService
    {
        Task PowerOnOff();

        void StandBy();

        Task MakeCoffee(BeverageType beverageType);

        Container GetContainerStatus();

        void RefillContainer(string containerType);

        IEnumerable<string> RetrieveMessages();
    }
}
