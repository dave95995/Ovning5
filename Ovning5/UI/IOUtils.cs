using Ovning5.Interfaces;

namespace Ovning5.UI
{
	internal static class IOUtils
	{
		// Reads an integer between minInclusive and maxInclusive from the user.
		// If the input is invalid, it prints the errorMessage and prompts again.
		internal static int ReadIntBetween(IUI io, int minInclusive, int maxInclusive, string? prompt, string? errorMessage)
		{
			while (true)
			{
				if (prompt is not null)
				{
					io.Print(prompt);
				}
				string input = io.GetInput();
				if (int.TryParse(input, out int value))
				{
					if (value >= minInclusive && value <= maxInclusive)
					{
						return value;
					}
				}
				if (errorMessage is not null)
				{
					io.PrintLine(errorMessage);
				}
			}
		}

		// Reads a non-negative integer from the user.
		// If the input is invalid, it prints the errorMessage and prompts again.
		internal static int ReadNonNegativeInt(IUI uI, string? prompt, string? errorMessage)
		{
			return ReadIntBetween(uI, 0, int.MaxValue, prompt, errorMessage);
		}

		// Reads a non-empty string from the user.
		// the string must contain at least one non-whitespace character.
		internal static string ReadString(IUI uI, string? prompt)
		{
			while (true)
			{
				if (prompt is not null)
				{
					uI.Print(prompt);
				}
				string input = uI.GetInput();
				if (!string.IsNullOrWhiteSpace(input))
				{
					return input;
				}
			}
		}

		// Reads a yes/no answer from the user.
		internal static bool ReadYesNo(IUI uI, string? prompt, string? errorMessage)
		{
			while (true)
			{
				if (prompt is not null)
				{
					uI.Print(prompt);
				}
				string input = uI.GetInput().Trim().ToLower();
				if (input == "y" || input == "yes")
				{
					return true;
				}
				else if (input == "n" || input == "no")
				{
					return false;
				}
				if (errorMessage is not null)
				{
					uI.PrintLine(errorMessage);
				}
			}
		}
	}
}