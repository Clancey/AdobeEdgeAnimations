using System;

namespace AdobeEdgeAnimations
{
	public enum EdgeObjectStateType
	{
		Style,
		Color,
		Transform,
	}
	public enum EdgePropertyType
	{
		Left,
		Top,
		Right,
		Bottom,
		Opacity,
		Font,
		FontSize,
		Width,
		Height,
		Overflow,
		Display,

		RotateZ,
		RotateX,
		RotateY,

		Color,
	}
	public class EdgeObjectState
	{
		public EdgeObjectStateType PropertyType {get;set;}
		public EdgePropertyType Property { get; set; }
		public string Value {get;set;}
		public override string ToString ()
		{
			return string.Format ("[EdgeObjectState: PropertyType={0}, Property={1}, Value={2}]", PropertyType, Property, Value);
		}
	}
}

