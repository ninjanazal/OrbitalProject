using System;
using System.Linq;

namespace Orbital {
	/// <summary>
	/// Represents a state in a state machine.
	/// </summary>
	public class State {
		#region Fields & Properties

		/// <summary>
		/// The owner state machine that this state belongs to.
		/// </summary>
		protected StateMachine m_owner;

		/// <summary>
		/// List of valid transition states from the current state.
		/// </summary>
		private Type[] m_transitions;

		#endregion Fields & Properties
		#region Public


		/// <summary>
		/// Sets the owner of this state.
		/// </summary>
		/// <param name="p_owner">The state machine that owns this state.</param>
		public void SetOwner(StateMachine p_owner) {
			m_owner = p_owner;
		}


		/// <summary>
		/// Checks if transitioning to the specified state type is allowed.
		/// </summary>
		/// <param name="p_type">The type of state to check.</param>
		/// <returns>True if the transition is valid, false otherwise.</returns>
		public bool ValidTransition(Type p_type) {
			return m_transitions.Contains(p_type);
		}


		/// <summary>
		/// Called when the state is entered.
		/// </summary>
		public virtual void OnEnter() {
			Logger.Trace($"Entered {GetType().Name}");
		}


		/// <summary>
		/// Called when the state is exited.
		/// </summary>
		public virtual void OnExit() { }

		#endregion Public
		#region Protected


		/// <summary>
		/// Defines the valid state transitions for this state.
		/// </summary>
		/// <param name="p_transitions">An array of valid state types.</param>
		protected void AddTransitions(Type[] p_transitions) {
			m_transitions = p_transitions;
		}

		#endregion Protected
	}
}
