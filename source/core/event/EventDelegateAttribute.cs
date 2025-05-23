using System;

namespace Orbital {
	/// <summary>
	/// Custom attribute for marking delegates that represent events in the system.
	/// Associates event delegates with a category, description, and specific type information.
	/// </summary>
	[AttributeUsage(AttributeTargets.Delegate)]
	public class EventDelegateAttribute : Attribute {
		#region Fields & Properties

		/// <summary>
		/// The logical grouping category for this event type
		/// </summary>
		private readonly string m_category;
		/// <summary>
		/// Human-readable explanation of the event's purpose
		/// </summary>
		private readonly string m_description;
		/// <summary>
		/// The System.Type of the associated delegate
		/// </summary>
		private readonly Type m_type;

		#endregion Fields & Properties
		#region Public

		public EventDelegateAttribute(Type p_delegateType, string p_category, string p_description = "") {
			m_type = p_delegateType;
			m_category = p_category;
			m_description = p_description;
		}

		/// <summary>
		/// Gets the delegate type associated with this event
		/// </summary>
		public Type GetEventType() {
			return m_type;
		}


		/// <summary>
		/// Gets the organizational category for this event
		/// </summary>
		public string GetCategory() {
			return m_category;
		}


		/// <summary>
		/// Checks whether a description exists for this event
		/// </summary>
		public bool HasDescription() {
			return String.IsNullOrEmpty(m_description);
		}


		/// <summary>
		/// Gets the event description text
		/// </summary>
		public string GetDescription() {
			return m_description;
		}

		#endregion Public
	}
}
