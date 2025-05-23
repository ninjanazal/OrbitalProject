using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;

namespace Orbital {
#if TOOLS
	[Tool]
	public partial class NodeLoader : EditorPlugin, ISerializationListener {
		#region Fields

		// List of registered node attributes for cleanup on exit.
		private readonly List<RegistNodeAttribute> m_registeredTypes = new();

		#endregion Fields
		#region Public


		/// <summary>
		/// Initializes a new instance of the <see cref="NodeLoader"/> class.
		/// </summary>
		public NodeLoader() { }


		/// <summary>
		/// Called when the plugin is enabled (enters the editor tree).
		/// Registers all custom nodes.
		/// </summary>
		public override void _EnterTree() {
			UpdateNodes();
		}


		/// <summary>
		/// Called when the plugin is disabled (exits the editor tree).
		/// Unregisters all custom nodes.
		/// </summary>
		public override void _ExitTree() {
			ClearNodes();
		}


		/// <summary>
		/// Called after deserialization, ensuring nodes are updated.
		/// </summary>
		public void OnAfterDeserialize() {
			UpdateNodes();
		}


		/// <summary>
		/// Called before serialization (not used but required by the interface).
		/// </summary>
		public void OnBeforeSerialize() { }

		#endregion Public
		#region Private


		/// <summary>
		/// Scans the assembly for types with the <see cref="RegistNodeAttribute"/>
		/// and registers them as custom Godot nodes.
		/// </summary>
		private void UpdateNodes() {
			Assembly assembly = Assembly.GetAssembly(typeof(RegistNodeAttribute));

			foreach (Type item in assembly.GetTypes()) {
				if (Attribute.IsDefined(item, typeof(RegistNodeAttribute))) {
					RegistNodeAttribute atr = (RegistNodeAttribute)Attribute.GetCustomAttribute(item, typeof(RegistNodeAttribute));

					AddCustomType(item.Name, atr.baseTypeName, atr.GetScript(), atr.GetIcon());
					m_registeredTypes.Add(atr);
					Logger.Info("📥 Custom Node Registered", atr.typeName);
				}
			}
		}


		/// <summary>
		/// Removes all previously registered custom nodes from the editor.
		/// </summary>
		private void ClearNodes() {
			foreach (RegistNodeAttribute item in m_registeredTypes) {
				RemoveCustomType(item.typeName);
				Logger.Info("🗑 Custom Node Unregistered", item.typeName);
			}
			m_registeredTypes.Clear();
		}

		#endregion Private
	}
#endif
}
