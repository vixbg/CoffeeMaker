namespace MagicCoffeeMachineV3.Tests.Tests
{
    using MagicCoffeeMachineV3.Models;
    using MagicCoffeeMachineV3.Services;
    using System.Threading.Tasks;
    using Xunit;

    public class CoffeeGrinderServiceTests
    {        
        [Fact]
        public async Task GrindBeans_WhenBeansAmountIsZero_ReturnsContainerWithZeroBeans()
        {
            // Arrange
            var container = new Container { BeansAmount = 0 };
            var coffeeGrinderService = new CoffeeGrinderService();

            // Act
            var result = await coffeeGrinderService.GrindBeans(container);

            // Assert
            Assert.Equal(0, result.BeansAmount);
        }

        [Fact]
        public async Task GrindBeans_WhenBeansAmountIsOne_ReturnsContainerWithZeroBeans()
        {
            // Arrange
            var container = new Container { BeansAmount = 1 };
            var coffeeGrinderService = new CoffeeGrinderService();

            // Act
            var result = await coffeeGrinderService.GrindBeans(container);

            // Assert
            Assert.Equal(0, result.BeansAmount);
        }
    }
}
