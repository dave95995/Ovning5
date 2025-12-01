using Ovning5.Interfaces;
using Ovning5.Models;
using Ovning5.UI;

namespace Ovning5.Managers
{
	internal class ManagerMenu
	{
		record MenuItem(string key, string description, Action action);

		private List<MenuItem> _menuItems = new List<MenuItem>();

		private IUI _uI;

		public ManagerMenu(IUI ui)
		{
			_uI = ui;
		}

		public void AddMenuItem(string key, string description, Action action)
		{
			_menuItems.Add(new MenuItem(key, description, action));
		}

		public void DisplayMenu()
		{
			foreach (var menuItem in _menuItems)
			{
				_uI.PrintLine($"{menuItem.key}: {menuItem.description}");
			}
		}

		public void ReadMenuChoice()
		{
			string choice = _uI.GetInput("Select an option: ");

			foreach (var menuItem in _menuItems)
			{
				if (menuItem.key == choice)
				{
					menuItem.action();
					return;
				}
			}
			_uI.PrintLine("Invalid selection. Please try again.");
		}
	}

	internal class Manager
	{
		private IUI _uI;
		private IHandler _handler;
		private ManagerMenu _menu;

		public Manager(IUI uI, IHandler handler)
		{
			_uI = uI ?? throw new ArgumentNullException(nameof(uI));
			_handler = handler ?? throw new ArgumentNullException(nameof(handler));

			_menu = new ManagerMenu(_uI);
			_menu.AddMenuItem("1", "List Parked Vehicles", ListParkedVehicles);
			_menu.AddMenuItem("2", "List Parked Vehicles by Group", ListParkedVehiclesByGroup);
			_menu.AddMenuItem("3", "Park a Vehicle", CreateAndParkVehicle);
			_menu.AddMenuItem("4", "Remove a Vehicle", PromptAndRemoveByLicensePlate);
			_menu.AddMenuItem("5", "Search Vehicle by license plate", PromptAndSearchByLicensePlate);
			_menu.AddMenuItem("6", "Search Vehicles by Properties", SearchByProperties);
			_menu.AddMenuItem("7", "Create garage", TryCreateGarage);
			_menu.AddMenuItem("8", "Exit", HandleExit);
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
				_menu.DisplayMenu();
				_menu.ReadMenuChoice();
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

		private Vehicle CreateVehicleFromUserInput()
		{
			string identifier = IOUtils.ReadString(_uI, "Enter license plate (identifier): ");

			int wheels = IOUtils.ReadNonNegativeInt(_uI, "Enter number of wheels: ", "Wheels cannot be negative.");
			var colors = Enum.GetValues<VehicleColor>().ToList();
			_uI.PrintLine("Available colors");
			for (int i = 0; i < colors.Count; i++)
				_uI.PrintLine($"{i + 1}. {colors[i]}");
			int selected = IOUtils.ReadIntBetween(_uI, 1, colors.Count, "Select color: ", "Invalid selection.");
			VehicleColor color = colors[selected - 1];
			_uI.PrintLine("Available vehicle types");
			_uI.PrintLine("1. Car");
			_uI.PrintLine("2. Motorcycle");
			_uI.PrintLine("3. Bus");
			_uI.PrintLine("4. Airplane");
			_uI.PrintLine("5. Boat");
			int typeSelection = IOUtils.ReadIntBetween(_uI, 1, 5, "Select type: ", "Invalid selection.");

			return typeSelection switch
			{
				1 => new Car(identifier, wheels, color, IOUtils.ReadYesNo(_uI, "Is it self-driving? (y/n): ", "Enter y or n.")),
				2 => new Motorcycle(identifier, wheels, color, IOUtils.ReadNonNegativeInt(_uI, "Enter cylinder volume: ", "Cylinder volume cannot be negative.")),
				3 => new Bus(identifier, wheels, color, IOUtils.ReadNonNegativeInt(_uI, "Enter number of seats: ", "Number of seats cannot be negative.")),
				4 => new Airplane(identifier, wheels, color, IOUtils.ReadNonNegativeInt(_uI, "Enter number of engines: ", "Number of engines cannot be negative.")),
				5 => new Boat(identifier, wheels, color, IOUtils.ReadNonNegativeInt(_uI, "Enter length: ", "Length cannot be negative.")),
				_ => throw new InvalidOperationException("Invalid vehicle type selection."),
			};
		}

		private void CreateAndParkVehicle()
		{
			Vehicle vehicle = CreateVehicleFromUserInput();
			ParkVehicle(vehicle);
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