namespace MagicCoffeeMachineV3.Services
{
    using MagicCoffeeMachineV3.Interfaces;
    using MagicCoffeeMachineV3.Models;
    using Newtonsoft.Json;

    public class PersistenceService : IPersistenceService
    {
        private readonly string _filePath;
        private readonly int _maxMilkAmount = 5;
        private readonly int _maxBeansAmount = 10;

        public PersistenceService(string containerStateFilePath)
        {
            _filePath = containerStateFilePath;
        }

        public Container GetContainer()
        {
            if (!File.Exists(_filePath))
            {
                var initialState = new Container { MilkAmount = _maxMilkAmount, BeansAmount = _maxBeansAmount };
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(initialState));
                return initialState;
            }

            var json = File.ReadAllText(_filePath);
            var containerState = JsonConvert.DeserializeObject<Container>(json);
            return containerState!;
        }

        public bool UpdateContainer(Container containerState)
        {
            try
            {
                var json = JsonConvert.SerializeObject(containerState);
                File.WriteAllText(_filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating container state. {ex}");
                return false;
            }
        }
    }
}
