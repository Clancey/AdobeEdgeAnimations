using System;
using MonoTouch.UIKit;
using System.Linq;
using MonoTouch.Foundation;
using System.Drawing;
using System.Timers;
using System.Collections.Generic;
using MonoTouch.CoreAnimation;
using MonoTouch.CoreGraphics;
using System.Threading.Tasks;

namespace AdobeEdgeAnimations
{
	public static class StageExtensions
	{
		public static Task<Stage> Load(string scriptName)
		{
			var tcs = new TaskCompletionSource<Stage> ();
			var webView = new UIWebView ();
			UIApplication.SharedApplication.Windows[0].Add (webView);
			//this.window.AddSubview (webView);
			webView.LoadError += (object sender, UIWebErrorArgs e) => {
				Console.WriteLine(e.Error);
				tcs.TrySetException(new Exception(e.Error.ToString()));
			};
			webView.LoadFinished += (object sender, EventArgs e) => {

				var data = webView.EvaluateJavascript ("window.JSON.stringify(symbols);"); 
				webView.RemoveFromSuperview();
				data = data.Replace("\"${_", "\"").Replace("}\"","\"");
				var stage = StageParser.Parse(data);
				tcs.TrySetResult(stage);
			};

			webView.LoadHtmlString("<script type=\"text/javascript\" src=\"" + scriptName + "\"></script>", new NSUrl(NSBundle.MainBundle.ResourcePath,true));
			return tcs.Task;
		}
		public static UIView ToUIView (this Stage stage)
		{
			var view = new UIView () {

			};
			stage.NaviveViews ["Stage"] = view;
			view.AddSubviews (stage.Content.Select (x => (UIView)(stage.NaviveViews[x.Name] = x.ToUIView ())).ToArray());

			stage.SetState (stage.InitialState, view);

			return view;
		}

		public static UIView ToUIView (this EdgeObject obj)
		{
			UIView view;
			if (obj is EdgeImage) {
				var img = obj as EdgeImage;
				view = new UIImageView (UIImage.FromBundle (img.ImageName));
			} else if (obj is EdgeText) {
				var txt = obj as EdgeText;
				view = new UILabel () {
					Text = txt.Text,
					Font = txt.Font.ToFont(),
					TextColor = txt.Font.Color.ToColor(),
				};
			} else
				view = new UIView ();

			view.Frame = obj.Rect;
			if (view.Frame.Size == SizeF.Empty && (obj.AutoHeight || obj.AutoHeight))
				view.SizeToFit ();
			view.Hidden = obj.Hidden;
			view.Alpha = obj.Opacity;
			return view;
		}

		public static void SetState(this Stage stage, string stateName, UIView view)
		{
			var state = stage.States.FirstOrDefault(x=> x.Name == stateName);
			if(state == null)
				return;

			foreach (var objState in state.ObjectStates)
				objState.Value.SetState (objState.Key,stage);
		}

		public static void SetState(this EdgeObjectState state,string id, Stage Stage)
		{
			var childView = Stage.NaviveViews [id] as UIView;
			if (childView == null)
				return;
			switch (state.PropertyType) {
			case EdgeObjectStateType.Color:
				state.SetColor (childView);
				break;
			case EdgeObjectStateType.Transform:
				state.SetTransform (childView);
				break;
			case EdgeObjectStateType.Style:
				state.SetStyle (childView);
				break;
			}
		}

		static void SetTransform(this EdgeObjectState state,UIView view)
		{
			switch (state.Property) {
			case EdgePropertyType.RotateZ:
				view.Transform = CGAffineTransform.MakeRotation (GetRotation (state.Value));
				return;
			}
		}

		static float GetRotation(string str)
		{
			if (str.Contains ("deg")) {
				var val = int.Parse (str.Replace ("deg", ""));
				return (float)(Math.PI * val / 180f);
			}
			return float.Parse (str);
		}
				

		static void SetStyle(this EdgeObjectState state,UIView view)
		{
			switch (state.Property) {
			case EdgePropertyType.Opacity:
				view.Alpha = float.Parse (state.Value);
				return;
				;
			case EdgePropertyType.Left:
				{
					var frame = view.Frame;
					frame.X = StageParser.ParseFloat (state.Value);
					view.Frame = frame;
				}
				return;
			case EdgePropertyType.Top:
				{
					var frame = view.Frame;
					frame.Y = StageParser.ParseFloat (state.Value);
					view.Frame = frame;
				}
				return;
			case EdgePropertyType.Width:
				{
					var frame = view.Frame;
					frame.Width = StageParser.ParseFloat (state.Value);
					view.Frame = frame;
				}
				return;
			case EdgePropertyType.Height:
				{
					var frame = view.Frame;
					frame.Height = StageParser.ParseFloat (state.Value);
					view.Frame = frame;
				}
				return;
			case EdgePropertyType.Display:
				view.Hidden = state.Value == "none";
				return;
			}
		} 
		public static UIColor ToColor (this EdgeColor edgeColor)
		{
			return UIColor.FromRGBA (edgeColor.Red, edgeColor.Green, edgeColor.Blue, edgeColor.Alpha * 255);
		}
		public static UIFont ToFont(this EdgeFont font)
		{
			return UIFont.FromName (font.Name.Replace(" ",""), font.Size);
		}
		static void SetColor(this EdgeObjectState state,UIView view)
		{
			var values = state.Value.Replace ("rgba(", "").Replace (")", "").Split (',');
			var color = UIColor.FromRGBA (
							int.Parse (values [0]),
							int.Parse (values [1]),
							int.Parse (values [2]),
							int.Parse (values [3]) * 255
			);

			if (view is UILabel) {
				var lbl = view as UILabel;
				lbl.TextColor = color;
				return;
			}
			view.BackgroundColor = color;
		} 


		public static void Animate(this Stage stage)
		{
			if (stage.IsAnimating || stage.Timelines.Count == 0)
				return;
			var timeline = stage.Timelines [0];
			if (timeline.Frames.Count == 0)
				return;
			stage.IsAnimating = true;
			int currentFrameIndex = 0;
			NSTimer timer = null;
			timer = NSTimer.CreateRepeatingScheduledTimer (.001, () => {
				stage.CurrentAnimationTime ++;
				if(stage.CurrentAnimationTime > timeline.Duration || currentFrameIndex >= timeline.Frames.Count){
					timer.Invalidate ();
					stage.IsAnimating = false;
				}
				if(timeline.Frames.Count <= currentFrameIndex  || !timeline.Frames[currentFrameIndex].ShouldAnimate(stage.CurrentAnimationTime))
					return;
				while(timeline.Frames.Count > currentFrameIndex && timeline.Frames[currentFrameIndex].ShouldAnimate(stage.CurrentAnimationTime))
				{
					timeline.Frames[currentFrameIndex].Animate(stage);
					currentFrameIndex ++;
				}
			});

		}

		static bool ShouldAnimate(this KeyFrame frame,int currentTime)
		{
			return frame.Position < currentTime;
		}

		public static void Animate(this KeyFrame frame, Stage stage)
		{
			Console.WriteLine (frame);
			//Console.WriteLine (NSThread.IsMain);
			if (frame.Duration == 0) {
				frame.Tween.SetState(frame.ObjectName,stage);
				return;
			}
			if (frame.Easing == EdgeEasing.EaseOut)
				UIView.SetAnimationCurve (UIViewAnimationCurve.EaseOut);
			else
				UIView.SetAnimationCurve (UIViewAnimationCurve.Linear);
			UIView.Animate (((double)frame.Duration) / 1000, () => {
				frame.Tween.SetState(frame.ObjectName,stage);
			});
		}

	}
}

