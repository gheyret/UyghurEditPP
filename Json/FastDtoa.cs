using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// The following code is translated from dtoa_milo.h
// https://github.com/miloyip/dtoa-benchmark/blob/12ba1f5ec091e7a9c8e91eccd7aace7014dbda93/src/milo/dtoa_milo.h
/*
Copyright (C) 2014 Milo Yip

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
 */

// ReSharper disable InconsistentNaming
namespace UyghurEditPP
{
	internal static class FastDtoa
	{
		private struct DiyFp
		{
			public ulong f;
			public int e;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public DiyFp(ulong f, int e)
			{
				this.f = f;
				this.e = e;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public DiyFp(double d)
			{
				var u64 = (ulong)BitConverter.DoubleToInt64Bits(d);
				var biased_e = (int)((u64 & kDpExponentMask) >> kDpSignificandSize);
				var significand = u64 & kDpSignificandMask;
				if (biased_e != 0)
				{
					f = significand + kDpHiddenBit;
					e = biased_e - kDpExponentBias;
				}
				else
				{
					f = significand;
					e = kDpMinExponent + 1;
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static DiyFp operator -(DiyFp lhs, DiyFp rhs)
			{
				Debug.Assert(lhs.e == rhs.e);
				Debug.Assert(lhs.f >= rhs.f);
				return new DiyFp(lhs.f - rhs.f, lhs.e);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static DiyFp operator *(DiyFp lhs, DiyFp rhs)
			{
				var a = lhs.f >> 32;
				var b = (uint)lhs.f;
				var c = rhs.f >> 32;
				var d = (uint)rhs.f;
				var ac = a * c;
				var bc = b * c;
				var ad = a * d;
				var bd = (ulong)b * d;
				var tmp = (bd >> 32) + (uint)ad + (uint)bc;
				tmp += 1ul << 31;
				return new DiyFp(ac + (ad >> 32) + (bc >> 32) + (tmp >> 32), lhs.e + rhs.e + 64);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public DiyFp Normalize()
			{
				var res = this;
				while ((res.f & kDpHiddenBit) == 0)
				{
					res.f <<= 1;
					res.e--;
				}
				const int delta = kDiySignificandSize - kDpSignificandSize - 1;
				res.f <<= delta;
				res.e -= delta;
				return res;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private DiyFp NormalizeBoundary()
			{
				var res = this;
				while ((res.f & (kDpHiddenBit << 1)) == 0)
				{
					res.f <<= 1;
					res.e--;
				}
				const int delta = kDiySignificandSize - kDpSignificandSize - 2;
				res.f <<= delta;
				res.e -= delta;
				return res;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void NormalizedBoundaries(out DiyFp minus, out DiyFp plus)
			{
				var pl = new DiyFp((f << 1) + 1, e - 1).NormalizeBoundary();
				var mi = f == kDpHiddenBit ? new DiyFp((f << 2) - 1, e - 2) : new DiyFp((f << 1) - 1, e - 1);
				mi.f <<= mi.e - pl.e;
				mi.e = pl.e;
				plus = pl;
				minus = mi;
			}

			private const int kDiySignificandSize = 64;
			private const int kDpSignificandSize = 52;
			private const int kDpExponentBias = 0x3FF + kDpSignificandSize;
			private const int kDpMinExponent = -kDpExponentBias;
			private const ulong kDpExponentMask = 0x7FF0000000000000;
			private const ulong kDpSignificandMask = 0x000FFFFFFFFFFFFF;
			private const ulong kDpHiddenBit = 0x0010000000000000;
		}

		// 10^-348, 10^-340, ..., 10^340
		private static readonly ulong[] kCachedPowers_F =
		{
			0xfa8fd5a0081c0288, 0xbaaee17fa23ebf76,
			0x8b16fb203055ac76, 0xcf42894a5dce35ea,
			0x9a6bb0aa55653b2d, 0xe61acf033d1a45df,
			0xab70fe17c79ac6ca, 0xff77b1fcbebcdc4f,
			0xbe5691ef416bd60c, 0x8dd01fad907ffc3c,
			0xd3515c2831559a83, 0x9d71ac8fada6c9b5,
			0xea9c227723ee8bcb, 0xaecc49914078536d,
			0x823c12795db6ce57, 0xc21094364dfb5637,
			0x9096ea6f3848984f, 0xd77485cb25823ac7,
			0xa086cfcd97bf97f4, 0xef340a98172aace5,
			0xb23867fb2a35b28e, 0x84c8d4dfd2c63f3b,
			0xc5dd44271ad3cdba, 0x936b9fcebb25c996,
			0xdbac6c247d62a584, 0xa3ab66580d5fdaf6,
			0xf3e2f893dec3f126, 0xb5b5ada8aaff80b8,
			0x87625f056c7c4a8b, 0xc9bcff6034c13053,
			0x964e858c91ba2655, 0xdff9772470297ebd,
			0xa6dfbd9fb8e5b88f, 0xf8a95fcf88747d94,
			0xb94470938fa89bcf, 0x8a08f0f8bf0f156b,
			0xcdb02555653131b6, 0x993fe2c6d07b7fac,
			0xe45c10c42a2b3b06, 0xaa242499697392d3,
			0xfd87b5f28300ca0e, 0xbce5086492111aeb,
			0x8cbccc096f5088cc, 0xd1b71758e219652c,
			0x9c40000000000000, 0xe8d4a51000000000,
			0xad78ebc5ac620000, 0x813f3978f8940984,
			0xc097ce7bc90715b3, 0x8f7e32ce7bea5c70,
			0xd5d238a4abe98068, 0x9f4f2726179a2245,
			0xed63a231d4c4fb27, 0xb0de65388cc8ada8,
			0x83c7088e1aab65db, 0xc45d1df942711d9a,
			0x924d692ca61be758, 0xda01ee641a708dea,
			0xa26da3999aef774a, 0xf209787bb47d6b85,
			0xb454e4a179dd1877, 0x865b86925b9bc5c2,
			0xc83553c5c8965d3d, 0x952ab45cfa97a0b3,
			0xde469fbd99a05fe3, 0xa59bc234db398c25,
			0xf6c69a72a3989f5c, 0xb7dcbf5354e9bece,
			0x88fcf317f22241e2, 0xcc20ce9bd35c78a5,
			0x98165af37b2153df, 0xe2a0b5dc971f303a,
			0xa8d9d1535ce3b396, 0xfb9b7cd9a4a7443c,
			0xbb764c4ca7a44410, 0x8bab8eefb6409c1a,
			0xd01fef10a657842c, 0x9b10a4e5e9913129,
			0xe7109bfba19c0c9d, 0xac2820d9623bf429,
			0x80444b5e7aa7cf85, 0xbf21e44003acdd2d,
			0x8e679c2f5e44ff8f, 0xd433179d9c8cb841,
			0x9e19db92b4e31ba9, 0xeb96bf6ebadf77d9,
			0xaf87023b9bf0ee6b
		};

		// @formatter:off
		private static readonly int[] kCachedPowers_E =
		{
			-1220, -1193, -1166, -1140, -1113, -1087, -1060, -1034, -1007,  -980,
			-954,  -927,  -901,  -874,  -847,  -821,  -794,  -768,  -741,  -715,
			-688,  -661,  -635,  -608,  -582,  -555,  -529,  -502,  -475,  -449,
			-422,  -396,  -369,  -343,  -316,  -289,  -263,  -236,  -210,  -183,
			-157,  -130,  -103,   -77,   -50,   -24,     3,    30,    56,    83,
			109,   136,   162,   189,   216,   242,   269,   295,   322,   348,
			375,   402,   428,   455,   481,   508,   534,   561,   588,   614,
			641,   667,   694,   720,   747,   774,   800,   827,   853,   880,
			907,   933,   960,   986,   1013, 1039,  1066
		};
		// @formatter:on

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static DiyFp GetCachedPower(int e, out int K)
		{
			//int k = static_cast<int>(ceil((-61 - e) * 0.30102999566398114)) + 374;
			var dk = (-61 - e) * 0.30102999566398114 + 347; // dk must be positive, so can do ceiling in positive
			var k = (int)dk;
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if (k != dk)
				k++;

			var index = (uint)((k >> 3) + 1);
			K = -(-348 + (int)(index << 3)); // decimal exponent no need lookup table

			Debug.Assert(index < kCachedPowers_F.Length);
			return new DiyFp(kCachedPowers_F[index], kCachedPowers_E[index]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void GrisuRound(char* buffer, ulong delta, ulong rest, ulong ten_kappa, ulong wp_w)
		{
			buffer--;
			while (rest < wp_w && delta - rest >= ten_kappa &&
			       (rest + ten_kappa < wp_w || // closer
			        wp_w - rest >= rest + ten_kappa - wp_w))
			{
				(*buffer)--;
				rest += ten_kappa;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int log10(uint v)
		{
			return v >= 1000000000u ? 9 :
				v >= 100000000u ? 8 :
				v >= 10000000u ? 7 :
				v >= 1000000u ? 6 :
				v >= 100000u ? 5 :
				v >= 10000u ? 4 :
				v >= 1000u ? 3 :
				v >= 100u ? 2 :
				v >= 10u ? 1 : 0;
		}

		private static readonly uint[] kPow10 =
		{1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000};

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe int DigitGen(DiyFp W, DiyFp Mp, ulong delta, char* buffer, ref int K)
		{
			var one = new DiyFp((ulong)1 << -Mp.e, Mp.e);
			var wp_w = Mp - W;
			var p1 = (uint)(Mp.f >> -one.e);
			var p2 = Mp.f & (one.f - 1);
			var kappa = log10(p1) + 1;
			var length = 0;

			var divisor = kPow10[kappa - 1];
			while (kappa > 0)
			{
				var d = p1 / divisor;
				p1 %= divisor;
				if (d != 0 || length != 0)
				{
					*buffer++ = (char)('0' + d);
					length++;
				}
				kappa--;
				var tmp = ((ulong)p1 << -one.e) + p2;
				if (tmp <= delta)
				{
					K += kappa;
					GrisuRound(buffer, delta, tmp, (ulong)divisor << -one.e, wp_w.f);
					return length;
				}
				divisor /= 10;
			}

			var pow10 = 10ul;
			// kappa = 0
			for (;;)
			{
				p2 *= 10;
				delta *= 10;
				var d1 = (char)(p2 >> -one.e);
				if (d1 != 0 || length != 0)
				{
					*buffer++ = (char)('0' + d1);
					length++;
				}
				p2 &= one.f - 1;
				kappa--;
				if (p2 < delta)
				{
					K += kappa;
					GrisuRound(buffer, delta, p2, one.f, wp_w.f * pow10);
					return length;
				}
				pow10 *= 10;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe int Grisu2(double value, char* buffer, out int K)
		{
			DiyFp diyFp = new DiyFp(value);
			DiyFp minus;
			DiyFp plus;
			diyFp.NormalizedBoundaries(out minus, out plus);
			DiyFp cachedPower = GetCachedPower(plus.e, out K);
			DiyFp w = diyFp.Normalize() * cachedPower;
			DiyFp mp = plus * cachedPower;
			DiyFp diyFp2 = minus * cachedPower;
			diyFp2.f++;
			mp.f--;
			return DigitGen(w, mp, mp.f - diyFp2.f, buffer, ref K);		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe int WriteExponent(int K, char* buffer)
		{
			var length = 0;
			if (K < 0)
			{
				*buffer++ = '-';
				length++;
				K = -K;
			}
			if (K >= 100)
			{
				*buffer++ = (char)('0' + K / 100);
				K %= 100;
				*buffer++ = (char)('0' + K / 10);
				K %= 10;
				*buffer = (char)('0' + K);
				return length + 3;
			}
			if (K >= 10)
			{
				*buffer++ = (char)('0' + K / 10);
				K %= 10;
				*buffer = (char)('0' + K);
				return length + 2;
			}
			*buffer = (char)('0' + K);
			return length + 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe int Prettify(char* buffer, int length, int k)
		{
			var kk = length + k; // 10^(kk-1) <= v < 10^kk

			if (length <= kk && kk <= 21)
			{
				// 1234e7 -> 12340000000
				for (var i = length; i < kk; i++)
					buffer[i] = '0';
				return kk;
			}
			if (0 < kk && kk <= 21)
			{
				// 1234e-2 -> 12.34
				MemMove(buffer + kk + 1, buffer + kk, length - kk);
				buffer[kk] = '.';
				return length + 1;
			}
			if (-6 < kk && kk <= 0)
			{
				// 1234e-6 -> 0.001234
				var offset = 2 - kk;
				MemMove(buffer + offset, buffer, length);
				*buffer++ = '0';
				*buffer++ = '.';
				for (var i = 2; i < offset; i++)
					*buffer++ = '0';
				return length + offset;
			}
			if (length == 1)
			{
				// 1e30
				buffer[1] = 'e';
				return 2 + WriteExponent(kk - 1, buffer + 2);
			}
			// 1234e30 -> 1.234e33
			MemMove(buffer + 2, buffer + 1, length - 1);
			buffer[1] = '.';
			buffer[length + 1] = 'e';
			return length + 2 + WriteExponent(kk - 1, buffer + length + 2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void MemMove(char* dst, char* src, int length)
		{
			dst += length - 1;
			src += length - 1;
			for (var i = 0; i < length; i++)
				*dst-- = *src--;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe int dtoa_milo(double value, char* buffer)
		{
			if (value == 0.0)
			{
				*buffer = '0';
				return 1;
			}
			int num = 0;
			if (value < 0.0)
			{
				char* intPtr = buffer;
				buffer = intPtr + 1;
				*intPtr = '-';
				value = 0.0 - value;
				num = 1;
			}
			int K;
			int length = Grisu2(value, buffer, out K);
			length = Prettify(buffer, length, K);
			return length + num;
		}

		internal static unsafe int Convert(double d, char* buffer)
		{
			return dtoa_milo(d, buffer);
		}
	}
}