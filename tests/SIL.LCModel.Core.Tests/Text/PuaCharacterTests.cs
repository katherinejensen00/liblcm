// Copyright (c) 2003-2017 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using System;
using NUnit.Framework;
using SIL.LCModel.Utils;

namespace SIL.LCModel.Core.Text
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// These test test the PuaCharacterDlg dialog and the PuaCharacter tab on the
	/// WritingSystemPropertiesDialog.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class PuaCharacterTests
		// can't derive from BaseTest, but instantiate DebugProcs instead
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the compareHex method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PuaCharacterCompareHex()
		{
			AssertComparisonWorks("E","E",0);
			AssertComparisonWorks("100A","1009",1);
			AssertComparisonWorks("1001","10001",-1);
			AssertComparisonWorks("01","1",0);
			AssertComparisonWorks("0001","1",0);
			AssertComparisonWorks("000E","E",0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Asserts the comparison works.
		/// </summary>
		/// <param name="hex1">The hex1.</param>
		/// <param name="hex2">The hex2.</param>
		/// <param name="expectedComparison">The expected comparison.</param>
		/// ------------------------------------------------------------------------------------
		private void AssertComparisonWorks(string hex1, string hex2, int expectedComparison)
		{
			int comparison;
			comparison = MiscUtils.CompareHex(hex1, hex2);
			string NL = Environment.NewLine;
			Assert.AreEqual(expectedComparison, comparison, "CompareHex did not compare correctly:" + NL +
				"values: " + hex1 + " ? " + hex2);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the UnicodeData.txt style "ToString" method of PUACharacter.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PuaCharacterToString()
		{
			string unicodeData =
				"VULGAR FRACTION ONE THIRD;No;0;ON;<fraction> 0031 2044 0033;;;1/3;N;FRACTION ONE THIRD;;;;";
			PUACharacter puaChar = new PUACharacter("2153",unicodeData);
			Assert.AreEqual("2153;" + unicodeData, puaChar.ToString(), "Error while writing PUACharacter");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the pua character text constructor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PuaCharacterTextConstructor()
		{
			new PUACharacter("0669", "ARABIC-INDIC DIGIT NINE;Nd;0;AN;;9;9;9;N;;;;;");
		}

		[TestCase("1", Result = "\x0001")]
		[TestCase("12", Result = "\x0012")]
		[TestCase("123", Result = "\x0123")]
		[TestCase("1234", Result = "\x1234")]
		[TestCase("12345", Result = "\xd808\xdf45")]
		[TestCase("10FFFF", Result = "\xdbff\xdfff")]
		[TestCase("123456", Result = " ")]
		[TestCase("110000", Result = " ")]
		[TestCase("D800", Result = " ")]
		[TestCase("D801", Result = " ")]
		[TestCase("DFFF", Result = " ")]
		[TestCase("", Result = " ")]
		[TestCase(null, Result = " ")]
		public string CodepointAsString(string codepoint)
		{
			return PUACharacter.CodepointAsString(codepoint);
		}

	}
}
