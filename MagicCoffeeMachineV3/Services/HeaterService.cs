namespace MagicCoffeeMachineV3.Services
{
    using MagicCoffeeMachineV3.Interfaces;

    public class HeaterService : IHeaterService
    {
        private DateTime? LastHeatedTime = null;
        private readonly int HeatingTimeMilliseconds = 2000;
        private readonly int HeatingThresholdSeconds = 15;

        public HeaterService() { }

        public async Task HeaterOnAsync()
        {
            LastHeatedTime = DateTime.Now;
            await Task.Delay(HeatingTimeMilliseconds);

            return;
        }

        public bool IsWaterHeated()
        {
            if (LastHeatedTime.HasValue)
            {
                return (DateTime.Now - LastHeatedTime.Value).TotalSeconds <= HeatingThresholdSeconds;
            }
            return false;
        }
    }

}
