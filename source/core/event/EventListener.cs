using System;
using System.Linq;
using System.Reflection;
using Godot;
using Godot.Collections;

namespace Orbital {
	/// <summary>
	/// The EventListener class is responsible for managing event subscriptions and raising events.
	/// It encapsulates a delegate, its target object, and method information, along with flags
	/// to control event behavior such as deferred execution or one-shot events.
	/// </summary>
	public partial class EventListener : GodotObject {
		#region Fields

		/// <summary>
		/// The target object to which the event is bound.
		/// </summary>
		private readonly object m_target;

		/// <summary>
		/// The method information of the event handler.
		/// </summary>
		private readonly MethodInfo m_methodInfo;

		/// <summary>
		/// Flags that control event behavior (e.g., deferred, one-shot).
		/// </summary>
		private readonly int m_flags;

		/// <summary>
		/// The delegate representing the event handler.
		/// </summary>
		private readonly Delegate m_call;

		/// <summary>
		/// Indicates whether this listener is a watcher (e.g., for monitoring purposes).
		/// </summary>
		private readonly bool m_watcher;


		/// <summary>
		/// Fix data to be sent upon raise call, this will be appended to the params
		/// </summary>
		private readonly object[] m_binds;

		/// <summary>
		/// Gets the target object to which the event is bound.
		/// </summary>
		public object Target { get => m_target; }

		/// <summary>
		/// Gets the method information of the event handler.
		/// </summary>
		public MethodInfo MethodInfo { get => m_methodInfo; }

		#endregion Fields
		#region Public

		public EventListener(Delegate p_delegate, int p_flag, bool p_watcher, params object[] p_binds) {
			m_call = p_delegate;
			m_target = p_delegate.Target;
			m_methodInfo = p_delegate.Method;
			m_flags = p_flag;
			m_watcher = p_watcher;
			m_binds = p_binds;
		}


		public bool Equals(EventListener p_listener) {
			return m_target == p_listener.Target && m_methodInfo == p_listener.MethodInfo;
		}


		public bool Equals(object p_target, MethodInfo p_method) {
			return m_target == p_target && m_methodInfo == p_method;
		}


		/// <summary>
		/// Checks if this event is a one-shot event.
		/// </summary>
		/// <returns>True if the event is one-shot; otherwise, false.</returns>
		public bool IsOneShot() {
			return (m_flags & (int)Node.ConnectFlags.OneShot) != 0;
		}

		/// <summary>
		/// Checks if this event is deferred.
		/// </summary>
		/// <returns>True if the event is deferred; otherwise, false.</returns>
		public bool IsDeferred() {
			return (m_flags & (int)Node.ConnectFlags.Deferred) != 0;
		}

		/// <summary>
		/// Checks if this listener is a watcher.
		/// </summary>
		/// <returns>True if this listener is a watcher; otherwise, false.</returns>
		public bool IsWatcher() {
			return m_watcher;
		}


		/// <summary>
		/// Raises the event with the provided parameters.
		/// </summary>
		/// <param name="p_params">The parameters to pass to the event handler.</param>
		public void Raise(params object[] p_params) {
			if (m_binds.Length != 0) {
				p_params = p_params.Merge(m_binds);
			}

			if (ValidateParams(p_params)) {
				if (IsDeferred() && m_target.GetType().IsAssignableTo(typeof(GodotObject))) {
					((GodotObject)m_target).CallDeferred(m_methodInfo.Name, ArrayExtensions.ToVariant(p_params));
					return;
				}
				m_call.GetMethodInfo().Invoke(m_target, p_params);
			}
		}


		#endregion Public
		#region Private


		/// <summary>
		/// Validates the parameters passed to the event handler.
		/// </summary>
		/// <param name="p_params">The parameters to validate.</param>
		/// <returns>True if the parameters are valid; otherwise, false.</returns>
		private bool ValidateParams(params object[] p_params) {
			MethodInfo callInfo = m_call.GetMethodInfo();
			ParameterInfo[] paramsInfo = callInfo.GetParameters();

			for (int i = 0; i < paramsInfo.Length; i++) {
				ParameterInfo bParam = paramsInfo[i];
				if (p_params.Length <= i) {
					if (bParam.IsOptional) {
						break;
					} else {
						Logger.Error("Failed to Raise event", "Invalid param count");
						return false;
					}
				}

				object vParam = p_params[i];

				if (!vParam.GetType().IsAssignableFrom(bParam.ParameterType) &&
						!TypeExtensions.HasImplicitConversion(bParam.ParameterType, vParam.GetType())) {
					Logger.Error("Failed to Raise event", "Mismatch param type");
					return false;
				}
			}
			return true;
		}

		#endregion Private
	}
}
