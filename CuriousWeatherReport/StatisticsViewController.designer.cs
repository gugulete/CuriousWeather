// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace CuriousWeather
{
	[Register ("StatisticsViewController")]
	partial class StatisticsViewController
	{
		[Outlet]
		MonoTouch.UIKit.UISegmentedControl seg_ChartType { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIScrollView sv_Charts { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (seg_ChartType != null) {
				seg_ChartType.Dispose ();
				seg_ChartType = null;
			}

			if (sv_Charts != null) {
				sv_Charts.Dispose ();
				sv_Charts = null;
			}
		}
	}
}
