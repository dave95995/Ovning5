namespace Ovning5.Models
{
	internal class Boat : Vehicle
	{
		public int Length { get; }

		public Boat(string identifier, int wheels, VehicleColor color, int length)
			: base(identifier, wheels, color)
		{
			if (length < 0)
				throw new ArgumentOutOfRangeException(nameof(length), "Length cannot be negative.");

			Length = length;
		}

		public override string ToString()
		{
			return base.ToString() + $"Length: {Length,-10}"; ;
		}
	}
}