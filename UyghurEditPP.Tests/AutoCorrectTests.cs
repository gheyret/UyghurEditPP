using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UyghurEditPP.Tests
{
	[TestClass]
	public class AutoCorrectTests
	{
		static readonly Regex gLatin = new Regex(string.Format("[{0}]+([-]?[{1}]+)*", Uyghur.ULYHerpler, Uyghur.ULYHerpler));
		static readonly Regex gUyghur = new Regex(string.Format("[{0}]+([-]?[{1}]+)*", Uyghur.UEYHerpler, Uyghur.UEYHerpler));

		static string Apply(string text, List<Tuzitish> list)
		{
			StringBuilder sb = new StringBuilder(text);
			for(int i = list.Count-1; i>=0; i--){
				sb.Remove(list[i].Offset, list[i].Length).Insert(list[i].Offset, list[i].Text);
			}
			return sb.ToString();
		}

		[TestMethod]
		public void Find_LatinVowels_AreCorrectedAndCapitalsKept()
		{
			KenjiSpell spell = new KenjiSpell();
			spell.Add("köz");
			spell.Add("bilen");
			spell.Add("kördüm");
			int sani, xatasani;
			string text = "Koz bilen kordum. Koz bilen kordum.";

			List<Tuzitish> list = AutoCorrect.Find(text, gLatin, spell, true, out sani, out xatasani);

			Assert.AreEqual(6, sani);
			Assert.AreEqual(4, xatasani);
			// Every capital is kept, also after the first correction.
			Assert.AreEqual("Köz bilen kördüm. Köz bilen kördüm.", Apply(text, list));
		}

		[TestMethod]
		public void Find_UnknownWords_AreCountedButNotChanged()
		{
			KenjiSpell spell = new KenjiSpell();
			spell.Add("bilen");
			int sani, xatasani;

			List<Tuzitish> list = AutoCorrect.Find("xyz bilen", gLatin, spell, true, out sani, out xatasani);

			Assert.AreEqual(2, sani);
			Assert.AreEqual(1, xatasani);
			Assert.AreEqual(0, list.Count);
		}

		[TestMethod]
		public void Find_UsesCorrectionList()
		{
			string folder = Path.Combine(Path.GetTempPath(), "UyghurEditPP.Tests", Guid.NewGuid().ToString("N"));
			Directory.CreateDirectory(folder);
			try{
				string shipped = Path.Combine(folder, "xatatoghra.txt");
				File.WriteAllText(shipped, "كانداق=قانداق\r\n", Encoding.UTF8);
				KenjiSpell spell = new KenjiSpell();
				spell.SetFileNames(Path.Combine(folder, "i.txt"), Path.Combine(folder, "u.txt"), shipped);
				using (MemoryStream dic = new MemoryStream(Encoding.UTF8.GetBytes("بىلەن 10\n"))) {
					spell.Load(dic, Uyghur.YEZIQ.UEY);
				}
				int sani, xatasani;
				string text = "كانداق بىلەن كانداق";

				List<Tuzitish> list = AutoCorrect.Find(text, gUyghur, spell, false, out sani, out xatasani);

				Assert.AreEqual(2, list.Count);
				Assert.AreEqual("قانداق بىلەن قانداق", Apply(text, list));
			}
			finally{
				Directory.Delete(folder, true);
			}
		}
	}
}
