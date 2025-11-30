namespace Ovning5.Interfaces
{
	internal interface IUI
	{
		string GetInput(string v = "");

		void Print(string message);

		void PrintLine(string message = "");
	}
}