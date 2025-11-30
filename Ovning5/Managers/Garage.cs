using Ovning5.Interfaces;
using Ovning5.Models;
using System.Collections;

namespace Ovning5.Managers
{
	internal class Garage<T> : IEnumerable<T>, IGarage<T> where T : Vehicle
	{
		private readonly T[] _vehicles;
		public int Capacity { get; }
		public string Name { get; }

		public Garage(int capacity, string name = "Garaget")
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException(nameof(name));
			}

			Name = name;

			if (capacity <= 0)
			{
				throw new ArgumentException("Capacity must be greater than zero.");
			}
			Capacity = capacity;
			_vehicles = new T[capacity];
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			for (int i = 0; i < _vehicles.Length; i++)
			{
				if (_vehicles[i] != null)
				{
					yield return _vehicles[i];
				}
			}
		}

		private string NormalizeIdentifier(string identifier)
		{
			if (string.IsNullOrWhiteSpace(identifier))
			{
				throw new ArgumentNullException("Identifier cannot be null or whitespace.", nameof(identifier));
			}
			return identifier.ToUpper();
		}

		// Adds a vehicle to the garage if there is space and it doesn't already exist
		// Returns true if added, false otherwise
		// Throws ArgumentNullException if vehicle is null
		public bool Add(T vehicle)
		{
			if (vehicle == null)
			{
				throw new ArgumentNullException(nameof(vehicle), "Vehicle cannot be null.");
			}

			if (Contains(vehicle.Identifier))
			{
				return false;
			}

			for (int i = 0; i < _vehicles.Length; i++)
			{
				// First empty slot
				if (_vehicles[i] == null)
				{
					_vehicles[i] = vehicle;
					return true;
				}
			}
			return false;
		}

		// Checks if a vehicle with the given identifier exists in the garage
		// case-insensitive check
		public bool Contains(string identifier)
		{
			identifier = NormalizeIdentifier(identifier);
			foreach (var vehicle in _vehicles)
			{
				if (vehicle != null && vehicle.Identifier == identifier)
				{
					return true;
				}
			}
			return false;
		}

		// Removes a vehicle with the given identifier from the garage
		// case-insensitive check
		public bool Remove(string identifier)
		{
			identifier = NormalizeIdentifier(identifier);
			for (int i = 0; i < _vehicles.Length; i++)
			{
				if (_vehicles[i] != null && _vehicles[i].Identifier == identifier)
				{
					_vehicles[i] = null;
					return true;
				}
			}
			return false;
		}

		// Gets a vehicle with the given identifier from the garage
		// case-insensitive check
		public T? GetVehicle(string identifier)
		{
			identifier = NormalizeIdentifier(identifier);
			foreach (var vehicle in _vehicles)
			{
				if (vehicle != null && vehicle.Identifier == identifier)
				{
					return vehicle;
				}
			}
			return null;
		}
	}
}