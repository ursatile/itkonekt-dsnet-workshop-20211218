using System;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;

namespace Autobarn.Website.Controllers.api {
    [Route("api/[controller]")]
    [ApiController]
    public class ModelsController : ControllerBase {
        private readonly IAutobarnDatabase db;
        private readonly IBus bus;

        public ModelsController(IAutobarnDatabase db, IBus bus) {
            this.db = db;
            this.bus = bus;
        }

        [HttpGet]
        public IEnumerable<Model> Get() {
            return db.ListModels();
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id) {
            var vehicleModel = db.FindModel(id);
            if (vehicleModel == default) return NotFound();
            var result = vehicleModel.ToDynamic();
            result._actions = new {
                create = new {
                    href = $"/api/models/{id}",
                    method = "POST",
                    type = "application/json",
                    name = $"Create a new {vehicleModel.Manufacturer.Name} {vehicleModel.Name}"
                }
            };
            return Ok(result);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Post(string id, [FromBody] VehicleDto dto) {
            var existingVehicle = db.FindVehicle(dto.Registration);
            if (existingVehicle != default) return Conflict($"Sorry - vehicle with registration {dto.Registration} is already listed in our database!");

            var vehicleModel = db.FindModel(id);

            var vehicle = new Vehicle {
                Registration = dto.Registration,
                Color = dto.Color,
                Year = dto.Year,
                VehicleModel = vehicleModel
            };
            db.CreateVehicle(vehicle);
            await PublishNewVehicleMessage(vehicle);
            return Created($"/api/vehicles/{vehicle.Registration}", vehicle.ToResource());
        }

        private async Task PublishNewVehicleMessage(Vehicle vehicle) {
            var m = new NewVehicleMessage {
                Registration = vehicle.Registration,
                ModelName = vehicle.VehicleModel?.Name ?? "MODEL_NAME_NOT_FOUND",
                ManufacturerName = vehicle.VehicleModel?.Manufacturer?.Name ?? "MANUFACTURER_NOT_FOUND",
                Color = vehicle.Color,
                Year = vehicle.Year,
                ListedAt = DateTimeOffset.UtcNow
            };
            await bus.PubSub.PublishAsync(m);
        }
    }
}