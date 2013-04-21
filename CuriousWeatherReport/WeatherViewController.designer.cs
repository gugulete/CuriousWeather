// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace CuriousWeather
{
	[Register ("WeatherViewController")]
	partial class WeatherViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel lbl_TempHigh { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lbl_TempLow { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lbl_Wind { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lbl_Pressure { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lbl_TempHigh != null) {
				lbl_TempHigh.Dispose ();
				lbl_TempHigh = null;
			}

			if (lbl_TempLow != null) {
				lbl_TempLow.Dispose ();
				lbl_TempLow = null;
			}

			if (lbl_Wind != null) {
				lbl_Wind.Dispose ();
				lbl_Wind = null;
			}

			if (lbl_Pressure != null) {
				lbl_Pressure.Dispose ();
				lbl_Pressure = null;
			}
		}
	}
}
