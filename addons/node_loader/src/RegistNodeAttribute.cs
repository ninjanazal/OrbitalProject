using System;
using System.Runtime.CompilerServices;
using Godot;

namespace Orbital {
	[AttributeUsage(AttributeTargets.Class)]
	public class RegistNodeAttribute : Attribute {
		#region Constants

		private const string k_EDITOR_THEME_NAME = "EditorIcons";

		#endregion Constants
		#region Fields

		private readonly Type m_type;
		private Type m_baseType;
		private readonly string m_iconPath;
		private readonly string m_scriptPath;


		/// <summary>
		/// Gets the name of the registered type.
		/// </summary>
		public string typeName {
			get => m_type.Name;
		}

		/// <summary>
		/// Gets the name of the base type.
		/// </summary>
		public string baseTypeName {
			get => m_baseType.Name;
		}

		#endregion Fields
		#region Public


		/// <summary>
		/// Initializes a new instance of the <see cref="RegistNodeAttribute"/> class.
		/// </summary>
		/// <param name="p_type">The type of the node being registered.</param>
		/// <param name="p_base">The base type of the node. Defaults to <see cref="Node"/> or <see cref="Resource"/> if null.</param>
		/// <param name="p_iconPath">Path to the custom icon for the node.</param>
		/// <param name="p_callerPath">Path to the script file (auto-detected).</param>
		public RegistNodeAttribute(Type p_type, Type p_base = null, string p_iconPath = "", [CallerFilePath] string p_callerPath = "") {
			m_type = p_type;
			m_baseType = p_base;
			m_iconPath = p_iconPath;

			m_scriptPath = p_callerPath;
			if (!string.IsNullOrEmpty(m_scriptPath)) {
				m_scriptPath = ProjectSettings.LocalizePath(m_scriptPath);
			}

			ParseBaseType();
		}


		/// <summary>
		/// Retrieves the icon for the registered node.
		/// </summary>
		/// <returns>The icon as a <see cref="Texture2D"/>.</returns>
		public Texture2D GetIcon() {
			if (string.IsNullOrEmpty(m_iconPath) || !FileAccess.FileExists(m_iconPath)) {
				return EditorInterface.Singleton.GetBaseControl().GetThemeIcon(m_baseType.Name, k_EDITOR_THEME_NAME);
			}

			return ResourceLoader.Load<Texture2D>(m_iconPath, nameof(Texture2D));
		}


		/// <summary>
		/// Retrieves the script associated with the registered node.
		/// </summary>
		/// <returns>The script as a <see cref="Script"/>, or null if not found.</returns>
		public Script GetScript() {
			if (FileAccess.FileExists(m_scriptPath)) {
				return ResourceLoader.Load<Script>(m_scriptPath, nameof(Script));
			}
			return null;
		}
		#endregion Public
		#region Private


		/// <summary>
		/// Determines the base type for the registered node.
		/// Defaults to <see cref="Node"/> if the type is a node, otherwise defaults to <see cref="Resource"/>.
		/// </summary>
		private void ParseBaseType() {
			if (m_baseType == null) {
				m_baseType = typeof(Node).IsAssignableFrom(m_type) ? typeof(Node) : typeof(Resource);
			}
		}

		#endregion Private
	}
}

