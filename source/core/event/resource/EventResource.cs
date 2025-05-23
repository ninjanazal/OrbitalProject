using System;
using Godot;

namespace Orbital {
	/// <summary>
	/// Represents a resource for an event in the game. This class stores information about the event,
	/// such as its category, name, description, and type.
	/// </summary>
	[Tool]
	[RegistNode(typeof(EventResource))]
	public partial class EventResource : Resource {
		#region Fields & Properties

		/// <summary>
		/// The category of the event. Used to group events into meaningful classifications.
		/// </summary>
		private string m_category;

		/// <summary>
		/// The name of the event. This should uniquely identify an event within its category.
		/// </summary>
		private string m_eventName;

		/// <summary>
		/// A description of the event. Provides additional context or details about the event.
		/// </summary>
		private string m_description;

		/// <summary>
		/// The type of the event. This is determined dynamically based on the category and name.
		/// </summary>
		private Type m_eventType;

		#endregion Fields & Properties
		#region Public

		/// <summary>
		/// Retrieves the type of the event based on its category and name.
		/// </summary>
		public Type GetEventType() {
			Type result = EventManager.GetTypeFromName(m_category, m_eventName);
			if (result == null) {
				Logger.Warn("Invalid parameters, failed to find event type", "Check for category and select an event");
			}
			return result;
		}

		#endregion Public

	}
}
