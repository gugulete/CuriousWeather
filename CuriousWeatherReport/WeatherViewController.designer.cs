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

		[Outlet]
		MonoTouch.UIKit.UIImageView img_Wind { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView img_Sun_Face { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView img_Sun_Rays { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView img_lbl_Pressure { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView img_lbl_Wind { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView img_lbl_Temp { get; set; }
		
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

			if (img_Wind != null) {
				img_Wind.Dispose ();
				img_Wind = null;
			}

			if (img_Sun_Face != null) {
				img_Sun_Face.Dispose ();
				img_Sun_Face = null;
			}

			if (img_Sun_Rays != null) {
				img_Sun_Rays.Dispose ();
				img_Sun_Rays = null;
			}

			if (img_lbl_Pressure != null) {
				img_lbl_Pressure.Dispose ();
				img_lbl_Pressure = null;
			}

			if (img_lbl_Wind != null) {
				img_lbl_Wind.Dispose ();
				img_lbl_Wind = null;
			}

			if (img_lbl_Temp != null) {
				img_lbl_Temp.Dispose ();
				img_lbl_Temp = null;
			}
		}
	}
}
