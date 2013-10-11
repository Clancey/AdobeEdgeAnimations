using System;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Web;

namespace AdobeEdgeAnimations
{
	public static class StageParser
	{
		public static float Scale = 2f;

		public static Stage Parse (string json)
		{
			var data = JObject.Parse (json);
			var stage = ParseStage ((JObject)data ["stage"]);
			//Console.WriteLine (data [0]);

			return stage;
		}

		static Stage ParseStage (JObject obj)
		{
			var stage = new Stage () {
				BaseState = (string)obj ["baseState"],
				InitialState = (string)obj ["initialState"],
				Content = ParseContent ((JArray)obj ["content"] ["dom"]),
				States = ParseStates ((JObject)obj ["states"]),
				Timelines = ParseTimelines (obj ["timelines"]),
			};

			return stage;
		}

		static List<EdgeObject> ParseContent (JArray items)
		{
			var objects = new List<EdgeObject> ();
			foreach (var obj in items)
				objects.Add (ParseObject ((JObject)obj));
			return objects;
		}

		static EdgeObject ParseObject (JObject obj)
		{
			var type = (EdgeObjectType)Enum.Parse (typeof(EdgeObjectType), (string)obj ["type"], true);
			EdgeObject edgeObject;
			switch (type) {
			case EdgeObjectType.Image:
				edgeObject = new EdgeImage {
					ImageName = HttpUtility.UrlDecode ((string)obj ["fill"] [1]),
				};
				break;
			case EdgeObjectType.Text:
				edgeObject = new EdgeText {
					Text = (string)obj ["text"],
					Font = ParseFont ((JArray)obj ["font"]),
				};
				break;
			default:
				edgeObject = new EdgeObject ();
				break;
			}
			bool autoWidth;
			bool autoHeight;
			edgeObject.Name = (string)obj ["id"];
			edgeObject.Rect = ParseRect ((JArray)obj ["rect"], out autoWidth, out autoHeight);
			edgeObject.AutoWidth = autoWidth;
			edgeObject.AutoHeight = autoHeight;
			edgeObject.Hidden = Parsehidden (obj);
			edgeObject.Opacity = ParseOpacity (obj);
			return edgeObject;
		}

		static EdgeFont ParseFont (JArray items)
		{
			return new EdgeFont {
				Name = ((string)items [0]).Replace ("'", ""),
				Size = (int)((int)items [1] / Scale),
				Color = ParseColor ((string)items [2]),
			};
		}

		public static EdgeColor ParseColor (string value)
		{
			var values = value.Replace ("rgba(", "").Replace (")", "").Split (',');
			return new EdgeColor () {
				Red = int.Parse (values [0]),
				Green = int.Parse (values [1]),
				Blue = int.Parse (values [2]),
				Alpha = float.Parse (values [3])
			};
		}

		static RectangleF ParseRect (JArray items, out bool autoWidth, out bool autoHeight)
		{
			var rect = new RectangleF (
				           ParseFloat (items [0]),
				           ParseFloat (items [1]),
				           ParseFloat (items [2]),
				           ParseFloat (items [3])
			           );
			autoWidth = (string)items [4] == "auto";
			autoHeight = (string)items [5] == "auto";
			return rect;
		}

		static float ParseFloat (JToken token)
		{
			return ParseFloat ((string)token);
		}

		public static float ParseFloat (string val)
		{
			if (val == "auto")
				return 0f;
			val = val.Replace ("px", "");
			return float.Parse (val) / Scale;
		}

		static float ParseOpacity (JObject obj)
		{
			JToken token;
			if (!obj.TryGetValue ("opacity", out token))
				return 1f;
			return (float)token;
		}

		static bool Parsehidden (JObject obj)
		{
			JToken token;
			if (obj.TryGetValue ("display", out token))
				return (string)token == "none";
			return false;
		}

		public static List<State> ParseStates (JToken token)
		{
			if (token.Type == JTokenType.Object)
				return new List<State> {
					ParseState ((JProperty)token.First),
				};
			return token.Select (x => ParseState ((JProperty)x)).ToList ();
		}

		static State ParseState (JProperty prop)
		{
			var state = new State {
				Name = prop.Name,
			};
			foreach (var token in prop.Value.Cast<JProperty>())
				state.ObjectStates.Add (token.Name, ParseObjectState ((JArray)token.Value.First));
			return state;
		}

		static EdgeObjectState ParseObjectState (JArray items)
		{
			return ParseObjectState (items.Select(x=> (string)x).ToArray());
		}

		static EdgeObjectState ParseObjectState (object[] items)
		{
			var type = (EdgeObjectStateType)Enum.Parse (typeof(EdgeObjectStateType), (string)items [0], true);
			var propString = (string)items [1];
			EdgePropertyType propType;
			if (!Enum.TryParse (propString, true, out propType)) {
				if (propString == "font-family")
					propType = EdgePropertyType.Font;
				else if (propString == "font-size")
					propType = EdgePropertyType.FontSize;
				else if (propString == "background-color")
					propType = EdgePropertyType.Color;
				else {
					propType = EdgePropertyType.Bottom;
					Console.WriteLine (propString);
				}
			}

			return new EdgeObjectState {
				PropertyType = type,
				Property = propType,
				Value = (string)items [2]
			};

		}

		static List<Timeline> ParseTimelines (JToken token)
		{
			if (token.Type == JTokenType.Object)
				return new List<Timeline> {
					ParseTimeline ((JProperty)token.First),
				};
			return token.Select (x => ParseTimeline ((JProperty)x)).ToList ();
		}

		static Timeline ParseTimeline (JProperty prop)
		{
			var timeline = new Timeline {
				Name = prop.Name,
				FromState = (string)prop.Value["fromState"],
				ToState = (string)prop.Value["toState"],
				Duration = (int)prop.Value["duration"],
				AutoPlay = (bool)prop.Value["autoPlay"],
			};

			foreach (var token in  (JArray)prop.Value["timeline"])
				timeline.Frames.Add (ParseKeyframe(token));
			timeline.Frames = timeline.Frames.OrderBy (x => x.Position).ToList ();
			return timeline;
		}
		static KeyFrame ParseKeyframe(JToken token)
		{
			var keyFrame = new KeyFrame {
				Name = (string)token["id"],
				Position = (int)token["position"],
				Duration = (int)token["duration"],
			};

			JToken easing;
			if (((JObject)token).TryGetValue ("easing", out easing)) {
				var eas = (string)easing;
				if (eas == "easeOutCubic")
					keyFrame.Easing = EdgeEasing.EaseOut;
			}


			var tween = token["tween"] as JArray;
			keyFrame.ObjectName = (string)tween [1];
			keyFrame.Tween = ParseObjectState(new []{(string)tween[0],(string)tween[2],(string)tween[3]});
			keyFrame.FromValue = ((string)tween [4] ["fromValue"]).Replace ("'", "");

			return keyFrame;
		}


	}
}

