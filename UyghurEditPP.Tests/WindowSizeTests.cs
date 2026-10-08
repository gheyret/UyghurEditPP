using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UyghurEditPP.Tests
{
	[TestClass]
	public class WindowSizeTests
	{
		[TestMethod]
		public void Size_IsSavedInLogicalUnits()
		{
			Assert.AreEqual(new Size(1024, 768), WindowSize.ToLogical(new Size(1024, 768), 96));
			Assert.AreEqual(new Size(1024, 768), WindowSize.ToLogical(new Size(1536, 1152), 144));
			Assert.AreEqual(new Size(1024, 768), WindowSize.ToLogical(new Size(1280, 960), 120));
		}

		[TestMethod]
		public void Size_IsRestoredForTheMonitorsDpi()
		{
			Assert.AreEqual(new Size(1024, 768), WindowSize.ToDevice(new Size(1024, 768), 96));
			Assert.AreEqual(new Size(1536, 1152), WindowSize.ToDevice(new Size(1024, 768), 144));
			Assert.AreEqual(new Size(2048, 1536), WindowSize.ToDevice(new Size(1024, 768), 192));
		}

		// Moving between monitors and restarting must not make the window grow or shrink.
		[TestMethod]
		public void SaveAndRestore_KeepsTheSize()
		{
			Size logical = new Size(1333, 829);
			foreach(int dpi in new[] { 96, 120, 144, 168, 192 }){
				Size device = WindowSize.ToDevice(logical, dpi);
				Assert.AreEqual(logical, WindowSize.ToLogical(device, dpi), "dpi " + dpi);
			}
		}

		[TestMethod]
		public void UnknownDpi_LeavesTheSize()
		{
			Assert.AreEqual(new Size(800, 600), WindowSize.ToLogical(new Size(800, 600), 0));
			Assert.AreEqual(new Size(800, 600), WindowSize.ToDevice(new Size(800, 600), 0));
		}
	}
}
