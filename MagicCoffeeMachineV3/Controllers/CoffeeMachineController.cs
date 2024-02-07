namespace MagicCoffeeMachineV3.Controllers
{
    using MagicCoffeeMachineV3.Enums;
    using MagicCoffeeMachineV3.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class CoffeeMachineController : Controller
    {
        private readonly ICoffeeMachineService _coffeeMachineService;

        public CoffeeMachineController(ICoffeeMachineService coffeeMachineService)
        {
            _coffeeMachineService = coffeeMachineService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetContainerStatus()
        {
            var containerStatus = _coffeeMachineService.GetContainerStatus();
            return Json(containerStatus);
        }

        [HttpGet]
        public IActionResult GetMachineMessages()
        {
            var messages = _coffeeMachineService.RetrieveMessages();
            return Json(messages);
        }

        [HttpPost]
        public async Task<IActionResult> PowerOnOff()
        {
            await _coffeeMachineService.PowerOnOff();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> MakeCoffee(BeverageType type)
        {
            await _coffeeMachineService.MakeCoffee(type);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult RefillContainer(string containerType)
        {
            _coffeeMachineService.RefillContainer(containerType);
            return Ok();
        }
    }
}
