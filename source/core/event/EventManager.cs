using System;
using System.Collections.Generic;
using System.Reflection;

namespace Orbital {
	/// <summary>
	/// The EventManager class is a singleton responsible for managing event subscriptions, dispatching events,
	/// and handling event listeners and watchers. It supports registration of event types, listening to events,
	/// watching events, dispatching events, and forgetting event listeners.
	/// </summary>
	public class EventManager {
		#region Fields

		private static readonly object m_lock = new();

		/// <summary>
		/// Singleton instance of the EventManager.
		/// </summary>
		private static EventManager m_instance = null;

		/// <summary>
		/// Thread-safe property to get the singleton instance of the EventManager.
		/// </summary>
		private static EventManager m_self {
			get {
				lock (m_lock) {
					m_instance ??= new EventManager();
					return m_instance;
				}
			}
		}

		/// <summary>
		/// List of all registered event category names
		/// </summary>
		private readonly List<string> m_registeredCategories;

		/// <summary>
		/// Mapping of event categories to their associated event types
		/// </summary>
		private readonly Dictionary<string, List<Type>> m_categoryToEventsMap;

		/// <summary>
		/// Master registry of all event types and their metadata
		/// </summary>
		private readonly Dictionary<Type, RawEventData> m_registeredEvents;

		/// <summary>
		/// Dictionary to store event subscribers, keyed by event type.
		/// </summary>
		private readonly Dictionary<Type, List<EventListener>> m_subscribers;

		/// <summary>
		/// Dictionary to store event watchers, keyed by event type.
		/// </summary>
		private readonly Dictionary<Type, List<EventListener>> m_watchers;

		/// <summary>
		/// Indicates whether the EventManager is currently processing events.
		/// </summary>
		private bool m_processing;

		/// <summary>
		/// Queue to store event processing elements for deferred processing.
		/// </summary>
		private readonly Queue<EventProcessingElement> m_processingQueue;

		#endregion Fields
		#region Public

		/// <summary>
		/// Subscribes a delegate to listen to an event of the specified type.
		/// </summary>
		/// <typeparam name="T">The delegate type representing the event.</typeparam>
		/// <param name="p_delegate">The delegate to subscribe.</param>
		/// <param name="p_connectFlags">Flags to control event behavior (e.g., deferred, one-shot).</param>
		public static void Listen<T>(Delegate p_delegate, int p_connectFlags = 0, params object[] p_binds) where T : Delegate {
			Type tType = typeof(T);

			EventProcessingElement elm = new EventProcessingElement(EventProcessingType.LISTEN);
			elm.AddType(tType).AddDelegate(p_delegate).AddConnectFlags(p_connectFlags).AddParams(p_binds);
			m_self.ToProcess(elm);
		}


		/// <summary>
		/// Subscribes a delegate to watch an event of the specified type.
		/// </summary>
		/// <typeparam name="T">The delegate type representing the event.</typeparam>
		/// <param name="p_delegate">The delegate to subscribe.</param>
		/// <param name="p_connectFlags">Flags to control event behavior (e.g., deferred, one-shot).</param>
		public static void Watch<T>(Delegate p_delegate, int p_connectFlags = 0, params object[] p_binds) where T : Delegate {
			Type tType = typeof(T);

			EventProcessingElement elm = new EventProcessingElement(EventProcessingType.WATCH);
			elm.AddType(tType).AddDelegate(p_delegate).AddConnectFlags(p_connectFlags).AddParams(p_binds);
			m_self.ToProcess(elm);
		}


		/// <summary>
		/// Dispatches an event of the specified type with the provided parameters.
		/// </summary>
		/// <typeparam name="T">The delegate type representing the event.</typeparam>
		/// <param name="p_params">The parameters to pass to the event handlers.</param>
		public static void Dispatch<T>(params object[] p_params) {
			Type tType = typeof(T);

			EventProcessingElement elm = new EventProcessingElement(EventProcessingType.DISPATCH);
			elm.AddType(tType).AddParams(p_params);
			m_self.ToProcess(elm);
		}


		/// <summary>
		/// Removes a delegate from listening to or watching an event of the specified type.
		/// </summary>
		/// <typeparam name="T">The delegate type representing the event.</typeparam>
		/// <param name="p_delegate">The delegate to remove.</param>
		public static void Forget<T>(Delegate p_delegate) where T : Delegate {
			Type tType = typeof(T);

			EventProcessingElement elm = new EventProcessingElement(EventProcessingType.FORGET);
			elm.AddType(tType).AddDelegate(p_delegate);
			m_self.ToProcess(elm);
		}


		/// <summary>
		/// Retrieves a list of all registered collection categories.
		/// </summary>
		public static string[] GetCollections() {
			return m_self.m_registeredCategories.ToArray();
		}


		/// <summary>
		/// Retrieves all event names registered under a specific category/collection
		/// </summary>
		/// <param name="p_collection">Category name to search for registered events</param>
		/// <returns>
		/// String array of event names if category exists, empty array otherwise
		/// </returns>
		public static string[] GetEventsNames(string p_collection) {
			if (m_self.m_categoryToEventsMap.ContainsKey(p_collection)) {
				string[] r = new string[m_self.m_categoryToEventsMap[p_collection].Count];
				for (int i = 0; i < m_self.m_categoryToEventsMap[p_collection].Count; i++) {
					r[i] = m_self.m_categoryToEventsMap[p_collection][i].Name;
				}

				return r;
			}
			return new string[] { };
		}


		/// <summary>
		/// Finds the event Type by its category and name
		/// </summary>
		/// <param name="p_category">Event category/group to search within</param>
		/// <param name="p_eventName">Name of the event type to locate</param>
		/// <returns>
		/// The matching event Type if found, null if no match exists
		/// </returns>
		public static Type GetTypeFromName(string p_category, string p_eventName) {
			if (m_self.m_categoryToEventsMap.ContainsKey(p_category)) {
				foreach (Type eventType in m_self.m_categoryToEventsMap[p_category]) {
					if (eventType.Name == p_eventName) {
						return eventType;
					}
				}
			}

			return null;
		}


		/// <summary>
		/// Retrieves the description text for a registered event type
		/// </summary>
		/// <param name="p_type">System.Type of the event to query</param>
		/// <returns>
		/// Event description if registered, empty string if not found
		/// </returns>
		public static string GetEventDescription(Type p_type) {
			if (m_self.m_registeredEvents.ContainsKey(p_type)) {
				return m_self.m_registeredEvents[p_type].GetEventDescription();
			}

			return string.Empty;
		}


		#endregion Public
		#region Private


		private EventManager() {
			m_registeredCategories = new List<string>();
			m_categoryToEventsMap = new Dictionary<string, List<Type>>();
			m_registeredEvents = new Dictionary<Type, RawEventData>();

			m_subscribers = new Dictionary<Type, List<EventListener>>();
			m_watchers = new Dictionary<Type, List<EventListener>>();


			m_processing = false;
			m_processingQueue = new Queue<EventProcessingElement>();

			Assembly assembly = Assembly.GetAssembly(typeof(EventManager));

			foreach (Type item in assembly.GetTypes()) {
				if (Attribute.IsDefined(item, typeof(EventDelegateAttribute))) {
					EventDelegateAttribute attribute = (EventDelegateAttribute)Attribute.GetCustomAttribute(item, typeof(EventDelegateAttribute));
					Regist(attribute.GetEventType());

					RawEventData rawEvent = new RawEventData(attribute.GetEventType(), attribute.GetCategory(), attribute.GetDescription());
					Type eventType = attribute.GetEventType();
					if (!m_registeredEvents.ContainsKey(eventType)) {
						m_registeredEvents.Add(eventType, rawEvent);

						if (!m_registeredCategories.Contains(attribute.GetCategory())) {
							m_registeredCategories.Add(attribute.GetCategory());
							m_categoryToEventsMap.Add(attribute.GetCategory(), new List<Type>());
						}

						if (!m_categoryToEventsMap[attribute.GetCategory()].Contains(eventType)) {
							m_categoryToEventsMap[attribute.GetCategory()].Add(eventType);
						}
					}

				}
			}

		}

		/// <summary>
		/// Registers an event type for use with the EventManager.
		/// </summary>
		/// <typeparam name="T">The delegate type representing the event.</typeparam>
		private void Regist(Type p_delegateType) {
			EventProcessingElement elm = new EventProcessingElement(EventProcessingType.REGIST);
			elm.AddType(p_delegateType);
			ToProcess(elm);
		}


		/// <summary>
		/// Validates the parameters of a delegate against the expected parameters of an event type.
		/// </summary>
		/// <param name="p_type">The event type to validate against.</param>
		/// <param name="p_toValidate">The delegate to validate.</param>
		/// <returns>True if the delegate parameters match the event type parameters; otherwise, false.</returns>
		private static bool ValidateParams(Type p_type, Delegate p_toValidate) {
			MethodInfo pBase = p_type.GetMethod("Invoke");

			ParameterInfo[] paramsBase = pBase.GetParameters();
			ParameterInfo[] paramsToValidate = p_toValidate.GetMethodInfo().GetParameters();

			if (paramsToValidate.Length != paramsBase.Length) {
				Logger.Error("Failed on validate", "Mismatch param count");
				return false;
			}

			for (int i = 0; i < paramsBase.Length; i++) {
				ParameterInfo bParam = paramsBase[i];
				ParameterInfo vParam = paramsToValidate[i];
				if (!vParam.ParameterType.IsAssignableFrom(bParam.ParameterType)) {
					Logger.Error("Failed to validate params", "Mismatch param type");
					return false;
				}
			}

			return true;
		}


		/// <summary>
		/// Adds an event processing element to the queue and starts processing if not already processing.
		/// </summary>
		/// <param name="p_element">The event processing element to add to the queue.</param>
		private void ToProcess(EventProcessingElement p_element) {
			m_processingQueue.Enqueue(p_element);
			if (!m_processing) {
				Resolve();
			}
		}


		/// <summary>
		/// Processes all queued event processing elements.
		/// </summary>
		private void Resolve() {
			m_processing = true;
			while (m_processingQueue.Count != 0) {
				EventProcessingElement elm = m_processingQueue.Dequeue();

				switch (elm.ProcessType) {
					case EventProcessingType.REGIST:
						ResolveRegist(elm.EventType);
						break;

					case EventProcessingType.LISTEN:
						ResolveListen(elm.EventType, elm.EventDelegate, elm.ConnectFlags, elm.Params);
						break;

					case EventProcessingType.WATCH:
						ResolveWatch(elm.EventType, elm.EventDelegate, elm.ConnectFlags, elm.Params);
						break;

					case EventProcessingType.DISPATCH:
						ResolveDispatch(elm.EventType, elm.Params);
						break;

					case EventProcessingType.FORGET:
						ResolveForget(elm.EventType, elm.EventDelegate);
						break;
				}
			}

			m_processing = false;
		}


		/// <summary>
		/// Registers a new event type in the EventManager.
		/// </summary>
		/// <param name="p_type">The event type to register.</param>
		private void ResolveRegist(Type p_type) {
			if (!m_subscribers.ContainsKey(p_type)) {
				m_subscribers.Add(p_type, new List<EventListener>());
				m_watchers.Add(p_type, new List<EventListener>());

				Logger.Info("Registered signal", p_type.Name);
			} else {
				Logger.Warn("Duplicated signal", p_type.Name);
			}
		}


		/// <summary>
		/// Adds a listener for a specific event type.
		/// </summary>
		/// <param name="p_type">The event type to listen to.</param>
		/// <param name="p_delegate">The delegate to subscribe as a listener.</param>
		/// <param name="p_connectFlags">Flags to control event behavior (e.g., deferred, one-shot).</param>
		private void ResolveListen(Type p_type, Delegate p_delegate, int p_connectFlags, params object[] p_binds) {
			if (!ValidateParams(p_type, p_delegate)) {
				Logger.Error("Failed to validate params", $"@ {p_type.Name}");
				return;
			}

			if (!m_subscribers.TryGetValue(p_type, out List<EventListener> value)) {
				Logger.Warn("Listen type not found", p_type.Name);
				return;
			}

			EventListener listener = new EventListener(p_delegate, p_connectFlags, p_watcher: false, p_binds);

			if (value.Contains(listener)) {
				Logger.Warn("Duplicated listener", p_delegate.Method.Name);
				return;
			}

			value.Add(listener);
		}


		/// <summary>
		/// Adds a watcher for a specific event type.
		/// </summary>
		/// <param name="p_type">The event type to watch.</param>
		/// <param name="p_delegate">The delegate to subscribe as a watcher.</param>
		/// <param name="p_connectFlags">Flags to control event behavior (e.g., deferred, one-shot).</param>
		private void ResolveWatch(Type p_type, Delegate p_delegate, int p_connectFlags, params object[] p_binds) {
			if (!m_subscribers.ContainsKey(p_type)) {
				Logger.Warn("Listen type not found", p_type.Name);
				return;
			}

			EventListener watcher = new EventListener(p_delegate, p_connectFlags, p_watcher: true, p_binds);
			if (m_watchers[p_type].Contains(watcher)) {
				Logger.Warn("Duplicated watcher", p_delegate.Method.Name);
				return;
			}

			m_watchers[p_type].Add(watcher);
		}


		/// <summary>
		/// Dispatches an event of the specified type with the provided parameters.
		/// </summary>
		/// <param name="p_type">The event type to dispatch.</param>
		/// <param name="p_params">The parameters to pass to the event handlers.</param>
		private void ResolveDispatch(Type p_type, params object[] p_params) {
			if (!m_subscribers.ContainsKey(p_type)) {
				Logger.Warn("Listen type not found", p_type.Name);
				return;
			}

			List<EventListener> lSubs = m_self.m_subscribers[p_type];
			for (int i = lSubs.Count - 1; i >= 0; i--) {
				EventListener evt = lSubs[i];
				evt.Raise(p_params);

				if (evt.IsOneShot()) {
					lSubs.RemoveAt(i);
				}
			}

			if (!m_self.m_watchers.TryGetValue(p_type, out List<EventListener> value)) {
				Logger.Warn("Listen type not found", p_type.Name);
				return;
			}

			List<EventListener> lWats = value;
			for (int i = lWats.Count - 1; i >= 0; i--) {
				EventListener evt = lWats[i];
				evt.Raise();

				if (evt.IsOneShot()) {
					lWats.RemoveAt(i);
				}
			}
		}


		/// <summary>
		/// Removes a listener or watcher for a specific event type.
		/// </summary>
		/// <param name="p_type">The event type to forget.</param>
		/// <param name="p_delegate">The delegate to remove.</param>
		private void ResolveForget(Type p_type, Delegate p_delegate) {
			if (!m_subscribers.ContainsKey(p_type)) {
				Logger.Warn("Listen type not found", p_type.Name);
				return;
			}

			List<EventListener> tListeners = m_subscribers[p_type];
			for (int i = 0; i < tListeners.Count; i++) {
				EventListener listener = tListeners[i];
				if (listener.Equals(p_delegate.Target, p_delegate.Method)) {
					tListeners.RemoveAt(i);
					break;
				}
			}

			List<EventListener> tWatchers = m_watchers[p_type];
			for (int i = 0; i < tWatchers.Count; i++) {
				EventListener watcher = tWatchers[i];
				if (watcher.Equals(p_delegate.Target, p_delegate.Method)) {
					tListeners.RemoveAt(i);
					break;
				}
			}
		}


		#endregion Private
	}
}
