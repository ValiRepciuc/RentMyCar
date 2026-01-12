using Domain.Entities;
using Infrastructure.DTOs.Car;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/car")]
[ApiController]
public class CarController : ControllerBase
{
    private readonly ICarService _carService;
    public CarController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] string? city = null,
        [FromQuery] int? minPrice = null,
        [FromQuery] int? maxPrice = null,
        [FromQuery] string? brand = null,
        [FromQuery] string? model = null)
    {
        var cars = await _carService.GetAllAsync(city, minPrice, maxPrice, brand, model);
        return Ok(cars);
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var car = await _carService.GetByIdAsync(id);
        return Ok(car);
    }

    [Authorize(Roles = "Owner")]
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] CreateCarDTO carDto)
    {
        var createdcar = await _carService.CreateAsync(carDto);
        return Ok(createdcar);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateCarDTO carDto)
    {
        var createdcar = await _carService.UpdateAsync(id, carDto);
        return Ok(createdcar);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await _carService.DeleteAsync(id);
        return Ok();
    }
}