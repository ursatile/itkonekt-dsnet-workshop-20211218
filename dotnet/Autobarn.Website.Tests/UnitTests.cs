using Xunit;
using Autobarn.Website.Controllers.api;
using Autobarn.Data;
using Autobarn.Data.Entities;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Shouldly;

namespace Autobarn.Website.Tests {
    //public class ApiVehiclesControllerTests {
    //    [Fact]
    //    public void POST_with_existing_vehicle_returns_conflict() {
    //        var db = new FakeDatabase();
    //        var c = new ModelsController(db);
    //        var dto = new Models.VehicleDto { Registration = "CONFLICT" };
    //        var result = c.Post("dmc-delorean", dto);
    //        result.ShouldBeOfType<ConflictObjectResult>();
    //    }
    //}

    public class FakeDatabase : IAutobarnDatabase {
        public int CountVehicles() {
            throw new System.NotImplementedException();
        }

        public void CreateVehicle(Vehicle vehicle) {
            throw new System.NotImplementedException();
        }

        public void DeleteVehicle(Vehicle vehicle) {
            throw new System.NotImplementedException();
        }

        public Manufacturer FindManufacturer(string code) {
            throw new System.NotImplementedException();
        }

        public Model FindModel(string code) {
            throw new System.NotImplementedException();
        }

        public Vehicle FindVehicle(string registration) {
            return (registration == "CONFLICT" ? new Vehicle() : null);
        }

        public IEnumerable<Manufacturer> ListManufacturers() {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Model> ListModels() {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Vehicle> ListVehicles() {
            throw new System.NotImplementedException();
        }

        public void UpdateVehicle(Vehicle vehicle) {
            throw new System.NotImplementedException();
        }
    }
}