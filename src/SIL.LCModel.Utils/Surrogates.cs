// --------------------------------------------------------------------------------------------
// Copyright (c) 2011-2015 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)
//
// File: Surrogates.cs
// --------------------------------------------------------------------------------------------

namespace SIL.LCModel.Utils
{
	/// <summary>
	/// A home for functions related to Unicode surrogate pairs.
	/// (Some of these unfortunately are duplicated in FwUtils.)
	/// </summary>
	public static class Surrogates
	{
		public const char kMinLeadSurrogate = '\xD800';
		public const char kMaxLeadSurrogate = '\xDBFF';

		/// <summary>
		/// Returns a string representation of the codepoint, handling surrogate pairs if necessary.
		/// </summary>
		/// <param name="codepoint">The codepoint</param>
		/// <returns>The string representation of the codepoint</returns>
		public static string StringFromCodePoint(int codepoint)
		{
			return char.ConvertFromUtf32(codepoint);
		}

		/// <summary>
		/// Return a full 32-bit character value from the surrogate pair.
		/// </summary>
		public static int Int32FromSurrogates(char ch1, char ch2)
		{
			return char.ConvertToUtf32(ch1, ch2);
		}
		/// <summary>
		/// Whether the character is the first of a surrogate pair.
		///  This was copied from SIL.FieldWorks.IText
		/// </summary>
		/// <param name="ch">The character</param>
		/// <returns></returns>
		public static bool IsLeadSurrogate(char ch)
		{
			// could also use char.IsHighSurrogate(ch)
			return ch >= kMinLeadSurrogate && ch <= kMaxLeadSurrogate;
		}
		/// <summary>
		/// Whether the character is the second of a surrogate pair.
		///  This was copied from SIL.FieldWorks.IText
		/// </summary>
		/// <param name="ch">The character</param>
		/// <returns></returns>
		public static bool IsTrailSurrogate(char ch)
		{
			// could also use char.IsLowSurrogate(ch)
			const char minTrailSurrogate = '\xDC00';
			const char maxTrailSurrogate = '\xDFFF';
			return ch >= minTrailSurrogate && ch <= maxTrailSurrogate;
		}
		/// <summary>
		/// Increment an index into a string, allowing for surrogates.
		/// This was copied from SIL.FieldWorks.IText
		/// </summary>
		/// <param name="st"></param>
		/// <param name="ich"></param>
		/// <returns></returns>
		public static int NextChar(string st, int ich)
		{
			if (IsLeadSurrogate(st[ich]) && ich < st.Length - 1 && IsTrailSurrogate(st[ich+1]))
				return ich + 2;
			return ich + 1;
		}

		/// <summary>
		/// Decrement an index into a string, allowing for surrogates.
		/// Assumes ich is pointing at the START of a character; will move back two if the two characters
		/// at ich-1 and ich-2 are a pair.
		/// </summary>
		public static int PrevChar(string st, int ich)
		{
			if (ich >= 2 && IsLeadSurrogate(st[ich - 2]) && IsTrailSurrogate(st[ich - 1]))
				return ich - 2;
			return ich - 1;
		}
	}
}
