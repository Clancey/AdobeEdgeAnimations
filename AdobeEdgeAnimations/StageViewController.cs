using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace AdobeEdgeAnimations
{
	public class StageViewController : UIViewController
	{
		public Stage Stage { get; set; }

		public StageViewController ()
		{
		}

		public override void LoadView ()
		{
			base.LoadView ();
			LoadStage ();
		}

		public virtual async void LoadStage ()
		{
			Stage = await StageExtensions.Load ();
			View = Stage.ToUIView ();				
			Stage.Animate ();			
		}
	}
}

