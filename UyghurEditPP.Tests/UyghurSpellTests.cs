using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UyghurEditPP.Tests
{
	[TestClass]
	public class UyghurSpellTests
	{
		string gFolder;
		string gShipped;
		string gUser;
		string gIshletkuchi;

		[TestInitialize]
		public void SetUp()
		{
			gFolder = Path.Combine(Path.GetTempPath(), "UyghurEditPP.Tests", Guid.NewGuid().ToString("N"));
			Directory.CreateDirectory(gFolder);
			gShipped = Path.Combine(gFolder, "program_imla_xatatoghra.txt");
			gUser = Path.Combine(gFolder, "appdata_imla_xatatoghra.txt");
			gIshletkuchi = Path.Combine(gFolder, "appdata_imla_ishletkuchi.txt");
		}

		[TestCleanup]
		public void TearDown()
		{
			if(Directory.Exists(gFolder)){
				Directory.Delete(gFolder, true);
			}
		}

		KenjiSpell Load()
		{
			KenjiSpell spell = new KenjiSpell();
			spell.SetFileNames(gIshletkuchi, gUser, gShipped);
			using (MemoryStream dic = new MemoryStream(Encoding.UTF8.GetBytes("كىتاب 10\n"))) {
				spell.Load(dic, Uyghur.YEZIQ.UEY);
			}
			return spell;
		}

		[TestMethod]
		public void Load_ShippedListOnly_IsUsed()
		{
			File.WriteAllText(gShipped, "كانداق=قانداق\r\nيۇراك=يۈرەك\r\n", Encoding.UTF8);

			KenjiSpell spell = Load();

			Assert.AreEqual("قانداق", spell.Toghrisi("كانداق"));
			Assert.AreEqual("يۈرەك", spell.Toghrisi("يۇراك"));
		}

		[TestMethod]
		public void Load_UserCorrections_AreAddedAndWin()
		{
			File.WriteAllText(gShipped, "كانداق=قانداق\r\nيۇراك=يۈرەك\r\n", Encoding.UTF8);
			File.WriteAllText(gUser, "يۇراك=يۈرەكلەر\r\nئالما=ئالمىلار\r\n", Encoding.UTF8);

			KenjiSpell spell = Load();

			Assert.AreEqual("قانداق", spell.Toghrisi("كانداق"));
			Assert.AreEqual("يۈرەكلەر", spell.Toghrisi("يۇراك"));
			Assert.AreEqual("ئالمىلار", spell.Toghrisi("ئالما"));
		}

		[TestMethod]
		public void SaveToXataToghra_WritesToUserFileOnly()
		{
			File.WriteAllText(gShipped, "كانداق=قانداق\r\n", Encoding.UTF8);
			KenjiSpell spell = Load();

			spell.SaveToXataToghra("ئالما", "ئالمىلار");

			Assert.AreEqual("كانداق=قانداق\r\n", File.ReadAllText(gShipped, Encoding.UTF8));
			StringAssert.Contains(File.ReadAllText(gUser, Encoding.UTF8), "ئالما=ئالمىلار");
			Assert.AreEqual("ئالمىلار", Load().Toghrisi("ئالما"));
		}
	}
}
