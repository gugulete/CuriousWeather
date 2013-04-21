// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace CuriousWeather
{
	[Register ("TweetsViewController")]
	partial class TweetsViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITableView tbl_Tweets { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (tbl_Tweets != null) {
				tbl_Tweets.Dispose ();
				tbl_Tweets = null;
			}
		}
	}
}
