namespace MagicCoffeeMachineV3.Services
{
    using MagicCoffeeMachineV3.Interfaces;

    public class HeaterService : IHeaterService
    {        
        private DateTime? _lastHeatedTime = null;
        private readonly int _heatingTimeMilliseconds = 2000;
        private readonly int _heatingThresholdSeconds = 5;

        public HeaterService() { }

        public async Task<bool> HeaterOnAsync()
        {            
            _lastHeatedTime = DateTime.Now;
            await Task.Delay(_heatingTimeMilliseconds);                                  
            
            return true;
        }

        public bool IsWaterHeated()
        {
            if (_lastHeatedTime.HasValue)
            {                
                return (DateTime.Now - _lastHeatedTime.Value).TotalSeconds <= _heatingThresholdSeconds;
            }
            return false;
        }
    }

}
