namespace MagicCoffeeMachineV3.Interfaces
{
    public interface IHeaterService
    {
        Task HeaterOnAsync();

        bool IsWaterHeated();
    }
}
