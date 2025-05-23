using System.Linq;
using Godot;
using Godot.Collections;

namespace Orbital {
	/// <summary>
	/// Provides extension methods for working with Godot's Array type.
	/// </summary>
	public static class ArrayExtensions {

		/// <summary>
		/// Converts a variable number of objects into a Array of Variants.
		/// </summary>
		/// <param name="p_params">A variable number of objects to be converted into Variants.</param>
		/// <returns>A Array containing the provided objects as Variants.</returns>
		public static Variant[] ToVariant(params object[] p_params) {
			Array r = new Array();
			foreach (var item in p_params) {
				_ = r.Append((Variant)item);
			}

			return [.. r];
		}


		/// <summary>
		/// Merges two arrays into a new array. The second array's elements are appended to the first.
		/// </summary>
		/// <param name="p_rArray">The right-hand array to append to the left array.</param>
		public static void Merge<[MustBeVariant] T>(this Array<T> p_lArray, Array<T> p_rArray) {
			if (p_rArray != null) {
				foreach (T item in p_rArray) { _ = p_lArray.Append(item); }
			}
		}


		/// <summary>
		/// Merges two arrays of the same type into a new array.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the arrays.</typeparam>
		/// <param name="p_lArray">The left (or first) array.</param>
		/// <param name="p_rArray">The right (or second) array.</param>
		public static T[] Merge<T>(this T[] p_lArray, T[] p_rArray) {
			T[] r = new T[p_lArray.Length + p_rArray.Length];

			for (int i = 0; i < p_lArray.Length; i++) {
				r[i] = p_lArray[i];
			}
			for (int i = 0; i < p_rArray.Length; i++) {
				r[p_lArray.Length + i] = p_rArray[i];
			}

			return r;
		}

		/// <summary>
		/// Converts a generic Array of strings to a single string with elements separated by a specified separator.
		/// </summary>
		/// <param name="p_value">The Array of strings to join.</param>
		/// <param name="p_separator">The string to use as a separator between elements.</param>
		/// <returns>A single concatenated string of all array elements separated by the given separator.</returns>
		public static string ToStringList(this Array<string> p_value, string p_separator) {
			string r = "";
			for (int i = 0; i < p_value.Count; i++) {
				r += p_value[i];
				if (p_value.Count != i + 1) {
					r += p_separator;
				}
			}
			return r;
		}

		/// <summary>
		/// Converts a string array to a single string with elements separated by a specified separator.
		/// </summary>
		/// <param name="p_value">The string array to join.</param>
		/// <param name="p_separator">The string to use as a separator between elements.</param>
		/// <returns>A single concatenated string of all array elements separated by the given separator.</returns>
		public static string ToStringList(this string[] p_value, string p_separator) {
			string r = "";
			for (int i = 0; i < p_value.Length; i++) {
				r += p_value[i];
				if (p_value.Length != i + 1) {
					r += p_separator;
				}
			}
			return r;
		}
	}
}
