using Ovning5.Models;

namespace Ovning5.Interfaces
{
	internal interface IGarage<T> : IEnumerable<T> where T : Vehicle
	{
		string Name { get; }

		bool Add(T vehicle);

		bool Contains(string identifier);

		bool Remove(string identifier);

		int Capacity { get; }

		T? GetVehicle(string identifier);
	}
}