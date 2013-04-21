using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Linq;
using System.Collections.Generic;

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

      this.img_Wind.AnimationImages      = GetImages("wind_", 4);
      this.img_Wind.AnimationDuration    = 0.4;
      this.img_Wind.AnimationRepeatCount = 0;
      this.img_Wind.StartAnimating();

      this.img_Sun_Rays.AnimationImages      = GetImages("sun_Rays_", 3);
      this.img_Sun_Rays.AnimationDuration    = 1.2;
      this.img_Sun_Rays.AnimationRepeatCount = 0;
      this.img_Sun_Rays.StartAnimating();

      this.img_Sun_Face.AnimationDuration    = 2;
      this.img_Sun_Face.AnimationRepeatCount = 1;
      this.sun_Face_Images.Add(GetImages("sun_Face_Eyes_", 5));

      Sys.Timeout(5, () => {
        RefreshSunFace();
      });
    }

    public override void ViewWillAppear (bool animated)
    {
      base.ViewWillAppear (animated);
      this.img_Wind    .Frame = App.IsTall ? new RectangleF(110, 185, 75, 85) : new RectangleF(110, 140, 75, 85);
      this.img_Sun_Rays.Frame = new RectangleF(0,0,180,180);
      this.img_Sun_Face.Frame = new RectangleF(0,0,180,180);
    }

    private List<UIImage[]> sun_Face_Images = new List<UIImage[]>();
    private void RefreshSunFace()
    {
      this.img_Sun_Face.AnimationImages = sun_Face_Images[Sys.Random.Next(sun_Face_Images.Count)];
      Sys.Timeout(Sys.Random.Next(5,10), () => {
        RefreshSunFace();
      });
      this.img_Sun_Face.StartAnimating();
    }

    private UIImage[] GetImages(string _prefix, int _count)
    {
      var imgs = new List<UIImage>();
      for (int i = 1; i <= _count; i++) {
        imgs.Add(UIImage.FromBundle(_prefix + i));
      }
      return imgs.ToArray();
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

