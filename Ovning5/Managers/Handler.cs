using Ovning5.Interfaces;
using Ovning5.Models;

namespace Ovning5.Managers
{
	internal class Handler : IHandler
	{
		private IGarage<Vehicle> _garage;

		public bool GarageExists()
		{
			return _garage != null;
		}

		public bool IsEmpty()
		{
			return _garage != null && !_garage.Any();
		}

		public void CreateNewGarage(int capacity, string name)
		{
			_garage = new Garage<Vehicle>(capacity, name);
		}

		public IEnumerable<Vehicle> GetAllParkedVehicles()
		{
			// if there is no garage, return empty list
			if (_garage is null)
				return Enumerable.Empty<Vehicle>();
			return _garage;
		}

		public string? GetGarageName()
		{
			if (_garage is not null)
				return _garage.Name;
			return null;
		}

		public Vehicle? GetVehicle(string identifier)
		{
			// if there is no garage, there can be no vehicles
			if (!GarageExists())
				return null;

			return _garage.GetVehicle(identifier);
		}

		public Dictionary<string, List<Vehicle>> GroupByType()
		{
			// if there is no garage, return empty dictionary
			// the caller does not have to check for null
			if (!GarageExists())
				return new Dictionary<string, List<Vehicle>>(0);
			return _garage
				.GroupBy(v => v.GetType().Name)
				.ToDictionary(
					g => g.Key,
					g => g.ToList()
				);
		}

		public bool ParkVehicle(Vehicle vehicle, out string message)
		{
			if (!GarageExists())
			{
				message = "No active garage. Create or load a garage first.";
				return false;
			}

			if (vehicle == null)
			{
				message = "Vehicle cannot be null.";
				return false;
			}

			if (_garage.GetVehicle(vehicle.Identifier) != null)
			{
				message = $"{vehicle.Identifier} already parked!";
				return false;
			}

			if (_garage.Add(vehicle))
			{
				message = $"{vehicle.Identifier} parked successfully.";
				return true;
			}
			else
			{
				message = "Garage is full!";
				return false;
			}
		}

		public bool RemoveVehicle(string? identifier, out string message)
		{
			if (!GarageExists())
			{
				message = "No active garage!";
				return false;
			}
			if (string.IsNullOrWhiteSpace(identifier))
			{
				message = "Identifier cannot be empty!";
				return false;
			}

			Vehicle vehicle = GetVehicle(identifier);
			if (vehicle == null)
			{
				message = $"{identifier} not found!";
				return false;
			}

			if (_garage.Remove(identifier))
			{
				message = $"Vehicle {identifier} left the parking!";
				return true;
			}

			message = $"Failed to remove vehicle {identifier}";
			return false;
		}

		// this is just a konujunction of filters
		public IEnumerable<Vehicle> Search(
				VehicleColor? color = null,
				int? wheels = null,
				string? identifierSubstring = null)

		{
			if (!GarageExists())
				return Enumerable.Empty<Vehicle>();

			IEnumerable<Vehicle> result = _garage;

			if (color != null)
				result = result.Where(v => v.Color == color);

			if (wheels != null)
				result = result.Where(v => v.Wheels == wheels);

			// all string searches are case insensitive
			if (!string.IsNullOrWhiteSpace(identifierSubstring))
			{
				result = result.Where(v => v.Identifier.Contains(identifierSubstring, StringComparison.InvariantCultureIgnoreCase));
			}

			return result;
		}
	}
}