using System;

namespace AdobeEdgeAnimations
{
	public enum EdgeEasing 
	{
		None,
		EaseIn,
		EaseOut,
		EaseInAndOut,
	}
	public class KeyFrame
	{
		public KeyFrame ()
		{
		}
		public string ObjectName { get; set; }
		public string Name {get;set;}
		public EdgeObjectState Tween {get;set;}
		public string FromValue {get;set;}
		public int Position {get;set;}
		public int Duration {get;set;}
		public EdgeEasing Easing {get;set;}

		public override string ToString ()
		{
			return string.Format ("[KeyFrame: ObjectName={0}, Name={1}, Tween={2}, FromValue={3}, Position={4}, Duration={5}, Easing={6}]", ObjectName, Name, Tween, FromValue, Position, Duration, Easing);
		}

	}
}

