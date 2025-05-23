using System;
using System.Linq;
using System.Reflection;

namespace Orbital {
	public static class TypeExtensions {

		/// <summary>
		/// Determines if an implicit type conversion exists between two types.
		/// </summary>
		/// <param name="baseType">The source type to convert from.</param>
		/// <param name="targetType">The target type to convert to.</param>
		/// <returns>
		/// <c>true</c> if an implicit conversion operator exists that converts from <paramref name="baseType"/>
		/// to <paramref name="targetType"/>; otherwise, <c>false</c>.
		/// </returns>
		/// <remarks>
		/// This method checks for public static methods named "op_Implicit" in the <paramref name="baseType"/>
		/// that meet the following criteria:
		/// <list type="bullet">
		/// <item>Returns the specified <paramref name="targetType"/></item>
		/// <item>Accepts a single parameter of type <paramref name="baseType"/></item>
		/// </list>
		/// Note: Only checks for conversion operators defined in the source type. For bidirectional checks,
		/// call this method with reversed parameters.
		/// </remarks>
		public static bool HasImplicitConversion(Type baseType, Type targetType) {
			return baseType.GetMethods(BindingFlags.Public | BindingFlags.Static)
					.Where(mi => mi.Name == "op_Implicit" && mi.ReturnType == targetType)
					.Any(mi => {
						ParameterInfo pi = mi.GetParameters().FirstOrDefault();
						return pi != null && pi.ParameterType == baseType;
					});
		}
	}
}
