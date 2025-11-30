namespace Ovning5.Models
{
	internal class Car : Vehicle
	{
		public bool SelfDriving { get; }

		public Car(string identifier, int wheels, VehicleColor color, bool selfDriving)
			: base(identifier, wheels, color)
		{
			SelfDriving = selfDriving;
		}

		public override string ToString()
		{
			return base.ToString() + $"Self-Driving: {SelfDriving,-10}";
		}
	}
}