using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UyghurEditPP.Tests
{
	[TestClass]
	public class LanguageTests
	{
		[TestMethod]
		public void NewMessage_ShowsTheUyghurText()
		{
			Language lang = new Language();
			string key = "An unexpected error occurred.";

			lang.LanguaID = "uly";
			Assert.AreEqual("Kütülmigen xataliq körüldi.", lang.GetText(key));

			lang.LanguaID = "uey";
			Assert.AreEqual(Uyghur.ULY2UEY("Kütülmigen xataliq körüldi.").Replace("🠊", "🠈"), lang.GetText(key));

			lang.LanguaID = "eng";
			Assert.AreEqual(key, lang.GetText(key));
		}

		[TestMethod]
		public void ExistingMessage_IsUnchanged()
		{
			Language lang = new Language();
			lang.LanguaID = "uly";

			Assert.AreEqual("Höjjetning mezmunida özgirish boldi. Saqlamsiz?", lang.GetText("Höjjetning mezmunida özgirish boldi. Saqlamsiz?"));
		}
	}
}
