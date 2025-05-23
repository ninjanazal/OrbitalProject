using Godot;

namespace Orbital {
	/// <summary>
	/// Handles logging of warning messages with a formatted output.
	/// </summary>
	public class WarnPrinter : IPrinter {
		#region Constants
		/// <summary>
		/// The color used for warning level logs in the output.
		/// </summary>
		private const string k_LEVEL_COLOR = "orange";

		#endregion
		#region Public

		/// <summary>
		/// Prints a warning log message using Godot's rich text formatting.
		/// </summary>
		/// <param name="p_lvl">The log level as a string (e.g., "WARN").</param>
		/// <param name="p_time">The formatted timestamp of the log entry.</param>
		/// <param name="p_caller">The caller's information, including file name and line number.</param>
		/// <param name="p_msg">The actual log message to print.</param>
		public void LogPrint(string p_lvl, string p_time, string p_caller, string p_msg) {
			GD.PrintRich($"{p_time} [color={k_LEVEL_COLOR}]{p_lvl}[/color] {p_caller} {IPrinter.k_ARROW_SEP} {p_msg}");
		}

		#endregion Public
	}
}
