using System;
using System.Drawing;
using System.Windows;
using System.Windows.Markup;

namespace UyghurEditPP
{
	/// <summary>
	/// WPF styles for the dark theme: the editor's scroll bars and the find window's check
	/// boxes and radio buttons. The usual Windows look of these controls ignores the colors
	/// set on them, so they get simple templates of their own; in the light theme they are
	/// removed again and the controls look as before.
	/// </summary>
	static class UiDarkStyles
	{
		const string Ns = "xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'";

		const string ScrollBarXaml = @"
<Style TargetType='ScrollBar' NS>
  <Style.Resources>
    <Style x:Key='Page' TargetType='RepeatButton'>
      <Setter Property='Focusable' Value='False'/>
      <Setter Property='IsTabStop' Value='False'/>
      <Setter Property='Template'><Setter.Value>
        <ControlTemplate TargetType='RepeatButton'><Border Background='Transparent'/></ControlTemplate>
      </Setter.Value></Setter>
    </Style>
    <Style x:Key='Arrow' TargetType='RepeatButton'>
      <Setter Property='Focusable' Value='False'/>
      <Setter Property='IsTabStop' Value='False'/>
      <Setter Property='Foreground' Value='ARROW'/>
      <Setter Property='Template'><Setter.Value>
        <ControlTemplate TargetType='RepeatButton'>
          <Border x:Name='b' Background='Transparent'><ContentPresenter HorizontalAlignment='Center' VerticalAlignment='Center'/></Border>
          <ControlTemplate.Triggers>
            <Trigger Property='IsMouseOver' Value='True'><Setter TargetName='b' Property='Background' Value='HOVERBACK'/></Trigger>
          </ControlTemplate.Triggers>
        </ControlTemplate>
      </Setter.Value></Setter>
    </Style>
    <Style x:Key='Thumb' TargetType='Thumb'>
      <Setter Property='Template'><Setter.Value>
        <ControlTemplate TargetType='Thumb'>
          <Border x:Name='t' Background='THUMB' CornerRadius='3' Margin='3'/>
          <ControlTemplate.Triggers>
            <Trigger Property='IsMouseOver' Value='True'><Setter TargetName='t' Property='Background' Value='THUMBHOVER'/></Trigger>
            <Trigger Property='IsDragging' Value='True'><Setter TargetName='t' Property='Background' Value='THUMBHOVER'/></Trigger>
          </ControlTemplate.Triggers>
        </ControlTemplate>
      </Setter.Value></Setter>
    </Style>
  </Style.Resources>
  <Setter Property='Background' Value='TRACK'/>
  <Setter Property='Width' Value='{x:Static SystemParameters.VerticalScrollBarWidth}'/>
  <Setter Property='MinWidth' Value='{x:Static SystemParameters.VerticalScrollBarWidth}'/>
  <Setter Property='Template'><Setter.Value>
    <ControlTemplate TargetType='ScrollBar'>
      <Grid Background='{TemplateBinding Background}'>
        <Grid.RowDefinitions>
          <RowDefinition Height='Auto'/>
          <RowDefinition Height='*'/>
          <RowDefinition Height='Auto'/>
        </Grid.RowDefinitions>
        <RepeatButton Grid.Row='0' Style='{StaticResource Arrow}' Height='{x:Static SystemParameters.VerticalScrollBarButtonHeight}' Command='ScrollBar.LineUpCommand'><Path Data='M 0 4 L 8 4 L 4 0 Z' Fill='ARROW'/></RepeatButton>
        <Track x:Name='PART_Track' Grid.Row='1' IsDirectionReversed='True'>
          <Track.DecreaseRepeatButton><RepeatButton Style='{StaticResource Page}' Command='ScrollBar.PageUpCommand'/></Track.DecreaseRepeatButton>
          <Track.Thumb><Thumb Style='{StaticResource Thumb}'/></Track.Thumb>
          <Track.IncreaseRepeatButton><RepeatButton Style='{StaticResource Page}' Command='ScrollBar.PageDownCommand'/></Track.IncreaseRepeatButton>
        </Track>
        <RepeatButton Grid.Row='2' Style='{StaticResource Arrow}' Height='{x:Static SystemParameters.VerticalScrollBarButtonHeight}' Command='ScrollBar.LineDownCommand'><Path Data='M 0 0 L 8 0 L 4 4 Z' Fill='ARROW'/></RepeatButton>
      </Grid>
    </ControlTemplate>
  </Setter.Value></Setter>
  <Style.Triggers>
    <Trigger Property='Orientation' Value='Horizontal'>
      <Setter Property='Width' Value='Auto'/>
      <Setter Property='MinWidth' Value='0'/>
      <Setter Property='Height' Value='{x:Static SystemParameters.HorizontalScrollBarHeight}'/>
      <Setter Property='MinHeight' Value='{x:Static SystemParameters.HorizontalScrollBarHeight}'/>
      <Setter Property='Template'><Setter.Value>
        <ControlTemplate TargetType='ScrollBar'>
          <Grid Background='{TemplateBinding Background}'>
            <Grid.ColumnDefinitions>
              <ColumnDefinition Width='Auto'/>
              <ColumnDefinition Width='*'/>
              <ColumnDefinition Width='Auto'/>
            </Grid.ColumnDefinitions>
            <RepeatButton Grid.Column='0' Style='{StaticResource Arrow}' Width='{x:Static SystemParameters.HorizontalScrollBarButtonWidth}' Command='ScrollBar.LineLeftCommand'><Path Data='M 4 0 L 4 8 L 0 4 Z' Fill='ARROW'/></RepeatButton>
            <Track x:Name='PART_Track' Grid.Column='1'>
              <Track.DecreaseRepeatButton><RepeatButton Style='{StaticResource Page}' Command='ScrollBar.PageLeftCommand'/></Track.DecreaseRepeatButton>
              <Track.Thumb><Thumb Style='{StaticResource Thumb}'/></Track.Thumb>
              <Track.IncreaseRepeatButton><RepeatButton Style='{StaticResource Page}' Command='ScrollBar.PageRightCommand'/></Track.IncreaseRepeatButton>
            </Track>
            <RepeatButton Grid.Column='2' Style='{StaticResource Arrow}' Width='{x:Static SystemParameters.HorizontalScrollBarButtonWidth}' Command='ScrollBar.LineRightCommand'><Path Data='M 0 0 L 0 8 L 4 4 Z' Fill='ARROW'/></RepeatButton>
          </Grid>
        </ControlTemplate>
      </Setter.Value></Setter>
    </Trigger>
  </Style.Triggers>
</Style>";

		const string CheckBoxXaml = @"
<Style TargetType='CheckBox' NS>
  <Setter Property='Template'><Setter.Value>
    <ControlTemplate TargetType='CheckBox'>
      <StackPanel Orientation='Horizontal' Background='Transparent'>
        <Border x:Name='box' Width='14' Height='14' BorderBrush='BOXBORDER' BorderThickness='1' Background='BOXBACK' VerticalAlignment='Center'>
          <Path x:Name='mark' Data='M 2 6.5 L 5.5 10 L 11.5 3' Stroke='MARK' StrokeThickness='2' Visibility='Collapsed'/>
        </Border>
        <ContentPresenter Margin='5,0,0,0' VerticalAlignment='Center'/>
      </StackPanel>
      <ControlTemplate.Triggers>
        <Trigger Property='IsChecked' Value='True'><Setter TargetName='mark' Property='Visibility' Value='Visible'/></Trigger>
        <Trigger Property='IsMouseOver' Value='True'><Setter TargetName='box' Property='BorderBrush' Value='ACCENT'/></Trigger>
      </ControlTemplate.Triggers>
    </ControlTemplate>
  </Setter.Value></Setter>
</Style>";

		const string RadioButtonXaml = @"
<Style TargetType='RadioButton' NS>
  <Setter Property='Template'><Setter.Value>
    <ControlTemplate TargetType='RadioButton'>
      <StackPanel Orientation='Horizontal' Background='Transparent'>
        <Grid Width='14' Height='14' VerticalAlignment='Center'>
          <Ellipse x:Name='ring' Stroke='BOXBORDER' StrokeThickness='1' Fill='BOXBACK'/>
          <Ellipse x:Name='dot' Width='6' Height='6' Fill='MARK' Visibility='Collapsed'/>
        </Grid>
        <ContentPresenter Margin='5,0,0,0' VerticalAlignment='Center'/>
      </StackPanel>
      <ControlTemplate.Triggers>
        <Trigger Property='IsChecked' Value='True'><Setter TargetName='dot' Property='Visibility' Value='Visible'/></Trigger>
        <Trigger Property='IsMouseOver' Value='True'><Setter TargetName='ring' Property='Stroke' Value='ACCENT'/></Trigger>
      </ControlTemplate.Triggers>
    </ControlTemplate>
  </Setter.Value></Setter>
</Style>";

		static string Hex(Color c)
		{
			return string.Format("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);
		}

		static Style Parse(string xaml, UiTheme theme)
		{
			xaml = xaml.Replace(" NS>", " " + Ns + ">")
				.Replace("THUMBHOVER", "#686868").Replace("THUMB", "#4A4A4A")
				.Replace("HOVERBACK", Hex(theme.TabHover)).Replace("TRACK", Hex(theme.TabStrip))
				.Replace("ARROW", "#9E9E9E")
				.Replace("BOXBORDER", "#8A8A8A").Replace("BOXBACK", Hex(theme.EditorBack))
				.Replace("MARK", Hex(theme.BarText)).Replace("ACCENT", Hex(theme.Accent));
			return (Style)XamlReader.Parse(xaml);
		}

		/// <summary>Dark scroll bars in res (dark), or the usual ones again (light).</summary>
		public static void ScrollBars(ResourceDictionary res, UiTheme theme)
		{
			Set(res, typeof(System.Windows.Controls.Primitives.ScrollBar), theme, ScrollBarXaml);
		}

		/// <summary>Dark check boxes and radio buttons in res (dark), or the usual ones (light).</summary>
		public static void Toggles(ResourceDictionary res, UiTheme theme)
		{
			Set(res, typeof(System.Windows.Controls.CheckBox), theme, CheckBoxXaml);
			Set(res, typeof(System.Windows.Controls.RadioButton), theme, RadioButtonXaml);
		}

		static void Set(ResourceDictionary res, Type type, UiTheme theme, string xaml)
		{
			if(theme.IsDark){
				try{
					res[type] = Parse(xaml, theme);
				}
				catch(Exception ee){
					ErrorLog.Write(ee); // keep the usual look rather than fail
				}
			}
			else{
				res.Remove(type);
			}
		}
	}
}
