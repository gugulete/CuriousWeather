using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using BigTed;

namespace CuriousWeather
{
  // The UIApplicationDelegate for the application. This class is responsible for launching the 
  // User Interface of the application, as well as listening (and optionally responding) to 
  // application events from iOS.
  [Register ("AppDelegate")]
  public partial class AppDelegate : UIApplicationDelegate
  {
    // class-level declarations
    UIWindow window;
    UITabBarController tabBarController;

    //
    // This method is invoked when the application has loaded and is ready to run. In this 
    // method you should instantiate the window, load the UI into it and then make the window
    // visible.
    //
    // You have 17 seconds to return from this method, or iOS will terminate your application.
    //
    public override bool FinishedLaunching (UIApplication app, NSDictionary options)
    {
      // create a new window instance based on the screen size
      window = new UIWindow (UIScreen.MainScreen.Bounds);
      
      var weatherController    = new WeatherViewController();
      var tweetsController     = new TweetsViewController ();
      var statisticsController = new StatisticsViewController();
      tabBarController = new UITabBarController ();

      tabBarController.TabBar.BackgroundImage         = UIImage.FromBundle("tabBar_background");
      tabBarController.TabBar.SelectionIndicatorImage = UIImage.FromBundle("tabBar_highlight" );

      tabBarController.ViewControllers = new UIViewController [] {
        weatherController   ,
        tweetsController    ,
        statisticsController,
      };

      tabBarController.TabBar.Items[0].SetFinishedImages(UIImage.FromBundle("weather"   ), UIImage.FromBundle("weather"   ));
      tabBarController.TabBar.Items[1].SetFinishedImages(UIImage.FromBundle("tweets"    ), UIImage.FromBundle("tweets"    ));
      tabBarController.TabBar.Items[2].SetFinishedImages(UIImage.FromBundle("statistics"), UIImage.FromBundle("statistics"));

      window.RootViewController = tabBarController;
      window.MakeKeyAndVisible ();

      App.ReloadData();

      UILabel.Appearance.Font = App.GetFont(14);
      UISegmentedControl.Appearance.SetTitleTextAttributes(new UITextAttributes() { Font = App.GetFont(14) }, UIControlState.Normal);
      UITabBarItem      .Appearance.SetTitleTextAttributes(new UITextAttributes() { Font = App.GetFont(11) }, UIControlState.Normal);

      return true;
    }
  }
}

