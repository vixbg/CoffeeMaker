namespace MagicCoffeeMachineV3.Tests.Tests
{
    using MagicCoffeeMachineV3.Interfaces;
    using MagicCoffeeMachineV3.Models;
    using MagicCoffeeMachineV3.Services;
    using Moq;
    using Xunit;

    public class PersistenceServiceTests
    {

        [Fact]
        public void GetContainer_WhenFileDoesNotExist_ReturnsInitialState()
        {
            // Arrange
            var filePath = "testNewState.json";
            var persistenceService = new PersistenceService(filePath);

            // Act
            var container = persistenceService.GetContainer();

            // Assert
            Assert.Equal(5, container.MilkAmount);
            Assert.Equal(10, container.BeansAmount);
            File.Delete(filePath);
        }

        [Fact]
        public void GetContainer_WhenFileExists_ReturnsContainerState()
        {
            // Arrange
            var filePath = "test.json";
            var persistenceService = new PersistenceService(filePath);
            var container = new Container { MilkAmount = 3, BeansAmount = 7 };
            persistenceService.UpdateContainer(container);

            // Act
            var result = persistenceService.GetContainer();

            // Assert
            Assert.Equal(container.MilkAmount, result.MilkAmount);
            Assert.Equal(container.BeansAmount, result.BeansAmount);
        }

        [Fact]
        public void UpdateContainer_WhenFileExists_UpdatesContainerState()
        {
            // Arrange
            var filePath = "test.json";
            var persistenceService = new PersistenceService(filePath);
            var container = new Container { MilkAmount = 3, BeansAmount = 7 };
            persistenceService.UpdateContainer(container);

            // Act
            var newContainer = new Container { MilkAmount = 4, BeansAmount = 8 };
            persistenceService.UpdateContainer(newContainer);
            var result = persistenceService.GetContainer();

            // Assert
            Assert.Equal(newContainer.MilkAmount, result.MilkAmount);
            Assert.Equal(newContainer.BeansAmount, result.BeansAmount);
        }
    }
}
