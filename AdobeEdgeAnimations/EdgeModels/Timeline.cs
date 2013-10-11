using System;
using System.Collections.Generic;

namespace AdobeEdgeAnimations
{
	public class Timeline
	{
		public Timeline ()
		{
		}
		public string Name {get;set;}
		public string FromState { get; set; }
		public string ToState {get;set;}
		public int Duration { get; set; }
		public bool AutoPlay {get;set;}
		public List<KeyFrame> Frames = new List<KeyFrame>();
	}
}

