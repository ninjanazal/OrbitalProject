using System;
using Godot;
using Godot.Collections;

namespace Orbital {
	public partial class EventResource : Resource {
		#region Constants

		private const string k_COLLECTION_NAME = "Collection";
		private const string k_EVENT_NAME = "EventName";
		private const string k_EVENT_DESCRIPTION = "EventDescription";

		#endregion Constants
		#region Public

		public override Array<Dictionary> _GetPropertyList() {
			Array<Dictionary> r = new Array<Dictionary>() {
				GodotProperties.CreateCategory(nameof(EventResource)),

				GodotProperties.CreateStringListProperty(k_COLLECTION_NAME,
							EventManager.GetCollections().ToStringList(","))
			};

			if (!string.IsNullOrEmpty(m_category)) {
				r.Add(GodotProperties.CreateStringListProperty(k_EVENT_NAME,
							EventManager.GetEventsNames(m_category).ToStringList(",")));
			}

			if (m_eventType != null && !string.IsNullOrEmpty(m_description)) {
				r.Add(GodotProperties.CreateMultilineString(k_EVENT_DESCRIPTION,
					m_description, PropertyUsageFlags.Editor));
			}

			r.Merge(base._GetPropertyList());
			return r;
		}


		public override Variant _Get(StringName p_property) {
			(bool, Variant) result = HandleGet(p_property);
			if (result.Item1) { return result.Item2; }

			return base._Get(p_property);
		}


		public override bool _Set(StringName p_property, Variant p_value) {
			if (HandleSet(p_property, p_value)) {
#if TOOLS
				NotifyPropertyListChanged();
#endif
				return true;
			}

			return base._Set(p_property, p_value);

		}

		#endregion Public
		#region Private

		private (bool, Variant) HandleGet(StringName p_property) {
			switch (p_property) {
				case k_COLLECTION_NAME:
					string value = m_category;
					return (true, value);

				case k_EVENT_NAME:
					return (true, m_eventName);

				case k_EVENT_DESCRIPTION:
					return (true, m_description);
			}

			return (false, new GodotObject());
		}


		private bool HandleSet(StringName p_property, Variant p_value) {
			switch (p_property) {
				case k_COLLECTION_NAME:
					m_category = (string)p_value;
					return true;

				case k_EVENT_NAME:
					m_eventName = (string)p_value;
					m_eventType = EventManager.GetTypeFromName(m_category, m_eventName);
					m_description = EventManager.GetEventDescription(m_eventType);
					return true;

				case k_EVENT_DESCRIPTION:
					m_description = EventManager.GetEventDescription(m_eventType);
					return true;

				default:
					return false;
			}

		}

		#endregion Private
	}
}
