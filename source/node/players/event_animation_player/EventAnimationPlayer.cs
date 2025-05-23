using Godot;
using Godot.Collections;

namespace Orbital {
	/// <summary>
	/// A specialized <see cref="AnimationPlayer"/> that plays animations in response to events
	/// via the <see cref="EventManager"/> system. Supports automatic connection and disconnection
	/// of events using reflection and generic method binding.
	/// </summary>
	[Tool]
	[RegistNode(typeof(EventAnimationPlayer), typeof(AnimationPlayer))]
	public partial class EventAnimationPlayer : AnimationPlayer {
		#region Field & Properties

		/// <summary>
		/// Holds a list of pairs that associate specific events with animation names.
		/// </summary>
		private Array<EventAnimationPair> m_animationPairs;

		/// <summary>
		/// When true, enables hot-reloading play behavior.
		/// </summary>
		private bool m_hotPlay = false;

		#endregion Field & Properties
		#region Public

		public EventAnimationPlayer() {
			m_animationPairs = new Array<EventAnimationPair>();
		}


		public override void _EnterTree() {
#if TOOLS
			if (Engine.IsEditorHint()) {
				AnimationListChanged += UpdateAnimationNames;
				return;
			}
#endif
			ConnectEvents();
		}

		public override void _ExitTree() {
#if TOOLS
			if (Engine.IsEditorHint()) {
				AnimationListChanged -= UpdateAnimationNames;
				return;
			}
#endif
			DisconnectEvents();
		}


		/// <summary>
		/// Plays the animation associated with the given name.
		/// Typically used as a callback from the event system.
		/// </summary>
		public void EventPlay(string p_animName) {
			if (m_hotPlay && IsPlaying()) {
				Stop();
			}

			base.Play(p_animName);
		}

		#endregion Public
		#region Private


		/// <summary>
		/// Connects each event specified in <see cref="m_animationPairs"/> to the
		/// <see cref="EventPlay"/> method using the <see cref="EventManager"/>.
		/// Uses reflection to bind the correct generic method type.
		/// </summary>
		private void ConnectEvents() {
			for (int i = 0; i < m_animationPairs.Count; i++) {
				EventAnimationPair pair = m_animationPairs[i];
				System.Reflection.MethodInfo watchMethod = typeof(EventManager).GetMethod(nameof(EventManager.Watch));

				watchMethod.MakeGenericMethod(pair.EventType).Invoke(
					null, [
						(System.Delegate)EventPlay, 			// Callback delegate (Cast for warning)
						0,																// ConnectFlags
						new object[] { pair.Animation } 	// Bind Params
					]);
			}
		}


		/// <summary>
		/// Disconnects all events connected via <see cref="ConnectEvents"/> to avoid memory leaks or lingering references.
		/// </summary>
		private void DisconnectEvents() {
			for (int i = 0; i < m_animationPairs.Count; i++) {
				EventAnimationPair pair = m_animationPairs[i];
				System.Reflection.MethodInfo forgetMethod = typeof(EventManager).GetMethod(nameof(EventManager.Forget));

				forgetMethod.MakeGenericMethod(pair.EventType).Invoke(
					null, [
						(System.Delegate)EventPlay	// Callback delegate (Cast for warning)
					]);
			}
		}




		#endregion Private
	}
}
