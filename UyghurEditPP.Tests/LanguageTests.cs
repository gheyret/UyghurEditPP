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

		const string Key4 = "OCR could not start because a Visual C++ runtime library is missing. Please install the Microsoft Visual C++ Redistributable (x64):";
		const string Key5 = "OCR could not start. Please check that this folder contains the language data (.traineddata) files:";

		// Latin product names and file types stay in Latin letters in UEY and USY.
		[TestMethod]
		public void LatinParts_StayLatinInEveryScript()
		{
			Language lang = new Language();
			const string LRM = "‎";

			lang.LanguaID = "uly";
			Assert.AreEqual("Visual C++ ambiri kem bolghachqa OCR qozghalmidi. Microsoft Visual C++ Redistributable (x64) ni ornitip béqing:", lang.GetText(Key4));
			Assert.AreEqual("OCR qozghalmidi. Töwendiki qisquchta til sanliq melumat (.traineddata) höjjetliri barmu, tekshürüp béqing:", lang.GetText(Key5));

			lang.LanguaID = "uey";
			Assert.AreEqual(
				LRM + "Visual C++" + LRM + Uyghur.ULY2UEY(" ambiri kem bolghachqa ") + LRM + "OCR" + LRM + Uyghur.ULY2UEY(" qozghalmidi. ")
				+ LRM + "Microsoft Visual C++ Redistributable (x64)" + LRM + Uyghur.ULY2UEY(" ni ornitip béqing:"),
				lang.GetText(Key4));
			Assert.AreEqual(
				LRM + "OCR" + LRM + Uyghur.ULY2UEY(" qozghalmidi. Töwendiki qisquchta til sanliq melumat (") + LRM + ".traineddata" + LRM + Uyghur.ULY2UEY(") höjjetliri barmu, tekshürüp béqing:"),
				lang.GetText(Key5));

			lang.LanguaID = "usy";
			Assert.AreEqual(
				"Visual C++" + Uyghur.ULY2USY(" ambiri kem bolghachqa ") + "OCR" + Uyghur.ULY2USY(" qozghalmidi. ")
				+ "Microsoft Visual C++ Redistributable (x64)" + Uyghur.ULY2USY(" ni ornitip béqing:"),
				lang.GetText(Key4));
			StringAssert.StartsWith(lang.GetText(Key5), "OCR");
			StringAssert.Contains(lang.GetText(Key5), "(.traineddata)");
		}

		// Texts without {...} are converted exactly as before.
		[TestMethod]
		public void TextWithoutBraces_IsConvertedAsBefore()
		{
			string uly = "Saqlanmighan özgirishler tashliwétilidu. Dawamlashturamsiz?";

			Assert.AreEqual(Uyghur.ULY2UEY(uly).Replace("🠊", "🠈"), Language.Yeziqla(uly, "uey"));
			Assert.AreEqual(Uyghur.ULY2USY(uly), Language.Yeziqla(uly, "usy"));
			Assert.AreEqual(uly, Language.Yeziqla(uly, "uly"));
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
