namespace Orbital {
	/// <summary>
	/// Interface for logging output handling.
	/// </summary>
	public interface IPrinter {

		#region Constants
		/// <summary>
		/// Arrow separator used in log formatting.
		/// </summary>
		protected const string k_ARROW_SEP = "[color=cyan]›[/color]";

		#endregion Constants
		#region Public


		/// <summary>
		/// Prints a log message with a formatted structure.
		/// </summary>
		/// <param name="p_lvl">The log level as a string (e.g., "INFO", "WARN").</param>
		/// <param name="p_time">The formatted timestamp of the log entry.</param>
		/// <param name="p_caller">The caller's information, including file name and line number.</param>
		/// <param name="p_msg">The actual log message to print.</param>
		public void LogPrint(string p_lvl, string p_time, string p_caller, string p_msg);

		#endregion Public
	}
}
