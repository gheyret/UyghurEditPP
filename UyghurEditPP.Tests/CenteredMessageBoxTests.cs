using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UyghurEditPP.Tests
{
	[TestClass]
	public class CenteredMessageBoxTests
	{
		static readonly Rectangle gLeftScreen = new Rectangle(0, 0, 1920, 1040);
		static readonly Rectangle gRightScreen = new Rectangle(1920, 0, 2560, 1400);

		[TestMethod]
		public void CenterIn_CentersOnTheOwner()
		{
			Point p = CenteredMessageBox.CenterIn(new Rectangle(100, 100, 1000, 800), new Size(400, 200), gLeftScreen);

			Assert.AreEqual(new Point(400, 400), p);
		}

		[TestMethod]
		public void CenterIn_OwnerOnTheSecondMonitor_StaysThere()
		{
			Point p = CenteredMessageBox.CenterIn(new Rectangle(2000, 100, 1200, 900), new Size(400, 200), gRightScreen);

			Assert.AreEqual(new Point(2400, 450), p);
		}

		// Owner partly off its monitor: the box is moved back into the working area.
		[TestMethod]
		public void CenterIn_OwnerPartlyOffScreen_IsClamped()
		{
			Point right = CenteredMessageBox.CenterIn(new Rectangle(1700, 900, 800, 600), new Size(400, 200), gLeftScreen);
			Point left = CenteredMessageBox.CenterIn(new Rectangle(-700, -500, 800, 600), new Size(400, 200), gLeftScreen);

			Assert.AreEqual(new Point(1520, 840), right);
			Assert.AreEqual(new Point(0, 0), left);
		}

		// A monitor to the left of the primary one has negative coordinates.
		[TestMethod]
		public void CenterIn_MonitorWithNegativeCoordinates()
		{
			Rectangle leftMonitor = new Rectangle(-1280, 0, 1280, 984);

			Point p = CenteredMessageBox.CenterIn(new Rectangle(-1200, 50, 1000, 700), new Size(300, 150), leftMonitor);

			Assert.AreEqual(new Point(-850, 325), p);
		}
	}
}
