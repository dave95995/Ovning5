namespace Ovning5.Models
{
	internal class Airplane : Vehicle
	{
		public int Engines { get; }

		public Airplane(string identifier, int wheels, VehicleColor color, int engines)
			: base(identifier, wheels, color)
		{
			if (engines < 0)
				throw new ArgumentOutOfRangeException(nameof(engines), "Number of engines cannot be negative.");
			Engines = engines;
		}

		public override string ToString()
		{
			return base.ToString() + $"Engines: {Engines,-10}";
		}
	}
}