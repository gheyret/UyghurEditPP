/*
 * Created by SharpDevelop.
 * User: nk1449
 * Date: 2014/11/04
 * Time: 8:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace UyghurEditPP
{
	partial class ResimRamka
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// ResimRamka
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Name = "ResimRamka";
			this.Size = new System.Drawing.Size(420, 259);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ResimRamkaPaint);
			this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ResimRamkaMouseClick);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ResimRamkaMouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ResimRamkaMouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ResimRamkaMouseUp);
			this.ResumeLayout(false);

		}
	}
}
