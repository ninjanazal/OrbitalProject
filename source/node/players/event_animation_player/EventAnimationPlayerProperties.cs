using Godot;
using Godot.Collections;

namespace Orbital {
	public partial class EventAnimationPlayer : AnimationPlayer {
		#region Field & Properties

		private const string k_HOTPLAY = "HotPlay";
		private const string k_EVENT_CONNECTIONS = "EventConnections";

		#endregion Field & Properties
		#region Public


		public override Array<Dictionary> _GetPropertyList() {
			Array<Dictionary> r = new Array<Dictionary>() {
				GodotProperties.CreateBoolProperty(k_HOTPLAY),
				GodotProperties.CreateResourceArray(k_EVENT_CONNECTIONS, nameof(EventAnimationPair))
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
				case k_HOTPLAY:
					return (true, m_hotPlay);

				case k_EVENT_CONNECTIONS:
					CheckForNullPairs();
					return (true, m_animationPairs);
			}
			return (false, new GodotObject());
		}


		private bool HandleSet(StringName p_property, Variant p_value) {
			switch (p_property) {
				case k_HOTPLAY:
					m_hotPlay = (bool)p_value;
					return true;

				case k_EVENT_CONNECTIONS:
					m_animationPairs = (Array<EventAnimationPair>)p_value;
					CheckForNullPairs();
#if TOOLS
					UpdateAnimationNames();
#endif
					return true;
			}
			return false;
		}


		private void CheckForNullPairs() {
			for (int i = 0; i < m_animationPairs.Count; i++) {
				if (m_animationPairs[i] == null) {
					m_animationPairs[i] = new EventAnimationPair();
				}
			}
		}


		private void UpdateAnimationNames() {
#if TOOLS
			for (int i = 0; i < m_animationPairs.Count; i++) {
				m_animationPairs[i].SetAvailableAnimations(new Array<string>(GetAnimationList()));
			}
#endif
		}

		#endregion Private
	}
}
