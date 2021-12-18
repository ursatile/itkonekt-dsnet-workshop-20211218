using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;

namespace Autobarn.Website.Controllers.api {
    [Route("api/[controller]")]
    [ApiController]
    public class ModelsController : ControllerBase {
        private readonly IAutobarnDatabase db;

        public ModelsController(IAutobarnDatabase db) {
            this.db = db;
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
        public IActionResult Post(string id, [FromBody] VehicleDto dto) {
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
            return Created($"/api/vehicles/{vehicle.Registration}", vehicle.ToResource());
        }
    }
}