using System;
using System.Collections.Generic;

namespace AdobeEdgeAnimations
{
	public class Stage
	{
		public Stage ()
		{
		}
		public EdgeColor Background { get; set; }
		public List<State> States = new List<State>();
		public List<EdgeObject> Content = new List<EdgeObject>();
		public string BaseState {get;set;}
		public string InitialState {get;set;}

		public List<Timeline> Timelines = new List<Timeline>();

		public Dictionary<string,object> NaviveViews = new Dictionary<string, object>();
		public bool IsAnimating {get;set;}
		public int CurrentAnimationTime {get;set;}
	}
}

