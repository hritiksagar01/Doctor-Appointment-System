using Doctor_Appointment_System.Models;
using Doctor_Appointment_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Doctor_Appointment_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _service;
    public AppointmentsController(IAppointmentService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var item = await _service.GetByIdAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId) =>
        Ok(await _service.GetByUserIdAsync(userId));

    [HttpPost]
    public async Task<IActionResult> Book(Appointment appointment)
    {
        try
        {
            var created = await _service.BookAsync(appointment);
            return CreatedAtAction(nameof(Get), new { id = created.AppointmentId }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
    {
        var updated = await _service.UpdateStatusAsync(id, status);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpPatch("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id) =>
        await _service.CancelAsync(id) ? Ok(new { message = "Cancelled" }) : NotFound();
}

