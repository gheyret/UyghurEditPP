using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UyghurEditPP.Tests
{
	[TestClass]
	public class KenjiSpellTests
	{
		static KenjiSpell CreateSpell(params string[] words)
		{
			KenjiSpell spell = new KenjiSpell();
			foreach(string word in words){
				spell.Add(word);
			}
			return spell;
		}

		// Words of one to three letters, including the endings ("si", "i") that
		// Lookup handles specially, in Latin, Arabic and Cyrillic script.
		[DataTestMethod]
		[DataRow("i")]
		[DataRow("si")]
		[DataRow("ki")]
		[DataRow("bi")]
		[DataRow("asi")]
		[DataRow("kki")]
		[DataRow("ى")]
		[DataRow("سى")]
		[DataRow("كى")]
		[DataRow("ئى")]
		[DataRow("и")]
		[DataRow("си")]
		[DataRow("ки")]
		[DataRow("a")]
		[DataRow("ab")]
		[DataRow("abc")]
		public void Lookup_ShortWords_DoesNotThrow(string word)
		{
			KenjiSpell spell = CreateSpell("kitab", "siz", "bu", "ki", "bilen", "كىتاب", "سىز", "китаб");

			List<string> result = spell.Lookup(word);

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Lookup_EmptyWord_ReturnsEmptyList()
		{
			KenjiSpell spell = CreateSpell("kitab");

			Assert.AreEqual(0, spell.Lookup("").Count);
		}

		[TestMethod]
		public void Lookup_EmptyDictionary_ReturnsEmptyList()
		{
			KenjiSpell spell = new KenjiSpell();

			Assert.AreEqual(0, spell.Lookup("si").Count);
		}

		[TestMethod]
		public void Lookup_MisspelledWord_SuggestsDictionaryWord()
		{
			KenjiSpell spell = CreateSpell("kitab", "bilen");

			CollectionAssert.Contains(spell.Lookup("kitap"), "kitab");
		}

		[TestMethod]
		public void IsListed_FindsAddedWordsOnly()
		{
			KenjiSpell spell = CreateSpell("kitab", "si");

			Assert.IsTrue(spell.IsListed("kitab"));
			Assert.IsTrue(spell.IsListed("si"));
			Assert.IsFalse(spell.IsListed("kita"));
		}
	}
}
