using System;
using System.Collections.Generic;

namespace AdobeEdgeAnimations
{
	public class State
	{
		public State ()
		{
		}

		public string Name { get; set; }
		public Dictionary<string,EdgeObjectState> ObjectStates = new Dictionary<string, EdgeObjectState>();
	}
}

