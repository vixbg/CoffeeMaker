using MagicCoffeeMachineV3Cloud.Interfaces;
using MagicCoffeeMachineV3Cloud.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService NotificationService;

    public NotificationsController(INotificationService notificationService)
    {
        NotificationService = notificationService;
    }

    [HttpPost("maintenance")]
    public IActionResult NotifyMaintenance([FromBody] MaintenanceRequest request)
    {
        NotificationService.NotifyMaintenanceNeeded(request.Message);
        return Ok();
    }

    [HttpPost("milk-refill")]
    public IActionResult NotifyMilkRefill()
    {
        NotificationService.NotifyMilkRefillNeeded();
        return Ok();
    }
}