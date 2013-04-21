using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Linq;

namespace CuriousWeather
{
  public partial class WeatherViewController : UIViewController
  {
    public WeatherViewController () : base ("WeatherViewController", null)
    {
      Title = "Weather";
    }
    
    public override void DidReceiveMemoryWarning ()
    {
      base.DidReceiveMemoryWarning ();
    }
    
    public override void ViewDidLoad ()
    {
      base.ViewDidLoad ();

      if (!App.WeatherInfos.Any()) {
        App.DataLoaded += (s,e) => {
          this.InvokeOnMainThread(() => {
            var wi = App.WeatherInfos.FirstOrDefault();
            if (wi != null) {
              this.lbl_TempHigh.Text = wi.HighTemp .ToString("0.0" ) + " °C";
              this.lbl_TempLow .Text = wi.LowTemp  .ToString("0.0" ) + " °C";
              this.lbl_Pressure.Text = wi.Pressure .ToString("0.00") + " hPa";
              this.lbl_Wind    .Text = wi.WindSpeed.ToString("0.0" ) + " km/h";
            }
          });
        };
      }
    }

    public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
    {
      return UIInterfaceOrientationMask.Portrait;
    }

    public override bool ShouldAutorotate ()
    {
      return false;
    }   
  }
}

