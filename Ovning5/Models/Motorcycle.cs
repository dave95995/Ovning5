namespace Ovning5.Models
{
	internal class Motorcycle : Vehicle
	{
		public int CylinderVolume { get; }

		public Motorcycle(string identifier, int wheels, VehicleColor color, int cylinderVolume)
			: base(identifier, wheels, color)
		{
			if (cylinderVolume < 0)
				throw new ArgumentOutOfRangeException(nameof(cylinderVolume), "Cylinder volume cannot be negative.");
			CylinderVolume = cylinderVolume;
		}

		public override string ToString()
		{
			return base.ToString() + $"CylinderVolume: {CylinderVolume,-10}";
		}
	}
}