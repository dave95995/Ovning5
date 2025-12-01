using Ovning5.Interfaces;
using Ovning5.Models;
using Ovning5.UI;
using System;
using System.Drawing;
using System.Runtime.ConstrainedExecution;

namespace Ovning5.Managers
{
	internal class Manager
	{
		private IUI _uI;
		private IHandler _handler;

		private ManagerMenu _mainMenu;
		private ManagerMenu _vehicleMenu;

		public Manager(IUI uI, IHandler handler)
		{
			_uI = uI ?? throw new ArgumentNullException(nameof(uI));
			_handler = handler ?? throw new ArgumentNullException(nameof(handler));

			_mainMenu = new ManagerMenu(_uI);
			_mainMenu.AddMenuItem("1", "List Parked Vehicles", ListParkedVehicles);
			_mainMenu.AddMenuItem("2", "List Parked Vehicles by Group", ListParkedVehiclesByGroup);
			_mainMenu.AddMenuItem("3", "Park a Vehicle", CreateVehicleFromUserInput);
			_mainMenu.AddMenuItem("4", "Remove a Vehicle", PromptAndRemoveByLicensePlate);
			_mainMenu.AddMenuItem("5", "Search Vehicle by license plate", PromptAndSearchByLicensePlate);
			_mainMenu.AddMenuItem("6", "Search Vehicles by Properties", SearchByProperties);
			_mainMenu.AddMenuItem("7", "Create garage", TryCreateGarage);
			_mainMenu.AddMenuItem("8", "Exit", HandleExit);

			_vehicleMenu = new ManagerMenu(_uI);
			_vehicleMenu.AddMenuItem("1", "Car", CreateCar);
			_vehicleMenu.AddMenuItem("2", "Motorcycle", CreateMotorcycle);
			_vehicleMenu.AddMenuItem("3", "Bus", CreateBus);
			_vehicleMenu.AddMenuItem("4", "Airplane", CreateAirplane);
			_vehicleMenu.AddMenuItem("5", "Boat", CreateBoat);
		}

		internal void Run()
		{
			if (_handler.GarageExists() == false)
			{
				HandleCreateGarage();
			}
			bool running = true;
			while (running)
			{
				_uI.PrintLine($"Menu for garage: {_handler.GetGarageName()}");
				_mainMenu.DisplayMenu();
				_mainMenu.ReadMenuChoice();
			}
		}

		private void HandleExit()
		{
			_uI.PrintLine("Exiting the system. Goodbye!");
			Environment.Exit(0);
		}

		private void TryCreateGarage()
		{
			if (_handler.GarageExists() == true)
			{
				_uI.PrintLine("Only one garage possible at this time.");
				return;
			}
			HandleCreateGarage();
		}

		private void PromptAndRemoveByLicensePlate()
		{
			_uI.Print("Enter license plate of vehicle to remove: ");
			string removeId = _uI.GetInput();
			RemoveVehicle(removeId);
		}

		private void PromptAndSearchByLicensePlate()
		{
			_uI.Print("Enter license plate to search: ");
			string searchId = _uI.GetInput();
			SearchVehicle(searchId);
			return;
		}

		private void HandleCreateGarage()
		{
			int garageCapacity = IOUtils.ReadIntBetween(_uI, 1, 100, "Enter garage capacity: ", "Enter a value between 1 and 100.");
			string name = IOUtils.ReadString(_uI, "Enter garage name: ");
			_handler.CreateNewGarage(garageCapacity, name);
		}

		private void SearchByProperties()
		{
			VehicleColor? color = null;
			int? wheels = null;
			string? text = null;

			if (IOUtils.ReadYesNo(_uI, "Filter by color? (y/n): ", "Enter y or n."))
			{
				var colors = Enum.GetValues<VehicleColor>().ToList();

				_uI.PrintLine("Available colors:");
				for (int i = 0; i < colors.Count; i++)
					_uI.PrintLine($"{i + 1}. {colors[i]}");

				int selected = IOUtils.ReadIntBetween(_uI, 1, colors.Count, "Select color: ", "");
				color = colors[selected - 1];
			}

			if (IOUtils.ReadYesNo(_uI, "Filter by number of wheels? (y/n): ", "Enter y or n."))
			{
				wheels = IOUtils.ReadNonNegativeInt(_uI, "Enter wheels: ", "");
			}

			if (IOUtils.ReadYesNo(_uI, "Search text in identifier? (y/n): ", "Enter y or n."))
			{
				text = _uI.GetInput("Enter substring: ");
			}

			var results = _handler.Search(color, wheels, text).ToList();
			_uI.PrintLine("Search results:");

			foreach (var v in results)
				_uI.PrintLine("\t" + v);
		}

		private void SearchVehicle(string identifier)
		{
			var vehicle = _handler.GetVehicle(identifier);
			if (vehicle != null)
			{
				_uI.PrintLine($"Vehicle found: {vehicle}");
			}
			else
			{
				_uI.PrintLine($"Vehicle with identifier {identifier} not found.");
			}
		}

		private void ListParkedVehiclesByGroup()
		{
			var groups = _handler.GroupByType();

			if (groups.Count == 0)
			{
				_uI.PrintLine("No vehicles are currently parked.");
				return;
			}

			foreach (var group in groups)
			{
				_uI.PrintLine($"{group.Key}: {group.Value.Count} parked");

				foreach (var v in group.Value)
					_uI.PrintLine("\t" + v.ToString());
			}
		}

		private void ListParkedVehicles()
		{
			var vehicles = _handler.GetAllParkedVehicles().ToList();
			if (vehicles.Count == 0)
			{
				_uI.PrintLine("No vehicles are currently parked.");
				return;
			}
			_uI.PrintLine("Parked Vehicles:");
			foreach (var v in vehicles)
			{
				_uI.PrintLine("\t" + v.ToString());
			}
		}

		private void ReadSharedAttributes(out string identifier, out int wheels, out VehicleColor color)
		{
			identifier = IOUtils.ReadString(_uI, "Enter license plate (identifier): ");
			wheels = IOUtils.ReadNonNegativeInt(_uI, "Enter number of wheels: ", "Wheels cannot be negative.");
			color = ReadColor();
		}

		private VehicleColor ReadColor()
		{
			var colors = Enum.GetValues<VehicleColor>().ToList();
			_uI.PrintLine("Available colors");
			for (int i = 0; i < colors.Count; i++)
				_uI.PrintLine($"{i + 1}. {colors[i]}");
			int selected = IOUtils.ReadIntBetween(_uI, 1, colors.Count, "Select color: ", "Invalid selection.");
			return colors[selected - 1];
		}

		private void CreateVehicleFromUserInput()
		{
			_vehicleMenu.DisplayMenu();
			_vehicleMenu.ReadMenuChoice();
		}

		private void CreateCar()
		{
			ReadSharedAttributes(out string identifier, out int wheels, out VehicleColor color);
			bool isSelfDriving = IOUtils.ReadYesNo(_uI, "Is the car self-driving? (y/n): ", "Enter y or n.");
			var car = new Car(identifier, wheels, color, isSelfDriving);
			ParkVehicle(car);
		}

		private void CreateMotorcycle()
		{
			ReadSharedAttributes(out string identifier, out int wheels, out VehicleColor color);
			int cylinderVolume = IOUtils.ReadNonNegativeInt(_uI, "Enter cylinder volume: ", "Cylinder volume cannot be negative.");
			var motorCycle = new Motorcycle(identifier, wheels, color, cylinderVolume);
			ParkVehicle(motorCycle);
		}

		private void CreateBus()
		{
			ReadSharedAttributes(out string identifier, out int wheels, out VehicleColor color);
			int numberOfSeats = IOUtils.ReadNonNegativeInt(_uI, "Enter number of seats: ", "Number of seats cannot be negative.");
			var bus = new Bus(identifier, wheels, color, numberOfSeats);
			ParkVehicle(bus);
		}

		private void CreateAirplane()
		{
			ReadSharedAttributes(out string identifier, out int wheels, out VehicleColor color);
			int numberOfEngines = IOUtils.ReadNonNegativeInt(_uI, "Enter number of engines: ", "Number of engines cannot be negative.");
			var airplane = new Airplane(identifier, wheels, color, numberOfEngines);
			ParkVehicle(airplane);
		}

		private void CreateBoat()
		{
			ReadSharedAttributes(out string identifier, out int wheels, out VehicleColor color);
			int length = IOUtils.ReadNonNegativeInt(_uI, "Enter length: ", "Length cannot be negative.");
			var boat = new Boat(identifier, wheels, color, length);
			ParkVehicle(boat);
		}

		private void ParkVehicle(Vehicle vehicle)
		{
			bool success = _handler.ParkVehicle(vehicle, out string message);
			_uI.PrintLine(message);
		}

		private void RemoveVehicle(string identifier)
		{
			bool success = _handler.RemoveVehicle(identifier, out string message);
			_uI.PrintLine(message);
		}
	}
}