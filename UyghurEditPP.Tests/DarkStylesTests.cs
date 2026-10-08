using System;
using System.Threading;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UyghurEditPP.Tests
{
	[TestClass]
	public class DarkStylesTests
	{
		// WPF objects need an STA thread.
		static void OnSta(Action action)
		{
			Exception error = null;
			Thread t = new Thread(() => { try{ action(); } catch(Exception ee){ error = ee; } });
			t.SetApartmentState(ApartmentState.STA);
			t.Start();
			t.Join();
			if(error != null){
				throw new AssertFailedException(error.ToString());
			}
		}

		[TestMethod]
		public void DarkStyles_AreAddedAndRemoved()
		{
			OnSta(() => {
				ResourceDictionary res = new ResourceDictionary();
				UiDarkStyles.ScrollBars(res, UiTheme.Dark);
				UiDarkStyles.Toggles(res, UiTheme.Dark);
				Assert.IsInstanceOfType(res[typeof(System.Windows.Controls.Primitives.ScrollBar)], typeof(Style));
				Assert.IsInstanceOfType(res[typeof(System.Windows.Controls.CheckBox)], typeof(Style));
				Assert.IsInstanceOfType(res[typeof(System.Windows.Controls.RadioButton)], typeof(Style));

				// The templates must build: apply them to real controls.
				System.Windows.Controls.Primitives.ScrollBar bar = new System.Windows.Controls.Primitives.ScrollBar();
				bar.Style = (Style)res[typeof(System.Windows.Controls.Primitives.ScrollBar)];
				Assert.IsTrue(bar.ApplyTemplate());
				bar.Orientation = System.Windows.Controls.Orientation.Horizontal;
				bar.ApplyTemplate();
				System.Windows.Controls.CheckBox box = new System.Windows.Controls.CheckBox();
				box.Style = (Style)res[typeof(System.Windows.Controls.CheckBox)];
				Assert.IsTrue(box.ApplyTemplate());

				UiDarkStyles.ScrollBars(res, UiTheme.Light);
				UiDarkStyles.Toggles(res, UiTheme.Light);
				Assert.AreEqual(0, res.Count);
			});
		}
	}
}
