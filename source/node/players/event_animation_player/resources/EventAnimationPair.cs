using System;
using Godot;
using Godot.Collections;

namespace Orbital {
	/// <summary>
	/// A resource that maps an event type to a specific animation name.
	/// Used by <see cref="EventAnimationPlayer"/> to play animations when specific events occur.
	/// </summary>
	[Tool]
	[RegistNode(typeof(EventAnimationPair), typeof(Resource))]
	public partial class EventAnimationPair : Resource {
		#region Field & Properties

		/// <summary>
		/// A list of valid animation names that can be assigned.
		/// Used for validation when setting the animation name.
		/// </summary>
		private Array<string> m_availableAnimations;

		/// <summary>
		/// The name of the animation associated with the event.
		/// </summary>
		private string m_animationName;

		/// <summary>
		/// The event resource that defines the event type this pair listens for.
		/// </summary>
		private EventResource m_eventResource;

		/// <summary>
		/// Gets the animation name associated with the event.
		/// </summary>
		public string Animation { get => m_animationName; }

		/// <summary>
		/// Gets the type of the event associated with this animation.
		/// Delegated through <see cref="EventResource.GetEventType"/>.
		/// </summary>
		public Type EventType { get => m_eventResource.GetEventType(); }

		#endregion Field & Properties
		#region Public

		public EventAnimationPair() {
			m_availableAnimations = new Array<string>();
		}


		/// <summary>
		/// Sets the list of available animations for validation and selection.
		/// If the current animation is no longer valid, it will be cleared.
		/// In editor mode, notifies that the property list has changed.
		/// </summary>
		/// <param name="p_animations">An array of available animation names.</param>
		public void SetAvailableAnimations(Array<string> p_animations) {
			m_availableAnimations = p_animations;
			if (!m_availableAnimations.Contains(m_animationName)) {
				m_animationName = "";
			}

#if TOOLS
			NotifyPropertyListChanged();
#endif
		}

		#endregion Public
	}
}
