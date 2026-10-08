using System.Globalization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UyghurEditPP.Tests
{
	// Uyghur Latin (ULY) uses I/i like English (see Uyghur.ULYHerpler). Under a Turkish
	// culture, culture-sensitive ToLower/ToUpper turn "I" into dotless "ı" and "i" into
	// "İ", neither of which is a ULY letter. The spelling and ULY code must not depend on
	// the Windows culture.
	[TestClass]
	public class CultureTests
	{
		CultureInfo gCulture;

		[TestInitialize]
		public void SetUp()
		{
			gCulture = Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");
		}

		[TestCleanup]
		public void TearDown()
		{
			Thread.CurrentThread.CurrentCulture = gCulture;
		}

		[TestMethod]
		public void KenjiSpell_CapitalI_IsFoundUnderTurkishCulture()
		{
			KenjiSpell spell = new KenjiSpell();
			spell.Add("ishchi");

			Assert.IsTrue(spell.IsListed("Ishchi"));
			Assert.IsTrue(spell.IsListed("ISHCHI"));
		}

		[TestMethod]
		public void KenjiSpell_WordAddedWithCapitalI_IsStoredWithDottedI()
		{
			KenjiSpell spell = new KenjiSpell();
			spell.Add("Ishchi");

			Assert.IsTrue(spell.IsListed("ishchi"));
		}

		[TestMethod]
		public void KenjiSpell_LookupSuggestsUnderTurkishCulture()
		{
			KenjiSpell spell = new KenjiSpell();
			spell.Add("ishchi");

			CollectionAssert.Contains(spell.Lookup("Ishchy"), "ishchi");
		}

		// The Case menu (owner's decision 2026-10-08: follow the ULY alphabet, not the culture).
		[TestMethod]
		public void CaseMenu_UsesUlyIUnderTurkishCulture()
		{
			Assert.AreEqual("ISHCHI IZ", Uyghur.ChongYaz("ishchi iz"));
			Assert.AreEqual("ishchi iz", Uyghur.KichikYaz("ISHCHI IZ"));
			Assert.AreEqual("Ishchi Iz", Uyghur.MawzuYaz("ishchi iz"));
		}

		// The find dialogs search case-insensitively with these options.
		[TestMethod]
		public void FindIgnoreCase_MatchesIWithiUnderTurkishCulture()
		{
			var options = System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.CultureInvariant;

			Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch("ISHCHI", "ishchi", options));
			Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch("ishchi", "ISHCHI", options));
		}

		[TestMethod]
		public void UlyJumleChongYaz_CapitalizesToI()
		{
			string result = Uyghur.ULYJumleChongYaz("ishchi keldi. ishchi ketti.");

			Assert.AreEqual("Ishchi keldi. Ishchi ketti.", result);
		}
	}
}
