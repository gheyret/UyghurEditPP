using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using WpfBrush = System.Windows.Media.SolidColorBrush;
using WpfColor = System.Windows.Media.Color;

namespace UyghurEditPP
{
	/// <summary>
	/// Puts the colors of a theme on windows, their controls and the editor. In the light theme
	/// every value goes back to what it was before (the designer's or the control's default),
	/// so the light theme looks as the program always did.
	/// </summary>
	public static class UiThemer
	{
		// The values a control had before the dark theme was put on it.
		sealed class Kona
		{
			public Color BackColor, ForeColor, LinkColor;
			public FlatStyle FlatStyle;
			public bool UseVisualStyleBackColor;
		}
		static readonly ConditionalWeakTable<Control, Kona> gKona = new ConditionalWeakTable<Control, Kona>();
		static readonly ConditionalWeakTable<Form, object> gTitleHooked = new ConditionalWeakTable<Form, object>();

		/// <summary>A window of its own (OCR, About ...): its colors and its title bar.</summary>
		public static void Apply(Form form, UiTheme theme)
		{
			ApplyControls(form, theme);
			if(form.IsHandleCreated){
				UiTheme.SetTitleBar(form.Handle, theme.IsDark);
			}
			// A window gets a new handle when it is created, and again when its right-to-left
			// layout changes (OCR in the UEY UI); give each new handle the title bar too.
			object hooked;
			if(!gTitleHooked.TryGetValue(form, out hooked)){
				gTitleHooked.Add(form, form);
				form.HandleCreated += (s, e) => UiTheme.SetTitleBar(form.Handle, UiTheme.Current.IsDark);
			}
		}

		static void ApplyControls(Control c, UiTheme theme)
		{
			if(c is ToolStrip || c is System.Windows.Forms.Integration.ElementHost){
				return; // drawn by UiRenderer / themed as WPF
			}
			Kona kona = gKona.GetValue(c, x => {
				Kona k = new Kona { BackColor = x.BackColor, ForeColor = x.ForeColor };
				ButtonBase b = x as ButtonBase;
				if(b != null){ k.FlatStyle = b.FlatStyle; k.UseVisualStyleBackColor = b.UseVisualStyleBackColor; }
				LinkLabel l = x as LinkLabel;
				if(l != null){ k.LinkColor = l.LinkColor; }
				return k;
			});
			bool dark = theme.IsDark;
			Button button = c as Button;
			if(button != null){
				button.FlatStyle = dark ? FlatStyle.Flat : kona.FlatStyle;
				button.FlatAppearance.BorderColor = theme.BarLine;
				button.BackColor = dark ? theme.ButtonBack : kona.BackColor;
				button.ForeColor = dark ? theme.FormText : kona.ForeColor;
				if(!dark){
					button.UseVisualStyleBackColor = kona.UseVisualStyleBackColor;
				}
			}
			else if(c is TextBoxBase){
				c.BackColor = dark ? theme.EditorBack : kona.BackColor;
				c.ForeColor = dark ? theme.EditorText : kona.ForeColor;
			}
			else{
				c.BackColor = dark ? theme.FormBack : kona.BackColor;
				c.ForeColor = dark ? theme.FormText : kona.ForeColor;
				LinkLabel link = c as LinkLabel;
				if(link != null){
					link.LinkColor = dark ? theme.LinkText : kona.LinkColor;
				}
			}
			foreach(Control child in c.Controls){
				ApplyControls(child, theme);
			}
		}

		static WpfBrush Brush(Color c)
		{
			WpfBrush b = new WpfBrush(WpfColor.FromArgb(c.A, c.R, c.G, c.B));
			b.Freeze();
			return b;
		}

		static void SetOrClear(System.Windows.DependencyObject d, System.Windows.DependencyProperty p, bool dark, object value)
		{
			if(dark){
				d.SetValue(p, value);
			}
			else{
				d.ClearValue(p);
			}
		}

		/// <summary>The editor: background, text, line numbers, selection and caret.</summary>
		public static void Apply(TextEditor editor, UiTheme theme)
		{
			bool dark = theme.IsDark;
			SetOrClear(editor, System.Windows.Controls.Control.BackgroundProperty, dark, Brush(theme.EditorBack));
			SetOrClear(editor, System.Windows.Controls.Control.ForegroundProperty, dark, Brush(theme.EditorText));
			SetOrClear(editor, TextEditor.LineNumbersForegroundProperty, dark, Brush(theme.LineNumbers));
			SetOrClear(editor.TextArea, UyghurEditPP.Editing.TextArea.SelectionBrushProperty, dark, Brush(Color.FromArgb(0xB4, theme.Selection)));
			SetOrClear(editor.TextArea, UyghurEditPP.Editing.TextArea.SelectionForegroundProperty, dark, Brush(theme.EditorText));
			SetOrClear(editor.TextArea, UyghurEditPP.Editing.TextArea.SelectionBorderProperty, dark, null);
			editor.TextArea.Caret.CaretBrush = dark ? Brush(theme.Caret) : null;
			UyghurEditPP.Editing.LineNumberMargin.CurrentLineNumberBrush = dark ? Brush(Color.FromArgb(0x56, 0x9C, 0xD6)) : System.Windows.Media.Brushes.DarkBlue;
			// The scroll bars (their usual look ignores the colors).
			UiDarkStyles.ScrollBars(editor.Resources, theme);
		}

		// Original local values of WPF elements, as for the WinForms controls above.
		static readonly ConditionalWeakTable<System.Windows.DependencyObject, Dictionary<System.Windows.DependencyProperty, object>> gWpfKona =
			new ConditionalWeakTable<System.Windows.DependencyObject, Dictionary<System.Windows.DependencyProperty, object>>();

		static void SetOrRestore(System.Windows.DependencyObject d, System.Windows.DependencyProperty p, bool dark, object value)
		{
			Dictionary<System.Windows.DependencyProperty, object> kona = gWpfKona.GetOrCreateValue(d);
			if(!kona.ContainsKey(p)){
				kona[p] = d.ReadLocalValue(p);
			}
			if(dark){
				d.SetValue(p, value);
			}
			else if(kona[p] == System.Windows.DependencyProperty.UnsetValue){
				d.ClearValue(p);
			}
			else{
				d.SetValue(p, kona[p]);
			}
		}

		/// <summary>A WPF window (the find window) or menu: backgrounds and text of its parts.</summary>
		public static void Apply(System.Windows.DependencyObject root, UiTheme theme)
		{
			bool dark = theme.IsDark;
			System.Windows.Window window = root as System.Windows.Window;
			if(window != null){
				SetOrRestore(window, System.Windows.Controls.Control.BackgroundProperty, dark, Brush(theme.FormBack));
				IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(window).Handle;
				UiTheme.SetTitleBar(hwnd, dark);
				UiDarkStyles.Toggles(window.Resources, theme);
			}
			ApplyWpf(root, theme);
		}

		static void ApplyWpf(System.Windows.DependencyObject d, UiTheme theme)
		{
			bool dark = theme.IsDark;
			if(d is System.Windows.Controls.TextBox){
				SetOrRestore(d, System.Windows.Controls.Control.BackgroundProperty, dark, Brush(theme.EditorBack));
				SetOrRestore(d, System.Windows.Controls.Control.ForegroundProperty, dark, Brush(theme.EditorText));
				SetOrRestore(d, System.Windows.Controls.TextBox.CaretBrushProperty, dark, Brush(theme.Caret));
			}
			else if(d is System.Windows.Controls.Primitives.ButtonBase && !(d is System.Windows.Controls.Primitives.ToggleButton)){
				SetOrRestore(d, System.Windows.Controls.Control.BackgroundProperty, dark, Brush(theme.ButtonBack));
				SetOrRestore(d, System.Windows.Controls.Control.ForegroundProperty, dark, Brush(theme.FormText));
				SetOrRestore(d, System.Windows.Controls.Control.BorderBrushProperty, dark, Brush(theme.BarLine));
			}
			else if(d is System.Windows.Controls.Control){
				SetOrRestore(d, System.Windows.Controls.Control.ForegroundProperty, dark, Brush(theme.FormText));
				if(d is System.Windows.Controls.ContextMenu || d is System.Windows.Controls.MenuItem){
					SetOrRestore(d, System.Windows.Controls.Control.BackgroundProperty, dark, Brush(theme.MenuBack));
				}
			}
			else if(d is System.Windows.Controls.TextBlock){
				SetOrRestore(d, System.Windows.Controls.TextBlock.ForegroundProperty, dark, Brush(theme.FormText));
				if(d.ReadLocalValue(System.Windows.Controls.TextBlock.BackgroundProperty) != System.Windows.DependencyProperty.UnsetValue || gWpfKona.GetOrCreateValue(d).ContainsKey(System.Windows.Controls.TextBlock.BackgroundProperty)){
					SetOrRestore(d, System.Windows.Controls.TextBlock.BackgroundProperty, dark, Brush(theme.ButtonBack));
				}
			}
			foreach(object child in System.Windows.LogicalTreeHelper.GetChildren(d)){
				System.Windows.DependencyObject c = child as System.Windows.DependencyObject;
				if(c != null){
					ApplyWpf(c, theme);
				}
			}
		}
	}
}
