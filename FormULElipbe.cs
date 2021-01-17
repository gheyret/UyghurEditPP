/*
 * Created by SharpDevelop.
 * User: Gheyret Kenji
 * Date: 2020/11/27
 * Time: 13:14
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UyghurEditPP
{
	/// <summary>
	/// Description of FormULY.
	/// </summary>
	public partial class FormULElipbe : Form
	{
		public FormULElipbe()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void FormULYLoad(object sender, EventArgs e)
		{
			
		}
		void FormULYMouseClick(object sender, MouseEventArgs e)
		{
			this.Close();
			this.Dispose();
		}
	}
}
