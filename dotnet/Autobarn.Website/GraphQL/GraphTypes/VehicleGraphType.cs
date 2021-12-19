using Autobarn.Data.Entities;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.GraphTypes {
    public class VehicleGraphType : ObjectGraphType<Vehicle> {
        public VehicleGraphType() {
            Name = "vehicle";
            Field(v => v.Registration)
                .Description("The registration (license plate) of the vehicle");
            Field(v => v.Color).Description("What color is this vehicle?");
            Field(v => v.Year).Description("What year was this vehicle first registered?");
            Field(c => c.VehicleModel, nullable: false, type: typeof(VehicleModelGraphType))
                .Description("What model of vehicle is this?");
        }
    }
}
