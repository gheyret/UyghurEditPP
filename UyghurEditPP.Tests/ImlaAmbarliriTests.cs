using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UyghurEditPP.Tests
{
	[TestClass]
	public class ImlaAmbarliriTests
	{
		int gAchildi;
		string gFolder;

		[TestInitialize]
		public void SetUp()
		{
			gAchildi = 0;
			gFolder = Path.Combine(Path.GetTempPath(), "UyghurEditPP.Tests", Guid.NewGuid().ToString("N"));
			Directory.CreateDirectory(gFolder);
		}

		[TestCleanup]
		public void TearDown()
		{
			Directory.Delete(gFolder, true);
		}

		ImlaAmbarliri Create()
		{
			return new ImlaAmbarliri(
				() => { Interlocked.Increment(ref gAchildi); return new MemoryStream(Encoding.UTF8.GetBytes("كىتاب 10\nئۆي 5\n")); },
				() => {
					KenjiSpell spell = new KenjiSpell();
					// No user files: keep the test away from %AppData%.
					spell.SetFileNames(Path.Combine(gFolder, "i.txt"), Path.Combine(gFolder, "u.txt"), Path.Combine(gFolder, "s.txt"));
					return spell;
				});
		}

		[TestMethod]
		public void Get_SameScriptTwice_LoadsOnce()
		{
			ImlaAmbarliri ambarlar = Create();

			Task<UyghurSpell> a = ambarlar.Get(Uyghur.YEZIQ.UEY);
			Task<UyghurSpell> b = ambarlar.Get(Uyghur.YEZIQ.UEY);

			Assert.AreSame(a, b);
			Assert.IsTrue(a.Result.IsListed("كىتاب"));
			Assert.AreEqual(1, gAchildi);
		}

		[TestMethod]
		public void Get_EachScript_HasItsOwnConvertedDictionary()
		{
			ImlaAmbarliri ambarlar = Create();

			UyghurSpell uey = ambarlar.Get(Uyghur.YEZIQ.UEY).Result;
			UyghurSpell uly = ambarlar.Get(Uyghur.YEZIQ.ULY).Result;
			UyghurSpell usy = ambarlar.Get(Uyghur.YEZIQ.USY).Result;

			Assert.AreEqual(3, gAchildi);
			Assert.IsTrue(uey.IsListed("كىتاب"));
			Assert.IsTrue(uly.IsListed(Uyghur.UEY2ULY("كىتاب")));
			Assert.IsTrue(usy.IsListed(Uyghur.UEY2USY("كىتاب")));
			Assert.AreEqual(3, ambarlar.Loaded.Count);
		}

		[TestMethod]
		public void Get_RunsInTheBackground()
		{
			ManualResetEventSlim release = new ManualResetEventSlim(false);
			ImlaAmbarliri ambarlar = new ImlaAmbarliri(
				() => { release.Wait(); return new MemoryStream(Encoding.UTF8.GetBytes("ئۆي 5\n")); },
				() => { KenjiSpell s = new KenjiSpell(); s.SetFileNames(Path.Combine(gFolder, "i.txt"), Path.Combine(gFolder, "u.txt"), Path.Combine(gFolder, "s.txt")); return s; });

			Task<UyghurSpell> task = ambarlar.Get(Uyghur.YEZIQ.UEY);

			Assert.IsFalse(task.IsCompleted);
			Assert.AreEqual(0, ambarlar.Loaded.Count);
			release.Set();
			Assert.IsTrue(task.Wait(10000));
			Assert.AreEqual(1, ambarlar.Loaded.Count);
		}

		[TestMethod]
		public void Get_AfterAFailedLoad_TriesAgain()
		{
			bool fail = true;
			ImlaAmbarliri ambarlar = new ImlaAmbarliri(
				() => { if(fail) throw new IOException("broken"); return new MemoryStream(Encoding.UTF8.GetBytes("ئۆي 5\n")); },
				() => { KenjiSpell s = new KenjiSpell(); s.SetFileNames(Path.Combine(gFolder, "i.txt"), Path.Combine(gFolder, "u.txt"), Path.Combine(gFolder, "s.txt")); return s; });

			Task<UyghurSpell> first = ambarlar.Get(Uyghur.YEZIQ.UEY);
			try { first.Wait(); } catch (AggregateException) { }
			fail = false;
			Task<UyghurSpell> second = ambarlar.Get(Uyghur.YEZIQ.UEY);

			Assert.IsTrue(first.IsFaulted);
			Assert.IsTrue(second.Result.IsListed("ئۆي"));
		}

		// A word the user marked as correct in one script is known in the others.
		[TestMethod]
		public void IshletkuchiSozQosh_IsConvertedToTheDictionaryScript()
		{
			ImlaAmbarliri ambarlar = Create();
			UyghurSpell uly = ambarlar.Get(Uyghur.YEZIQ.ULY).Result;

			uly.IshletkuchiSozQosh("ئالما");

			Assert.IsTrue(uly.IsListed(Uyghur.UEY2ULY("ئالما")));
		}
	}
}
