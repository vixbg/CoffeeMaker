namespace MagicCoffeeMachineV3.Controllers
{
    using MagicCoffeeMachineV3.Enums;
    using MagicCoffeeMachineV3.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class CoffeeMachineController : Controller
    {
        private readonly ICoffeeMachineService CoffeeMachineService;

        public CoffeeMachineController(ICoffeeMachineService coffeeMachineService)
        {
            CoffeeMachineService = coffeeMachineService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetContainerStatus()
        {
            var containerStatus = CoffeeMachineService.GetContainerStatus();
            return Json(containerStatus);
        }

        [HttpGet]
        public IActionResult GetMachineMessages()
        {
            var messages = CoffeeMachineService.RetrieveMessages();
            return Json(messages);
        }

        [HttpPost]
        public async Task<IActionResult> PowerOnOff()
        {
            await CoffeeMachineService.PowerOnOff();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> MakeCoffee(BeverageType type)
        {
            await CoffeeMachineService.MakeCoffee(type);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult RefillContainer(string containerType)
        {
            CoffeeMachineService.RefillContainer(containerType);
            return Ok();
        }
    }
}
