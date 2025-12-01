using Ovning5.Interfaces;

namespace Ovning5.Managers
{
	internal class ManagerMenu
	{
		private record MenuItem(string key, string description, Action action);

		// ToDo: Change to Dictionary<string, MenuItem> for faster lookup but for now, keep it simple
		// The order added shouold be the order displayed
		private List<MenuItem> _menuItems = new List<MenuItem>();

		private IUI _uI;

		public ManagerMenu(IUI ui)
		{
			_uI = ui;
		}

		public void AddMenuItem(string key, string description, Action action)
		{
			foreach (var item in _menuItems)
			{
				if (item.key == key)
				{
					throw new ArgumentException($"Menu item with key {key} already exists.");
				}
			}
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
					_uI.PrintLine($"You selected: {menuItem.description}");
					menuItem.action();
					return;
				}
			}
			_uI.PrintLine("Invalid selection. Please try again.");
		}
	}
}