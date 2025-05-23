using System;
using System.Collections.Generic;

namespace Orbital {
	/// <summary>
	/// Represents a finite state machine (FSM) that manages different states and transitions between them.
	/// </summary>
	public class StateMachine {
		#region Fields & Properties

		/// <summary>
		/// The name of the state machine.
		/// </summary>
		private readonly string m_name;

		/// <summary>
		/// Dictionary storing the states, mapped by their type.
		/// </summary>
		private readonly Dictionary<Type, State> m_states;

		/// <summary>
		/// The current active state.
		/// </summary>
		private State m_current;

		/// <summary>
		/// The previous state before the last transition.
		/// </summary>
		private State m_previous;

		/// <summary>
		/// Gets the name of the state machine.
		/// </summary>
		public string Name { get => m_name; }

		/// <summary>
		/// Gets the current active state.
		/// </summary>
		public State CurrentState { get => m_current; }

		/// <summary>
		/// Gets the previous state before the last transition.
		/// </summary>
		public State PreviousState { get => m_previous; }


		#endregion Fields & Properties
		#region  Public

		/// <summary>
		/// Initializes a new instance of the <see cref="StateMachine"/> class with a given name.
		/// </summary>
		/// <param name="p_name">The name of the state machine.</param>
		public StateMachine(string p_name) {
			m_name = p_name;

			m_states = new Dictionary<Type, State>();
			m_current = null;
			m_previous = null;

			Logger.Trace("FSM Created", $"Created {m_name}");
		}


		/// <summary>
		/// Adds a new state to the state machine.
		/// </summary>
		/// <typeparam name="T">The type of the state to add.</typeparam>
		public void AddState<T>() where T : State {
			Type tType = typeof(T);
			if (m_states.ContainsKey(tType)) {
				Logger.Warn("Duplicated state", $"{tType.Name} already exists");
				return;
			}

			T instance = (T)Activator.CreateInstance<T>();
			instance.SetOwner(this);
			m_states.Add(tType, instance);
		}


		/// <summary>
		/// Starts the state machine with an initial state.
		/// </summary>
		/// <typeparam name="T">The type of the initial state.</typeparam>
		public void Start<T>() {
			Type tType = typeof(T);
			if (!m_states.ContainsKey(tType)) {
				Logger.Error("State not found", $"Failed to start on {tType.Name}");
				return;
			}

			m_current = m_states[tType];
			m_previous = null;

			Logger.Trace("Booting State machine", $"Boot state {m_current.GetType().Name}");
			m_current.OnEnter();
		}


		/// <summary>
		/// Transitions to the next state if the transition is valid.
		/// </summary>
		/// <typeparam name="T">The type of the state to transition to.</typeparam>
		public void NextState<T>() where T : State {
			Type tType = typeof(T);
			if (!m_states.ContainsKey(tType)) {
				Logger.Error("State not found", $"State transition failed - {tType.Name}");
				return;
			}

			if (!m_current.ValidTransition(tType)) {
				Logger.Error("Invalid transition", tType.Name);
				return;
			}

			m_previous = m_current;
			m_current.OnExit();

			m_current = m_states[tType];
			m_current.OnEnter();
		}
		#endregion  Public
	}
}
