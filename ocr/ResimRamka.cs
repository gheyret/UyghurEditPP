/*
 * Created by SharpDevelop.
 * User: nk1449
 * Date: 2014/11/04
 * Time: 8:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace UyghurEditPP
{
	/// <summary>
	/// Description of ResimRamka.
	/// </summary>
	/// 
	public partial class ResimRamka : UserControl
	{
		public delegate void   dlSeletedImage(Bitmap org);
		public delegate void   dlSelectedArea(int x, int y, int w, int h);
		public delegate void   dlMousePoint(int x, int y);
		
		private Bitmap img_ORG=null;
		
		private float _rt=1.0f;
		private int _showX=0,_showY=0;
		private int _showW=0,_showH=0;

		private int _selX=-1,_selY=-1;

		private int _fW=-1;
		private int _fH=-1;
		private bool _isPainting=false;
		private bool _isBesildi=false;

		private Pen  talrayon=new Pen(Color.Red);
		private Pen  penOtturi=new Pen(Color.SkyBlue);
		
		public  dlSeletedImage Selected = null;
		public  dlSelectedArea SelectedArea = null;
		public  dlMousePoint   MousePoint = null;
		public  dlMousePoint   RightMouseClick = null;
		
		
		private int _stBasX=0;
		private int _stBasY=0;
		private int _stSelX=0;
		private int _stSelY=0;
		private int _stSelW=0;
		private int _stSelH=0;
		public ResimRamka()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer|ControlStyles.ResizeRedraw, true);
			this.UpdateStyles();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		public Bitmap Image{
			set{
				_selX=-1;
				_selY=-1;
				_fW = -1;
				_fH = -1;
				if(value!=null){
					if(img_ORG!=null){
						img_ORG.Dispose();
					}
					img_ORG = value;
				}
				Invalidate();
			}
			get{
				return img_ORG;
			}
		}
		
		
		void ResimRamkaPaint(object sender, PaintEventArgs e)
		{
			if(_isPainting)return;
			_isPainting=true;
			int x=0,y=0;
			float rtw,rth;
			int panW=this.Width;
			int panH=this.Height;
			int imgH,imgW;
			if(img_ORG!=null){
				imgH=img_ORG.Height;
				imgW=img_ORG.Width;
				rth=(float)panH/(float)imgH;
				rtw=(float)panW/(float)imgW;
				if(rth>rtw){
					_rt=rtw;
				}
				else{
					_rt=rth;
				}
				_showW=(int)(imgW*_rt);
				_showH=(int)(imgH*_rt);
				if (panW > _showW) {
					x = (panW - _showW) / 2;
				}
				if (panH > _showH) {
					y = (panH - _showH) / 2;
				}

				_showX=x;
				_showY=y;
				e.Graphics.InterpolationMode=InterpolationMode.HighQualityBilinear;
				e.Graphics.DrawImage(img_ORG, _showX,_showY, _showW, _showH);
				if((_fW!=-1) && (_fH!=-1))
				{
					e.Graphics.DrawRectangle(talrayon,(int)(_selX*_rt)+_showX,(int)(_selY*_rt)+_showY,(int)(_fW*_rt),(int)(_fH*_rt));
				}
			}
			_isPainting=false;
		}
		
		void ResimRamkaMouseMove(object sender, MouseEventArgs e)
		{
			int w,h,posx=0,posy=0;
			int difX,difY;
			if(img_ORG==null) return;
			if(e.Button== MouseButtons.Left && _isBesildi ){
				w = (int) (img_ORG.Width * _rt);
				h = (int) (img_ORG.Height * _rt);
				difX=_stBasX-e.X;
				difY=_stBasY-e.Y;

				posy=(int)(_stSelH-difY);
				posx=(int)(_stSelW-difX);
				if(posx<0)posx=0;
				if(posy<0)posy=0;
				if((posy+_selY*_rt)<h){
					_fH=(int)(posy/_rt);
				}
				if((posx+_selX*_rt)<w){
					_fW=(int)(posx/_rt);
				}
				Invalidate();
			}
			else if(MousePoint!=null){
				posx=(int)(( e.X-_showX)/_rt);
				posy=(int)((e.Y-_showY)/_rt);
				if(posx<0)
					posx = 0;
				if(posx >= img_ORG.Width)
					posx = img_ORG.Width-1;
				
				if(posy<0)
					posy = 0;
				if(posy >= img_ORG.Height)
					posy = img_ORG.Height-1;
				
				MousePoint(posx,posy);
			}
		}
		
		void ResimRamkaMouseDown(object sender, MouseEventArgs e)
		{
			if(img_ORG==null) return;
			if(e.Button== MouseButtons.Left){
				_isBesildi=true;
				_stBasX = e.X;
				_stBasY = e.Y;
				
				_selX=(int)((_stBasX-_showX)/_rt);
				_selY=(int)((_stBasY-_showY)/_rt);
				
				_fW= -1;
				_fH = -1;
				
				_stSelX=(int)(_selX*_rt);
				_stSelY=(int)(_selY*_rt);
				
				_stSelW=(int)(_fW*_rt);
				_stSelH=(int)(_fH*_rt);
			}
			Invalidate();
		}
		
		void ResimRamkaMouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left){
				if(Selected!=null && _fH>0 && _fW>0){
					Selected(img_ORG.Clone(new Rectangle(_selY,_selY+_fH,_selX,_selX+_fW),img_ORG.PixelFormat));
				}
				
				if(SelectedArea!=null && _fH>0 && _fW>0){
					SelectedArea(_selX,_selY, _fW, _fH);
				}
			}
			_isBesildi = false;
		}
		
		public void SetROI(int x, int y, int w, int h)
		{
			_selX = x;
			_selY = y;
			_fW   = w;
			_fH   = h;
			
			Invalidate();
		}
		
		public Rectangle getRoi(){
			if(_fW!=-1 && _fH!=-1)
			{
				return new Rectangle(_selX,_selY,_fW,_fH);
			}
			else{
				return new Rectangle(0,0,img_ORG.Width,img_ORG.Height);
			}
		}
		
		void ResimRamkaMouseClick(object sender, MouseEventArgs e)
		{
			int posx=0,posy=0;
			if(img_ORG==null) return;
			if(e.Button == MouseButtons.Right && RightMouseClick!=null){
				posx=(int)(( e.X-_showX)/_rt);
				posy=(int)((e.Y-_showY)/_rt);
				if(posx<0)
					posx = 0;
				if(posx >= img_ORG.Width)
					posx = img_ORG.Width-1;
				
				if(posy<0)
					posy = 0;
				if(posy >= img_ORG.Height)
					posy = img_ORG.Height-1;
				
				RightMouseClick(posx,posy);
			}
		}
	}
}
