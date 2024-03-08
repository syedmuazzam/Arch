using System;

namespace ArchEditor.Utilities;

/// <summary>
/// Utility class for math operations
/// </summary>
internal static class MathUtil
{
	/// <summary>
	/// Compares two floats for equality with a given epsilon
	/// </summary>
	/// <param name="a">
	///  First float to compare
	/// </param>
	/// <param name="b">
	/// Second float to compare
	/// </param>
	/// <param name="epsilon">
	/// The epsilon to use for comparison
	/// </param>
	/// <returns></returns>
	public static bool IsEqualTo(this float a, float b, float epsilon = 0.0001f)
	{
		return Math.Abs(a - b) < epsilon;
	}

	/// <summary>
	/// Compares two nullable floats for equality with a given epsilon
	/// </summary>
	/// <param name="a">
	/// First float to compare
	/// </param>
	/// <param name="b">
	/// Second float to compare
	/// </param>
	/// <param name="epsilon">
	/// The epsilon to use for comparison
	/// </param>
	/// <returns></returns>
	public static bool IsEqualTo(this float? a, float? b, float epsilon = 0.0001f)
	{
		if (a.HasValue && b.HasValue)
			return Math.Abs(a.Value - b.Value) < epsilon;


		return !a.HasValue && !b.HasValue;
	}
}
