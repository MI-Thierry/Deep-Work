using Microsoft.UI.Xaml;
using SkiaSharp;

namespace DeepWork.Themes
{
	public class GraphTheme
	{
		public SKColor LabelsForegroundColor { get; set; }
		public SKColor NamesForegroundColor { get; set; }
		public SKColor SeparatorForegroundColor { get; set; }
		public SKColor SubSeparatorForegroundColor { get; set; }
		public SKColor FillColor { get; set; }
		public SKColor StrokeColor { get; set; }
		public SKColor TicksForegroundColor { get; set; }
		public SKColor SubTicksForegroundColor { get; set; }

		public static GraphTheme Light { get; } = new()
		{
			LabelsForegroundColor = new SKColor(0xff3d3d3d),
			NamesForegroundColor = new SKColor(0xff5e5e5e),
			SeparatorForegroundColor = new SKColor(0xff3d3d3d),
			SubSeparatorForegroundColor = new SKColor(0xff5a5a5a),
			FillColor = new SKColor(0x59008aff),
			StrokeColor = new SKColor(0xedff9605),
			TicksForegroundColor = new SKColor(0xff3d3d3d),
			SubTicksForegroundColor = new SKColor(0xff5a5a5a)
		};

		public static GraphTheme Dark { get; } = new()
		{
			LabelsForegroundColor = new SKColor(0xffc3c3c3),
			NamesForegroundColor = new SKColor(0xffa0a0a0),
			SeparatorForegroundColor = new SKColor(0xffc3c3c3),
			SubSeparatorForegroundColor = new SKColor(0xff5a5a5a),
			FillColor = new SKColor(0x59008aff),
			StrokeColor = new SKColor(0xedff9605),
			TicksForegroundColor = new SKColor(0xffc3c3c3),
			SubTicksForegroundColor = new SKColor(0xff5a5a5a)
		};
		public static GraphTheme Default { get; } = Application.Current.RequestedTheme == ApplicationTheme.Light ? Light : Dark;
	}
}
