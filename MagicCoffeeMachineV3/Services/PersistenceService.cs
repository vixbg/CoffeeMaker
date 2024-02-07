namespace MagicCoffeeMachineV3.Services
{
    using MagicCoffeeMachineV3.Interfaces;
    using MagicCoffeeMachineV3.Models;
    using Newtonsoft.Json;

    public class PersistenceService : IPersistenceService
    {
        private readonly string FilePath;
        private readonly int MaxMilkAmount = 5;
        private readonly int MaxBeansAmount = 10;

        public PersistenceService(string containerStateFilePath)
        {
            FilePath = containerStateFilePath;
        }

        public Container GetContainer()
        {
            if (!File.Exists(FilePath))
            {
                var initialState = new Container { MilkAmount = MaxMilkAmount, BeansAmount = MaxBeansAmount };
                File.WriteAllText(FilePath, JsonConvert.SerializeObject(initialState));
                return initialState;
            }

            var json = File.ReadAllText(FilePath);
            var container = JsonConvert.DeserializeObject<Container>(json);
            return container!;
        }

        public bool UpdateContainer(Container containerState)
        {
            try
            {
                var json = JsonConvert.SerializeObject(containerState);
                File.WriteAllText(FilePath, json);
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
