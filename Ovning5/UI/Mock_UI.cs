using Ovning5.Interfaces;

namespace Ovning5.UI
{
	internal class Mock_UI : IUI
	{
		private const string InputBgColor = "\u001b[42m";
		private const string InputTextColor = "\u001b[30m";
		private const string ResetColor = "\u001b[0m";

		private readonly Queue<string> _inputs = new();
		public readonly List<string> Outputs = new();

		public void AddInput(params string[] inputs)
		{
			foreach (var input in inputs)
				_inputs.Enqueue(input);
		}

		public string GetInput(string prompt = "")
		{
			var input = _inputs.Dequeue();
			Outputs.Add($"{prompt}{InputBgColor}{InputTextColor}{input}{ResetColor}\n");
			return input;
		}

		public void Print(string message)
		{
			Outputs.Add(message);
		}

		public void PrintLine(string message = "")
		{
			Outputs.Add(message + "\n");
		}
	}
}