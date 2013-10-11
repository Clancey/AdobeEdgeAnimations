using System;
using System.Drawing;

namespace AdobeEdgeAnimations
{
	public class EdgeObject
	{
		public EdgeObject ()
		{
		}
		public string Name {get;set;}
		public bool AutoHeight {get;set;}
		public bool AutoWidth {get;set;}
		public RectangleF Rect {get;set;}
		public float Opacity {get;set;}
		public bool Hidden { get; set; }
	}
	public class EdgeImage : EdgeObject
	{
		public string ImageName { get; set; }
	}
	public class EdgeText : EdgeObject
	{
		public EdgeFont Font {get;set;}
		public string Text { get; set; }
	}

	public class EdgeFont
	{
		public string Name {get;set;}
		public int Size { get; set; }
		public EdgeColor Color {get;set;}
	}
}

