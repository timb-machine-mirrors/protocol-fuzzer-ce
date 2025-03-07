﻿//
// Copyright (c) Peach Fuzzer, LLC
//

using System.ComponentModel;
using Peach.Core;
using Peach.Core.Dom;

namespace Peach.Pro.Core.Mutators
{
	/// <summary>
	/// Format characters are characters that do not have a visible appearance,
	/// but may have an effect on the appearance or behavior of neighboring characters.
	/// For example, U+200C ZERO WIDTH NON-JOINER and U+200D ZERO WIDTH JOINER may
	/// be used to change the default shaping behavior of adjacent characters
	/// (e.g. to inhibit ligatures or request ligature formation).
	/// There are 152 format characters in Unicode 7.0.
	/// </summary>
	[Mutator("StringUnicodeFormatCharacters")]
	[Description("Produce string comprised of unicode format characters.")]
	public class StringUnicodeFormatCharacters : Utility.StringMutator
	{
		// http://www.unicode.org/Public/UNIDATA/UnicodeData.txt
		// All characters of class Cf, Zl, Zp

		static readonly int[] codePoints = new int[]
		{
			0x000AD, 0x00600, 0x00601, 0x00602, 0x00603, 0x00604, 0x00605, 0x0061C,
			0x006DD, 0x0070F, 0x0180E, 0x0200B, 0x0200C, 0x0200D, 0x0200E, 0x0200F,
			0x02028, 0x02029, 0x0202A, 0x0202B, 0x0202C, 0x0202D, 0x0202E, 0x02060,
			0x02061, 0x02062, 0x02063, 0x02064, 0x02066, 0x02067, 0x02068, 0x02069,
			0x0206A, 0x0206B, 0x0206C, 0x0206D, 0x0206E, 0x0206F, 0x0FEFF, 0x0FFF9,
			0x0FFFA, 0x0FFFB, 0x110BD, 0x1BCA0, 0x1BCA1, 0x1BCA2, 0x1BCA3, 0x1D173,
			0x1D174, 0x1D175, 0x1D176, 0x1D177, 0x1D178, 0x1D179, 0x1D17A, 0xE0001,
			0xE0020, 0xE0021, 0xE0022, 0xE0023, 0xE0024, 0xE0025, 0xE0026, 0xE0027,
			0xE0028, 0xE0029, 0xE002A, 0xE002B, 0xE002C, 0xE002D, 0xE002E, 0xE002F,
			0xE0030, 0xE0031, 0xE0032, 0xE0033, 0xE0034, 0xE0035, 0xE0036, 0xE0037,
			0xE0038, 0xE0039, 0xE003A, 0xE003B, 0xE003C, 0xE003D, 0xE003E, 0xE003F,
			0xE0040, 0xE0041, 0xE0042, 0xE0043, 0xE0044, 0xE0045, 0xE0046, 0xE0047,
			0xE0048, 0xE0049, 0xE004A, 0xE004B, 0xE004C, 0xE004D, 0xE004E, 0xE004F,
			0xE0050, 0xE0051, 0xE0052, 0xE0053, 0xE0054, 0xE0055, 0xE0056, 0xE0057,
			0xE0058, 0xE0059, 0xE005A, 0xE005B, 0xE005C, 0xE005D, 0xE005E, 0xE005F,
			0xE0060, 0xE0061, 0xE0062, 0xE0063, 0xE0064, 0xE0065, 0xE0066, 0xE0067,
			0xE0068, 0xE0069, 0xE006A, 0xE006B, 0xE006C, 0xE006D, 0xE006E, 0xE006F,
			0xE0070, 0xE0071, 0xE0072, 0xE0073, 0xE0074, 0xE0075, 0xE0076, 0xE0077,
			0xE0078, 0xE0079, 0xE007A, 0xE007B, 0xE007C, 0xE007D, 0xE007E, 0xE007F,
		};

		static StringUnicodeFormatCharacters()
		{
			System.Diagnostics.Debug.Assert(codePoints.Length == 152);
		}

		public StringUnicodeFormatCharacters(DataElement obj)
			: base(obj)
		{
		}

		protected override int GetCodePoint()
		{
			var idx = context.Random.Next(codePoints.Length);
			return codePoints[idx];
		}
	}
}
