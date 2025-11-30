using Ovning5.Models;

namespace Ovning5.Interfaces
{
	internal interface IHandler
	{
		bool GarageExists();

		bool IsEmpty();

		string? GetGarageName();

		void CreateNewGarage(int capacity, string name);

		bool ParkVehicle(Vehicle vehicle, out string message);

		bool RemoveVehicle(string identifier, out string message);

		IEnumerable<Vehicle> GetAllParkedVehicles();

		Vehicle? GetVehicle(string identifier);

		IEnumerable<Vehicle> Search(
			VehicleColor? color = null,
			int? wheels = null,
			string? identifierSubstring = null
		);

		Dictionary<string, List<Vehicle>> GroupByType();
	}
}