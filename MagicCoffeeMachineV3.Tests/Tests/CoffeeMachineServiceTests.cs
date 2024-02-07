namespace MagicCoffeeMachineV3.Tests
{
    using MagicCoffeeMachineV3.Enums;
    using MagicCoffeeMachineV3.Interfaces;
    using MagicCoffeeMachineV3.Models;
    using MagicCoffeeMachineV3.Services;
    using Moq;
    using Xunit;

    public class CoffeeMachineServiceTests
    {
        private readonly CoffeeMachineService CoffeeMachineService;
        private readonly Mock<IPersistenceService> MockPersistenceService = new Mock<IPersistenceService>();
        private readonly Mock<IHeaterService> MockHeaterService = new Mock<IHeaterService>();
        private readonly Mock<ICoffeeGrinderService> MockCoffeeGrinderService = new Mock<ICoffeeGrinderService>();
        private readonly Mock<ICloudService> MockCloudService = new Mock<ICloudService>();

        public CoffeeMachineServiceTests()
        {
            CoffeeMachineService = new CoffeeMachineService(MockPersistenceService.Object, MockHeaterService.Object, MockCoffeeGrinderService.Object, MockCloudService.Object);
        }

        [Fact]
        public async Task MakeCoffee_NotEnoughBeans_SendsMaintenanceNotification()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.On;
            var container = new Container { BeansAmount = 0, MilkAmount = 5 };
            MockPersistenceService.Setup(s => s.GetContainer()).Returns(container);

            // Act
            await CoffeeMachineService.MakeCoffee(BeverageType.BlackCoffee);

            // Assert
            MockCloudService.Verify(s => s.NotifyMaintenanceNeededAsync(), Times.Once);

        }

        [Fact]
        public async Task MakeCoffee_NotEnoughMilk_SendsMilkRefillNotification()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.On;
            var container = new Container { BeansAmount = 10, MilkAmount = 0 };
            MockPersistenceService.Setup(s => s.GetContainer()).Returns(container);

            // Act
            await CoffeeMachineService.MakeCoffee(BeverageType.CoffeeWithMilk);

            // Assert
            MockCloudService.Verify(s => s.NotifyMilkRefillNeededAsync(), Times.Once);

        }
        
        [Fact]
        public async Task TurnOn_WhenMachineIsOffAndWaterIsNotHeated_PreheatsWater()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.Off;
            MockHeaterService.Setup(s => s.IsWaterHeated()).Returns(false);

            // Act
            await CoffeeMachineService.TurnOn();

            // Assert
            MockHeaterService.Verify(s => s.HeaterOnAsync(), Times.Once);
        }
        
        [Fact]
        public void StandBy_WhenMachineIsOn_ChangesStatusToStandBy()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.On;

            // Act
            CoffeeMachineService.StandBy();

            // Assert
            Assert.Equal(CoffeeMachineStatus.StandBy, CoffeeMachineService.Status);
        }
                
        [Fact]
        public async Task TurnOff_WhenMachineIsOn_ChangesStatusToOff()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.On;

            // Act
            await CoffeeMachineService.TurnOff();

            // Assert
            Assert.Equal(CoffeeMachineStatus.Off, CoffeeMachineService.Status);
        }
        
        [Fact]
        public async Task PowerOnOff_WhenMachineIsOff_TurnsOn()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.Off;

            // Act
            await CoffeeMachineService.PowerOnOff();

            // Assert
            Assert.Equal(CoffeeMachineStatus.On, CoffeeMachineService.Status);
        }

        [Fact]
        public async Task PowerOnOff_WhenMachineIsOn_TurnsOff()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.On;

            // Act
            await CoffeeMachineService.PowerOnOff();

            // Assert
            Assert.Equal(CoffeeMachineStatus.Off, CoffeeMachineService.Status);
        }
                
        [Fact]
        public async Task MakeCoffee_WhenMachineIsOnAndEnoughBeansAndMilk_MakesCoffee()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.On;
            var container = new Container { BeansAmount = 10, MilkAmount = 10 };
            MockPersistenceService.Setup(s => s.GetContainer()).Returns(container);
            MockCoffeeGrinderService.Setup(s => s.GrindBeans(container)).ReturnsAsync(container);

            // Act
            await CoffeeMachineService.MakeCoffee(BeverageType.CoffeeWithMilk);

            // Assert
            MockCoffeeGrinderService.Verify(s => s.GrindBeans(container), Times.Once);
            MockHeaterService.Verify(s => s.HeaterOnAsync(), Times.Once);
            MockPersistenceService.Verify(s => s.UpdateContainer(It.IsAny<Container>()), Times.Once);
        }

        [Fact]
        public async Task MakeCoffee_WhenMachineIsStandBy_TurnsOn()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.StandBy;
            var container = new Container { BeansAmount = 10, MilkAmount = 10 };
            MockPersistenceService.Setup(s => s.GetContainer()).Returns(container);
            MockCoffeeGrinderService.Setup(s => s.GrindBeans(container)).ReturnsAsync(container);

            // Act
            await CoffeeMachineService.MakeCoffee(BeverageType.CoffeeWithMilk);

            // Assert
            MockPersistenceService.Verify(s => s.UpdateContainer(It.IsAny<Container>()), Times.Once);
        }        

        [Fact]
        public async Task MakeCoffee_WhenMachineIsOnAndNotEnoughBeans_DoesNotMakeCoffee()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.On;
            var container = new Container { BeansAmount = 0, MilkAmount = 10 };
            MockPersistenceService.Setup(s => s.GetContainer()).Returns(container);

            // Act
            await CoffeeMachineService.MakeCoffee(BeverageType.CoffeeWithMilk);

            // Assert
            MockCoffeeGrinderService.Verify(s => s.GrindBeans(container), Times.Never);
            MockHeaterService.Verify(s => s.HeaterOnAsync(), Times.Never);
            MockPersistenceService.Verify(s => s.UpdateContainer(It.IsAny<Container>()), Times.Never);
        }

        [Fact]
        public async Task MakeCoffee_WhenMachineIsOnAndNotEnoughMilk_DoesNotMakeCoffee()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.On;
            var container = new Container { BeansAmount = 10, MilkAmount = 0 };
            MockPersistenceService.Setup(s => s.GetContainer()).Returns(container);

            // Act
            await CoffeeMachineService.MakeCoffee(BeverageType.CoffeeWithMilk);

            // Assert
            MockCoffeeGrinderService.Verify(s => s.GrindBeans(container), Times.Never);
            MockHeaterService.Verify(s => s.HeaterOnAsync(), Times.Never);
            MockPersistenceService.Verify(s => s.UpdateContainer(It.IsAny<Container>()), Times.Never);
        }

        [Fact]
        public async Task MakeCoffee_WhenMachineIsOnAndNotEnoughMilk_SendsMilkRefillNotification()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.On;
            var container = new Container { BeansAmount = 10, MilkAmount = 0 };
            MockPersistenceService.Setup(s => s.GetContainer()).Returns(container);

            // Act
            await CoffeeMachineService.MakeCoffee(BeverageType.CoffeeWithMilk);

            // Assert
            MockCloudService.Verify(s => s.NotifyMilkRefillNeededAsync(), Times.Once);
        }

        [Fact]
        public async Task MakeCoffee_WhenMachineIsOnAndNotEnoughBeans_SendsMaintenanceNotification()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.On;
            var container = new Container { BeansAmount = 0, MilkAmount = 10 };
            MockPersistenceService.Setup(s => s.GetContainer()).Returns(container);

            // Act
            await CoffeeMachineService.MakeCoffee(BeverageType.CoffeeWithMilk);

            // Assert
            MockCloudService.Verify(s => s.NotifyMaintenanceNeededAsync(), Times.Once);
        }

        [Fact]
        public async Task MakeCoffee_WhenMachineIsOnAndWaterIsNotHeated_HeatsWater()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.On;
            var container = new Container { BeansAmount = 10, MilkAmount = 10 };
            MockPersistenceService.Setup(s => s.GetContainer()).Returns(container);
            MockHeaterService.Setup(s => s.IsWaterHeated()).Returns(false);
            MockCoffeeGrinderService.Setup(s => s.GrindBeans(container)).ReturnsAsync(container);

            // Act
            await CoffeeMachineService.MakeCoffee(BeverageType.CoffeeWithMilk);

            // Assert
            MockHeaterService.Verify(s => s.HeaterOnAsync(), Times.Once);
        }        

        [Fact]
        public async Task MakeCoffee_WhenMachineIsStandByAndEnoughBeansAndMilk_MakesCoffee()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.StandBy;
            var container = new Container { BeansAmount = 10, MilkAmount = 10 };
            MockPersistenceService.Setup(s => s.GetContainer()).Returns(container);
            MockCoffeeGrinderService.Setup(s => s.GrindBeans(container)).ReturnsAsync(container);

            // Act
            await CoffeeMachineService.MakeCoffee(BeverageType.CoffeeWithMilk);

            // Assert
            MockCoffeeGrinderService.Verify(s => s.GrindBeans(container), Times.Once);
            MockHeaterService.Verify(s => s.HeaterOnAsync(), Times.AtLeastOnce);
            MockPersistenceService.Verify(s => s.UpdateContainer(It.IsAny<Container>()), Times.Once);
        }

        [Fact]
        public async Task MakeCoffee_WhenMachineIsStandByAndNotEnoughBeans_DoesNotMakeCoffee()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.StandBy;
            var container = new Container { BeansAmount = 0, MilkAmount = 10 };
            MockPersistenceService.Setup(s => s.GetContainer()).Returns(container);

            // Act
            await CoffeeMachineService.MakeCoffee(BeverageType.CoffeeWithMilk);

            // Assert
            MockCoffeeGrinderService.Verify(s => s.GrindBeans(container), Times.Never);
            MockHeaterService.Verify(s => s.HeaterOnAsync(), Times.Once);
            MockPersistenceService.Verify(s => s.UpdateContainer(It.IsAny<Container>()), Times.Never);
        }

        [Fact]
        public async Task MakeCoffee_WhenMachineIsStandByAndNotEnoughMilk_DoesNotMakeCoffee()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.StandBy;
            var container = new Container { BeansAmount = 10, MilkAmount = 0 };
            MockPersistenceService.Setup(s => s.GetContainer()).Returns(container);

            // Act
            await CoffeeMachineService.MakeCoffee(BeverageType.CoffeeWithMilk);

            // Assert
            MockCoffeeGrinderService.Verify(s => s.GrindBeans(container), Times.Never);
            MockHeaterService.Verify(s => s.HeaterOnAsync(), Times.Once);
            MockPersistenceService.Verify(s => s.UpdateContainer(It.IsAny<Container>()), Times.Never);
        }

        [Fact]
        public async Task MakeCoffee_WhenMachineIsStandByAndNotEnoughMilk_SendsMilkRefillNotification()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.StandBy;
            var container = new Container { BeansAmount = 10, MilkAmount = 0 };
            MockPersistenceService.Setup(s => s.GetContainer()).Returns(container);

            // Act
            await CoffeeMachineService.MakeCoffee(BeverageType.CoffeeWithMilk);

            // Assert
            MockCloudService.Verify(s => s.NotifyMilkRefillNeededAsync(), Times.Once);
        }

        [Fact]
        public async Task MakeCoffee_WhenMachineIsStandByAndNotEnoughBeans_SendsMaintenanceNotification()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.StandBy;
            var container = new Container { BeansAmount = 0, MilkAmount = 10 };
            MockPersistenceService.Setup(s => s.GetContainer()).Returns(container);

            // Act
            await CoffeeMachineService.MakeCoffee(BeverageType.CoffeeWithMilk);

            // Assert
            MockCloudService.Verify(s => s.NotifyMaintenanceNeededAsync(), Times.Once);
        }

        [Fact]
        public async Task MakeCoffee_WhenMachineIsStandByAndWaterIsNotHeated_HeatsWater()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.StandBy;
            var container = new Container { BeansAmount = 10, MilkAmount = 10 };
            MockPersistenceService.Setup(s => s.GetContainer()).Returns(container);
            MockHeaterService.Setup(s => s.IsWaterHeated()).Returns(false);
            MockCoffeeGrinderService.Setup(s => s.GrindBeans(container)).ReturnsAsync(container);

            // Act
            await CoffeeMachineService.MakeCoffee(BeverageType.CoffeeWithMilk);

            // Assert
            MockHeaterService.Verify(s => s.HeaterOnAsync(), Times.AtLeastOnce);
        }

        [Fact]
        public async Task MakeCoffee_WhenMachineIsOffAndNotEnoughBeans_DoesNotMakeCoffee()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.Off;
            var container = new Container { BeansAmount = 0, MilkAmount = 10 };
            MockPersistenceService.Setup(s => s.GetContainer()).Returns(container);

            // Act
            await CoffeeMachineService.MakeCoffee(BeverageType.CoffeeWithMilk);

            // Assert
            MockCoffeeGrinderService.Verify(s => s.GrindBeans(container), Times.Never);
            MockHeaterService.Verify(s => s.HeaterOnAsync(), Times.Never);
            MockPersistenceService.Verify(s => s.UpdateContainer(It.IsAny<Container>()), Times.Never);
        }

        [Fact]
        public async Task MakeCoffee_WhenMachineIsOffAndNotEnoughMilk_DoesNotMakeCoffee()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.Off;
            var container = new Container { BeansAmount = 10, MilkAmount = 0 };
            MockPersistenceService.Setup(s => s.GetContainer()).Returns(container);

            // Act
            await CoffeeMachineService.MakeCoffee(BeverageType.CoffeeWithMilk);

            // Assert
            MockCoffeeGrinderService.Verify(s => s.GrindBeans(container), Times.Never);
            MockHeaterService.Verify(s => s.HeaterOnAsync(), Times.Never);
            MockPersistenceService.Verify(s => s.UpdateContainer(It.IsAny<Container>()), Times.Never);
        }        

        [Fact]
        public async Task MakeCoffee_WhenMachineIsOffAndWaterIsHeated_DoesNotHeatWater()
        {
            // Arrange
            CoffeeMachineService.Status = CoffeeMachineStatus.Off;
            var container = new Container { BeansAmount = 10, MilkAmount = 10 };
            MockPersistenceService.Setup(s => s.GetContainer()).Returns(container);
            MockHeaterService.Setup(s => s.IsWaterHeated()).Returns(true);

            // Act
            await CoffeeMachineService.MakeCoffee(BeverageType.CoffeeWithMilk);

            // Assert
            MockHeaterService.Verify(s => s.HeaterOnAsync(), Times.Never);
        }       

    }

}
