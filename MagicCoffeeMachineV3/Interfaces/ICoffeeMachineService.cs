namespace MagicCoffeeMachineV3.Interfaces
{
    using MagicCoffeeMachineV3.Enums;

    public interface ICoffeeMachineService
    {
        void TurnOn();

        void TurnOff();

        void StandBy();

        void RefillMilk();

        void RefillBeans();

        Task MakeCoffee(BeverageType beverageType);
    }
}
