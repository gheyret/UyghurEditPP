/*
 * Finds the automatic corrections for a whole text ("Aptomatik Tekshür").
 */
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UyghurEditPP
{
	/// <summary>
	/// One correction: replace Length characters at Offset with Text.
	/// </summary>
	public class Tuzitish
	{
		public int Offset;
		public int Length;
		public string Text;
	}

	public static class AutoCorrect
	{
		/// <summary>
		/// Checks every word of text and returns, in text order, the corrections for the
		/// misspelled words that have a known correction. Offsets refer to the unchanged text.
		/// </summary>
		public static List<Tuzitish> Find(string text, Regex wordFinder, UyghurSpell spell, bool latin, out int sani, out int xatasani)
		{
			List<Tuzitish> ret = new List<Tuzitish>();
			sani = 0;
			xatasani = 0;
			foreach(Match soz in wordFinder.Matches(text)){
				sani++;
				if(spell.IsListed(soz.Value)){
					continue;
				}
				xatasani++;
				string toghrisi = Correction(soz.Value, spell, latin);
				if(toghrisi!=null){
					ret.Add(new Tuzitish{Offset = soz.Index, Length = soz.Length, Text = toghrisi});
				}
			}
			return ret;
		}

		static string Correction(string soz, UyghurSpell spell, bool latin)
		{
			string toghrisi = spell.Toghrisi(soz);
			if(toghrisi==null && latin){
				// Common ULY typing mistakes: o/u/e written for ö/ü/é.
				string[] namzatlar = {
					soz.Replace('o','ö').Replace('u','ü').Replace('e','é'),
					soz.Replace('o','ö').Replace('u','ü'),
					soz.Replace('o','ö'),
					soz.Replace('u','ü'),
					soz.Replace('e','é')
				};
				foreach(string namzat in namzatlar){
					if(spell.IsListed(namzat)){
						toghrisi = namzat;
						break;
					}
				}
			}
			if(string.IsNullOrEmpty(toghrisi)){
				return null;
			}
			if(char.IsUpper(soz[0])){
				toghrisi = char.ToUpperInvariant(toghrisi[0]) + toghrisi.Substring(1);
			}
			return toghrisi;
		}
	}
}
