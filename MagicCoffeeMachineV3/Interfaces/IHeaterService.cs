namespace MagicCoffeeMachineV3.Interfaces
{
    public interface IHeaterService
    {
        Task<bool> HeaterOnAsync();

        bool IsWaterHeated();
    }
}
