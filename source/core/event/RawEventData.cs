using System;

namespace Orbital {
	public class RawEventData {
		#region Fields & Properties

		/// <summary>
		/// The type of the event.
		/// </summary>
		private readonly Type m_type;

		/// <summary>
		/// The category of the event.
		/// </summary>
		private readonly string m_category;

		/// <summary>
		/// A description of the event.
		/// </summary>
		private readonly string m_description;

		#endregion Fields & Properties
		#region Public

		public RawEventData(Type p_type, string p_category, string p_description) {
			m_type = p_type;
			m_category = p_category;
			m_description = p_description;
		}

		/// <summary>
		/// Gets the type of the event.
		/// </summary>
		/// <returns>The event type.</returns>
		public Type GetEventType() {
			return m_type;
		}

		/// <summary>
		/// Gets the category of the event.
		/// </summary>
		/// <returns>The event category.</returns>
		public string GetEventCategory() {
			return m_category;
		}

		/// <summary>
		/// Gets the description of the event.
		/// </summary>
		/// <returns>The event description.</returns>
		public string GetEventDescription() {
			return m_description;
		}

		#endregion Public
	}
}
