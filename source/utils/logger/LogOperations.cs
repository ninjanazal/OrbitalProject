using Godot;
using System.Collections.Generic;

namespace Orbital {
	using LogLevel = LogConstants.Level;

	public static partial class Logger {
		#region Constants

		/// <summary>
		/// Maps log levels to their corresponding short string representations.
		/// </summary>
		private static readonly Dictionary<LogLevel, string> m_levelNames = new() {
			{LogLevel.TRACE, "TRC"},
			{LogLevel.INFO, "INF"},
			{LogLevel.WARN, "WRN"},
			{LogLevel.ERROR, "ERR"}
		};

		/// <summary>
		/// Maps log levels to their respective printer instances.
		/// </summary>
		private static readonly Dictionary<LogLevel, IPrinter> m_printers = new() {
			{LogLevel.TRACE, new TracePrinter()},
			{LogLevel.INFO, new InfoPrinter()},
			{LogLevel.WARN, new WarnPrinter()},
			{LogLevel.ERROR, new ErrorPrinter()}
		};

		/// <summary>
		/// Separator character used in optional log messages.
		/// </summary>
		private const char k_OPTIONAL_SEP = '|';

		/// <summary>
		/// String used to indicate incomplete names.
		/// </summary>
		private const string k_INCOMPLETE_NAME = "…";

		/// <summary>
		/// String used for incomplete script names.
		/// </summary>
		private const string k_INCOMPLETE_SCRIPT = $"{k_INCOMPLETE_NAME}cs";

		/// <summary>
		/// Maximum length allowed for caller names.
		/// </summary>
		private const int k_MAX_LENGTH_NAME = 15;

		/// <summary>
		/// Expected length of line number padding.
		/// </summary>
		private const int k_LINE_EXPECTED_LENGTH = 3;

		#endregion Constants

		#region Private


		/// <summary>
		/// Outputs a formatted log message.
		/// </summary>
		/// <param name="p_level">The log level of the message.</param>
		/// <param name="p_msg">The main message to log.</param>
		/// <param name="p_optional">An optional additional message or object.</param>
		/// <param name="p_callerPath">The file path of the caller.</param>
		/// <param name="p_callerMember">The name of the calling method.</param>
		/// <param name="p_callerLine">The line number in the source code where this method was called.</param>
		private static void Output(LogLevel p_level, object p_msg, object p_optional,
												string p_callerPath, string p_callerMember, int p_callerLine) {
			string lvl = m_levelNames[p_level];
			string time = ParseUpTime();
			string caller = ParseCallerData(p_callerPath, p_callerMember, p_callerLine);
			string msg = p_msg.ToString();

			if (p_optional != null && !string.IsNullOrEmpty(p_optional.ToString())) {
				msg += $" {k_OPTIONAL_SEP} {p_optional}";
			}

			m_printers[p_level].LogPrint(lvl, time, caller, msg);

		}

		#region Parsers


		/// <summary>
		/// Parses and formats caller information.
		/// </summary>
		/// <param name="p_callerPath">The file path of the caller.</param>
		/// <param name="p_callerMember">The name of the calling method.</param>
		/// <param name="p_callerLine">The line number in the source code where this method was called.</param>
		/// <returns>A formatted string representing the caller's information.</returns>
		private static string ParseCallerData(string p_callerPath, string p_callerMember, int p_callerLine) {
			string callerFile = p_callerPath.GetFile().Split('.')[0];
			if (callerFile.Length > k_MAX_LENGTH_NAME + k_INCOMPLETE_SCRIPT.Length) {
				callerFile = callerFile.Substr(0, k_MAX_LENGTH_NAME) + k_INCOMPLETE_SCRIPT;
			}

			string memberName = p_callerMember;
			if (memberName.Length > k_MAX_LENGTH_NAME + k_INCOMPLETE_NAME.Length) {
				memberName = memberName.Substr(0, k_MAX_LENGTH_NAME) + k_INCOMPLETE_NAME;
			}

			string line = p_callerLine.ToString().PadRight(k_LINE_EXPECTED_LENGTH);
			return $"({memberName,-k_MAX_LENGTH_NAME}) {callerFile,-k_MAX_LENGTH_NAME}:{line, k_LINE_EXPECTED_LENGTH}";
		}


		/// <summary>
		/// Parses and formats the application's uptime into a timestamp.
		/// </summary>
		/// <returns>A formatted string representing the elapsed time since application start.</returns>
		private static string ParseUpTime() {
			System.TimeSpan time = System.TimeSpan.FromMilliseconds(Time.GetTicksMsec());

			int hours = time.Hours;
			int minutes = time.Minutes;
			int seconds = time.Seconds;
			int ms = time.Milliseconds;

			return $"{hours:D2}:{minutes:D2}:{seconds:D2}:{ms:D3}";
		}

		#endregion Parsers
		#endregion Private
	}
}
