using Ovning5.Managers;
using Ovning5.Models;
using Ovning5.UI;

namespace Ovning5
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			//NormalRun();
			TestRun();
			//AutoRun();
		}

		private static void NormalRun()
		{
			var ui = new ConsoleUI();
			var handler = new Handler();
			var manager = new Manager(ui, handler);
			manager.Run();
		}

		private static void AutoRun()
		{
			var ui = new Mock_UI();
			ui.AddInput("1", "2", "oops", "   ", "4", "   ", "4", "FINNSINTE", "1", "3", "abc123", "2", "4", "1", "1", "y", "3", "nybil3", "-4", "2", "4", "3", "24", "3", "asdf23434232", "4", "green", "7", "0", "-4", "2", "1", "y", "2", "5", "abc", "6", "n", "asdfsadf", "n", "y", "abc", "1", "2", "6", "y", "4", "y", "2", "n", "1", "5", "boat5", "7", "8");
			var handler = new Handler();
			handler.CreateNewGarage(10, "AutoGarage");
			handler.ParkVehicle(new Car("ABC123", 4, VehicleColor.Blue, false), out _);
			handler.ParkVehicle(new Motorcycle("MOTO1", 2, VehicleColor.Red, 600), out _);
			handler.ParkVehicle(new Bus("BUS22", 6, VehicleColor.Black, 45), out _);
			handler.ParkVehicle(new Car("XYZ789", 4, VehicleColor.White, true), out _);
			handler.ParkVehicle(new Motorcycle("BIKE9", 2, VehicleColor.Green, 1000), out _);
			handler.ParkVehicle(new Boat("BOAT5", 0, VehicleColor.Yellow, 20), out _);
			handler.ParkVehicle(new Airplane("PLANE3", 3, VehicleColor.White, 150), out _);

			var manager = new Manager(ui, handler);
			manager.Run();
			foreach (var u in ui.Outputs)
			{
				Console.Write(u);
			}
		}

		private static void TestRun()
		{
			var ui = new ConsoleUI();
			var handler = new Handler();
			handler.CreateNewGarage(8, "TestRunGarage");
			handler.ParkVehicle(new Car("ABC123", 4, VehicleColor.Blue, false), out _);
			handler.ParkVehicle(new Motorcycle("MOTO1", 2, VehicleColor.Red, 600), out _);
			handler.ParkVehicle(new Bus("BUS22", 6, VehicleColor.Black, 45), out _);
			handler.ParkVehicle(new Car("XYZ789", 4, VehicleColor.White, true), out _);
			handler.ParkVehicle(new Motorcycle("BIKE9", 2, VehicleColor.Green, 1000), out _);
			handler.ParkVehicle(new Boat("BOAT5", 0, VehicleColor.Yellow, 20), out _);
			handler.ParkVehicle(new Airplane("PLANE3", 3, VehicleColor.White, 150), out _);
			var manager = new Manager(ui, handler);
			manager.Run();

		}
	}
}