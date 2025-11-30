namespace Ovning5.Models
{
	internal class Bus : Vehicle
	{
		public int Seats { get; }

		public Bus(string identifier, int wheels, VehicleColor color, int seats)
			: base(identifier, wheels, color)
		{
			if (seats < 0)
				throw new ArgumentOutOfRangeException(nameof(seats), "Number of seats cannot be negative.");
			Seats = seats;
		}

		public override string ToString()
		{
			return base.ToString() + $"Seats: {Seats,-10}";
		}
	}
}