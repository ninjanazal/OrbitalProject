using Godot;
using Godot.Collections;

namespace Orbital {
	public static class GodotProperties {
		#region  Public

		public static Dictionary CreateProperty(string p_name,
																														Variant.Type p_type,
																														PropertyHint p_hint,
																														string p_hint_str,
																														PropertyUsageFlags p_usage,
																														string p_className = "") {
			return new Dictionary() {
				{"name", p_name},
				{"type", (int)p_type},
				{"hint", (int)p_hint},
				{"hint_string", p_hint_str},
				{"usage", (int)p_usage},
				{"class_name", p_className}
			};
		}

		public static Dictionary CreateCategory(string p_name) {
			return CreateProperty(p_name, Variant.Type.Nil, PropertyHint.None, "", p_usage: PropertyUsageFlags.Category);
		}

		public static Dictionary CreateIntProperty(string p_name,
																							string p_hint_str = "",
																							PropertyUsageFlags p_usage = PropertyUsageFlags.Default) {
			return CreateProperty(p_name, Variant.Type.Int, PropertyHint.None, p_hint_str, p_usage);
		}

		public static Dictionary CreateBoolProperty(string p_name,
																							PropertyUsageFlags p_usage = PropertyUsageFlags.Default) {
			return CreateProperty(p_name, Variant.Type.Bool, PropertyHint.None, "", p_usage);
		}

		public static Dictionary CreateStringProperty(string p_name,
																									PropertyUsageFlags p_usage = PropertyUsageFlags.Default) {
			return CreateProperty(p_name, Variant.Type.String, PropertyHint.None, "", p_usage);
		}

		public static Dictionary CreateResource(string p_name,
																						string p_hint_str,
																						PropertyUsageFlags p_usage = PropertyUsageFlags.Default) {
			return CreateProperty(p_name, Variant.Type.Object, PropertyHint.ResourceType, p_hint_str, p_usage);
		}


		public static Dictionary CreateStringListProperty(string p_name,
																										string p_hint_str = "",
																										PropertyUsageFlags p_usage = PropertyUsageFlags.Default) {
			return CreateProperty(p_name, Variant.Type.String, PropertyHint.Enum, p_hint_str, p_usage);
		}


		public static Dictionary CreateMultilineString(string p_name,
																									string p_hint_str,
																									PropertyUsageFlags p_usage = PropertyUsageFlags.Default) {
			return CreateProperty(p_name, Variant.Type.String, PropertyHint.MultilineText, p_hint_str, p_usage);
		}


		public static Dictionary CreateResourceArray(string p_name,
																								string p_hint_str,
																								PropertyUsageFlags p_usage = PropertyUsageFlags.Default) {
			return CreateProperty(p_name, Variant.Type.Array, PropertyHint.ArrayType, p_hint_str, p_usage);
		}


		#endregion  Public
	}
}
