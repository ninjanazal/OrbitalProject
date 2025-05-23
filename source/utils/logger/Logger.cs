using System.Runtime.CompilerServices;

namespace Orbital {
	using LogLevel = LogConstants.Level;

	/// <summary>
	/// Provides logging functionality with different log levels.
	/// </summary>
	/// <remarks>
	/// This class allows logging messages at various levels such as TRACE, INFO, WARN, and ERROR.
	/// It also captures metadata such as the file path, member name, and line number of the caller.
	/// </remarks>
	public static partial class Logger {
		#region Public

		/// <summary>
		/// Logs a message at the TRACE level.
		/// </summary>
		/// <param name="p_msg">The main message to log.</param>
		/// <param name="p_optional">An optional additional message or object.</param>
		public static void Trace(object p_msg, object p_optional = null,
															[CallerFilePath] string p_callerPath = "",
															[CallerMemberName] string p_memberName = "",
															[CallerLineNumber] int p_callerLine = 0) {
			Output(LogLevel.TRACE, p_msg, p_optional, p_callerPath, p_memberName, p_callerLine);
		}


		/// <summary>
		/// Logs a message at the INFO level.
		/// </summary>
		/// <param name="p_msg">The main message to log.</param>
		/// <param name="p_optional">An optional additional message or object.</param>
		public static void Info(object p_msg, object p_optional = null,
															[CallerFilePath] string p_callerPath = "",
															[CallerMemberName] string p_memberName = "",
															[CallerLineNumber] int p_callerLine = 0) {
			Output(LogLevel.INFO, p_msg, p_optional, p_callerPath, p_memberName, p_callerLine);
		}


		/// <summary>
		/// Logs a message at the WARN level.
		/// </summary>
		/// <param name="p_msg">The main message to log.</param>
		/// <param name="p_optional">An optional additional message or object.</param>
		public static void Warn(object p_msg, object p_optional = null,
															[CallerFilePath] string p_callerPath = "",
															[CallerMemberName] string p_memberName = "",
															[CallerLineNumber] int p_callerLine = 0) {
			Output(LogLevel.WARN, p_msg, p_optional, p_callerPath, p_memberName, p_callerLine);
		}


		/// <summary>
		/// Logs a message at the ERROR level.
		/// </summary>
		/// <param name="p_msg">The main message to log.</param>
		/// <param name="p_optional">An optional additional message or object.</param>
		public static void Error(object p_msg, object p_optional = null,
															[CallerFilePath] string p_callerPath = "",
															[CallerMemberName] string p_memberName = "",
															[CallerLineNumber] int p_callerLine = 0) {
			Output(LogLevel.ERROR, p_msg, p_optional, p_callerPath, p_memberName, p_callerLine);
		}

		#endregion Public

	}
}
