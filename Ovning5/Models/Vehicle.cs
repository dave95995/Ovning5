namespace Ovning5.Models
{
	internal abstract class Vehicle
	{
		public string Identifier { get; }
		public int Wheels { get; }

		// I used enum instead of string for color to ensure valid colors only and make error handling easier
		public VehicleColor Color { get; }

		public Vehicle(string identifier, int wheels, VehicleColor color)
		{
			if (string.IsNullOrWhiteSpace(identifier))
			{
				throw new ArgumentException("Identifying number cannot be null or whitespace.", nameof(identifier));
			}

			// Boats may have 0 wheels, but no vehicle can have negative wheels
			if (wheels < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(wheels), "Wheels cannot be negative.");
			}

			Identifier = identifier.ToUpper();
			Wheels = wheels;
			Color = color;
		}

		public override string ToString()
		{
			return $"{GetType().Name,-15}ID: {Identifier,-10}Wheels: {Wheels,-5}Color: {Color,-10}";
		}
	}
}