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

		[TestMethod]
		public void UlyJumleChongYaz_CapitalizesToI()
		{
			string result = Uyghur.ULYJumleChongYaz("ishchi keldi. ishchi ketti.");

			Assert.AreEqual("Ishchi keldi. Ishchi ketti.", result);
		}
	}
}
