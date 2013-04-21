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

      this.img_Sun_Rays.AnimationImages      = GetImages("sun_Rays_", 6);
      this.img_Sun_Rays.AnimationDuration    = 1;
      this.img_Sun_Rays.AnimationRepeatCount = 0;
      this.img_Sun_Rays.StartAnimating();

      this.img_Sun_Face.AnimationDuration    = 2;
      this.img_Sun_Face.AnimationRepeatCount = 1;
      this.sun_Face_Images.Add(new UIImage[] {
        UIImage.FromBundle("sun_Face_Eyes_1"),
        UIImage.FromBundle("sun_Face_Eyes_2"),
        UIImage.FromBundle("sun_Face_Eyes_1"),
        UIImage.FromBundle("sun_Face_Eyes_3"),
        UIImage.FromBundle("sun_Face_Eyes_1"),
      });
      this.sun_Face_Images.Add(new UIImage[] {
        UIImage.FromBundle("sun_Face_Eyes_1" ),
        UIImage.FromBundle("sun_Face_Blink_1"),
        UIImage.FromBundle("sun_Face_Eyes_1" ),
        UIImage.FromBundle("sun_Face_Eyes_1" ),
      });
      this.sun_Face_Images.Add(new UIImage[] {
        UIImage.FromBundle("sun_Face_Eyes_1" ),
        UIImage.FromBundle("sun_Face_Laugh_1"),
        UIImage.FromBundle("sun_Face_Eyes_1" ),
      });

      this.img_lbl_Temp.AnimationDuration    = 1.5;
      this.img_lbl_Temp.AnimationRepeatCount = 1;
      this.img_lbl_Temp.AnimationImages = new UIImage[] {
        UIImage.FromBundle("temp_1"),
        UIImage.FromBundle("temp_2"),
        UIImage.FromBundle("temp_3"),
        UIImage.FromBundle("temp_2"),
        UIImage.FromBundle("temp_1"),
      };

      this.img_lbl_Pressure.AnimationDuration    = 1.2;
      this.img_lbl_Pressure.AnimationRepeatCount = 1;
      this.img_lbl_Pressure.AnimationImages = new UIImage[] {
        UIImage.FromBundle("pressure_1"),
        UIImage.FromBundle("pressure_2"),
        UIImage.FromBundle("pressure_3"),
        UIImage.FromBundle("pressure_4"),
        UIImage.FromBundle("pressure_5"),
        UIImage.FromBundle("pressure_4"),
        UIImage.FromBundle("pressure_3"),
        UIImage.FromBundle("pressure_2"),
        UIImage.FromBundle("pressure_1"),
      };

      Sys.Timeout(Sys.Random.Next(5,10), () => AnimateSunFace ());
      Sys.Timeout(Sys.Random.Next(5,10), () => Animate(this.img_lbl_Pressure));
      Sys.Timeout(Sys.Random.Next(5,10), () => Animate(this.img_lbl_Temp    ));
    }

    public override void ViewWillAppear (bool animated)
    {
      base.ViewWillAppear (animated);
      this.img_Wind        .Frame = App.IsTall ? new RectangleF(110,185, 75, 85) : new RectangleF(110, 140, 75, 85);
      this.img_Sun_Rays    .Frame =              new RectangleF(  0,  0,180,180);
      this.img_Sun_Face    .Frame =              new RectangleF(  0,  0,180,180);
      this.img_lbl_Pressure.Frame = App.IsTall ? new RectangleF(  5,270,105,115) : new RectangleF(  5,215,105,115);
      this.img_lbl_Temp    .Frame =              new RectangleF(190,  5,128,110);
      this.img_lbl_Wind    .Frame = App.IsTall ? new RectangleF(180,170,140, 60) : new RectangleF(180,130,140, 60);
      this.lbl_Pressure    .Frame = App.IsTall ? new RectangleF( 15,345, 90, 30) : new RectangleF( 15,285, 90, 30);
      this.lbl_TempHigh    .Frame =              new RectangleF(230, 20, 80, 20);
      this.lbl_TempLow     .Frame =              new RectangleF(230, 50, 80, 20);
      this.lbl_Wind        .Frame = App.IsTall ? new RectangleF(235,190, 75, 20) : new RectangleF(235,150, 75, 20);
    }

    private List<UIImage[]> sun_Face_Images = new List<UIImage[]>();
    private void AnimateSunFace()
    {
      this.img_Sun_Face.AnimationImages = sun_Face_Images[Sys.Random.Next(sun_Face_Images.Count)];
      Sys.Timeout(Sys.Random.Next(5,10), () => AnimateSunFace());
      this.img_Sun_Face.StartAnimating();
    }

    private void Animate(UIImageView _imgView)
    {
      _imgView.StartAnimating();
      Sys.Timeout(Sys.Random.Next(5,10), () => Animate(_imgView));
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

