using Ovning5.Interfaces;

namespace Ovning5.UI
{
	internal class ConsoleUI : IUI
	{
		public string GetInput(string v)
		{
			Print(v);
			return Console.ReadLine() ?? string.Empty;
		}

		public void Print(string message)
		{
			Console.Write(message);
		}

		public void PrintLine(string message)
		{
			Console.WriteLine(message);
		}
	}
}