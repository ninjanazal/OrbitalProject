using System;

namespace Orbital {

	/// <summary>
	/// Represents the type of event processing to be performed.
	/// </summary>
	public enum EventProcessingType {
		REGIST,
		LISTEN,
		WATCH,
		DISPATCH,
		FORGET
	}

	/// <summary>
	/// Represents an element of event processing, encapsulating details such as the type of processing,
	/// the event type, the delegate to handle the event, connection flags, and optional parameters.
	/// </summary>
	public class EventProcessingElement {
		#region Fields & Properties
		private readonly EventProcessingType m_processType;
		private Type m_type;
		private Delegate m_delegate;
		private int m_connectFlags;
		private object[] m_params;

		/// <summary>Gets the type of event processing.</summary>
		public EventProcessingType ProcessType { get => m_processType; }

		/// <summary>Gets the type of the event.</summary>
		public Type EventType { get => m_type; }

		/// <summary>Gets the delegate associated with the event.</summary>
		public Delegate EventDelegate { get => m_delegate; }

		/// <summary>Gets the connection flags associated with the event.</summary>
		public int ConnectFlags { get => m_connectFlags; }

		/// <summary>Gets the optional parameters associated with the event.</summary>
		public object[] Params { get => m_params; }

		#endregion Fields & Properties

		public EventProcessingElement(EventProcessingType p_type) {
			m_processType = p_type;
		}


		/// <summary>
		/// Adds the type of the event to the processing element.
		/// </summary>
		/// <param name="p_type">The type of the event.</param>
		/// <returns>The current instance of <see cref="EventProcessingElement"/> for method chaining.</returns>
		public EventProcessingElement AddType(Type p_type) {
			m_type = p_type;
			return this;
		}

		/// <summary>
		/// Adds the delegate to handle the event to the processing element.
		/// </summary>
		/// <param name="p_delegate">The delegate to handle the event.</param>
		/// <returns>The current instance of <see cref="EventProcessingElement"/> for method chaining.</returns>
		public EventProcessingElement AddDelegate(Delegate p_delegate) {
			m_delegate = p_delegate;
			return this;
		}

		/// <summary>
		/// Adds the connection flags to the processing element.
		/// </summary>
		/// <param name="p_flags">The connection flags.</param>
		/// <returns>The current instance of <see cref="EventProcessingElement"/> for method chaining.</returns>
		public EventProcessingElement AddConnectFlags(int p_flags) {
			m_connectFlags = p_flags;
			return this;
		}

		/// <summary>
		/// Adds optional parameters to the processing element.
		/// </summary>
		/// <param name="p_params">The optional parameters.</param>
		/// <returns>The current instance of <see cref="EventProcessingElement"/> for method chaining.</returns>
		public EventProcessingElement AddParams(object[] p_params) {
			m_params = p_params;
			return this;
		}

	}
}
