using Godot;
using Godot.Collections;

namespace Orbital {
	public partial class EventAnimationPair : Resource {
		#region Constants
		private const string k_AVAILABLE_ANIMATIONS = "AvailableAnimations";

		private const string k_ANIMATION_NAME = "Animation";

		private const string k_EVENT_RESOURCE = "EventResource";

		#endregion Constants
		#region Public

		public override Array<Dictionary> _GetPropertyList() {
			Array<Dictionary> r = new Array<Dictionary>() {
				GodotProperties.CreateStringListProperty(k_AVAILABLE_ANIMATIONS,
					m_availableAnimations.ToStringList(","),PropertyUsageFlags.Editor
				),
				GodotProperties.CreateStringProperty(k_ANIMATION_NAME, PropertyUsageFlags.Storage),

				GodotProperties.CreateResource(k_EVENT_RESOURCE, nameof(EventResource)),

			};

			r.Merge(base._GetPropertyList());
			return r;
		}


		public override Variant _Get(StringName p_property) {
			(bool, Variant) r = HandleGet(p_property);
			if (r.Item1) { return r.Item2; }

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
		#region  Private


		private (bool, Variant) HandleGet(StringName p_property) {
			switch (p_property) {
				case k_AVAILABLE_ANIMATIONS:
				case k_ANIMATION_NAME:
					return (true, m_animationName);

				case k_EVENT_RESOURCE:
					if (m_eventResource == null) { m_eventResource = new EventResource(); }
					return (true, m_eventResource);
			}
			return (false, new GodotObject());
		}


		private bool HandleSet(StringName p_property, Variant p_value) {
			switch (p_property) {
				case k_AVAILABLE_ANIMATIONS:
				case k_ANIMATION_NAME:
					m_animationName = (string)p_value;
					return true;

				case k_EVENT_RESOURCE:
					m_eventResource = (EventResource)p_value;
					if (m_eventResource == null) {
						m_eventResource = new EventResource();
					}
					return true;


			}

			return false;
		}

		#endregion Private
	}
}
