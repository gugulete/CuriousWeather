using System;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Globalization;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using BigTed;

namespace CuriousWeather
{
  public class App
  {
    public static List<WeatherInfo> WeatherInfos = new List<WeatherInfo>();
    public static List<Tweet      > Tweets       = new List<Tweet      >();
    public static UIImage           ProfilePic;
    public static event EventHandler                DataLoading;
    public static event EventHandler<BoolEventArgs> DataLoaded;


    public static void ReloadData()
    {
      if (DataLoading != null) DataLoading(null, null);

      BTProgressHUD.Show("Loading data", -1, BTProgressHUD.MaskType.Black);

      try {
        var client = new RestClient("http://api.twitter.com/1/");
        var request = new RestRequest("statuses/user_timeline.json?screen_name={name}&count=200", Method.GET);
        request.AddUrlSegment("name", "MarsWxReport");
        client.ExecuteAsync(request, response => {
          var arr = JArray.Parse(response.Content);
          foreach (var item in arr) {
            if (ProfilePic == null) LoadImage(item["user"]["profile_image_url"].Value<string>());
            ReadTweet(item["text"].Value<string>(), item["id_str"].Value<string>(), DateTime.ParseExact(item["created_at"].Value<string>(), "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture));
          }

          if (DataLoaded != null) DataLoaded(null, new BoolEventArgs(true));
          BTProgressHUD.ShowSuccessWithStatus("Data loaded!");
          Sys.Timeout(1, () => BTProgressHUD.Dismiss());
        });
      } catch {
        if (DataLoaded != null) DataLoaded(null, new BoolEventArgs(false));
        BTProgressHUD.ShowErrorWithStatus  ("Something went wrong :(\nTry again after a while!");
        Sys.Timeout(1, () => BTProgressHUD.Dismiss());
      }
    }

    private static void LoadImage(string _url)
    {
      NSUrl nsUrl = new NSUrl(_url);
      NSData data = NSData.FromUrl(nsUrl);
      ProfilePic = new UIImage(data); 
    }

    private static NumberStyles parseStyle {
      get {
        return NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite | NumberStyles.AllowThousands | NumberStyles.AllowTrailingWhite;
      }
    }

    private static void ReadTweet(string _text, string _id, DateTime _date)
    {
      if (_text.StartsWith("Sol")) {
        int beg, end;

        var wi = new WeatherInfo();
        beg = _text.IndexOf("(") + 1;
        end = _text.IndexOf(")", beg);
        if (beg > 0 && end > 0) {
          DateTime _temp = DateTime.MinValue;
          if (DateTime.TryParse(_text.Substring(beg, end - beg), out _temp)) wi.Date = _temp;
        }

        beg = _text.IndexOf("): ", beg) + 3;
        end = _text.IndexOf(","  , beg);
        if (beg > 2 && end > 2) wi.Explanation = _text.Substring(beg,end - beg);

        beg = _text.IndexOf("high ", beg) + 5;
        end = _text.IndexOf("C/"   , beg);
        if (beg > 4 && end > 4) wi.HighTempC = double.Parse(_text.Substring(beg, end - beg).Replace(',','.'), parseStyle, CultureInfo.InvariantCulture);

        beg = _text.IndexOf("C/", beg) + 2;
        end = _text.IndexOf("F" , beg);
        if (beg > 1 && end > 1) wi.HighTempF = double.Parse(_text.Substring(beg, end - beg).Replace(',','.'), parseStyle, CultureInfo.InvariantCulture);

        beg = _text.IndexOf("low " , beg) + 4;
        end = _text.IndexOf("C/"   , beg);
        if (beg > 3 && end > 3) wi.LowTempC  = double.Parse(_text.Substring(beg, end - beg).Replace(',','.'), parseStyle, CultureInfo.InvariantCulture);
        
        beg = _text.IndexOf("C/", beg) + 2;
        end = _text.IndexOf("F" , beg);
        if (beg > 1 && end > 1) wi.LowTempF  = double.Parse(_text.Substring(beg, end - beg).Replace(',','.'), parseStyle, CultureInfo.InvariantCulture);

        if (wi.HighTemp < wi.LowTemp) {
          var temp = wi.HighTempC;
          wi.HighTempC = wi.LowTempC;
          wi.LowTempC  = temp;

          temp = wi.HighTempF;
          wi.HighTempF = wi.LowTempF;
          wi.LowTempF  = temp;
        }

        beg = _text.IndexOf("at ", beg) + 3;
        end = _text.IndexOf("hPa", beg);
        if (beg > 2 && end > 2) wi.Pressure  = double.Parse(_text.Substring(beg, end - beg).Replace(',','.'), parseStyle, CultureInfo.InvariantCulture);

        beg = _text.IndexOf("wind ", beg) + 5;
        end = _text.IndexOf(" at " , beg);
        if (beg > 4 && end > 4) wi.WindDir = (WindDirection)Enum.Parse(typeof(WindDirection), _text.Substring(beg, end - beg));

        beg = _text.IndexOf(" at ", beg) + 4;
        end = _text.IndexOf("kmh" , beg);
        if (beg > 3 && end > 3) wi.WindSpeed = double.Parse(_text.Substring(beg, end - beg).Replace(',','.'), parseStyle, CultureInfo.InvariantCulture);

        if (wi.Date > Date_Min && wi.Pressure > 0) WeatherInfos.Add(wi);
      } else {
        Tweets.Add(new Tweet() { Text = _text, Date = _date, URL = "https://twitter.com/MarsWxReport/status/" + _id });
      }
    }

    public static readonly DateTime Date_Min = new DateTime(2012,9,1);

    public static bool IsCelsius = true; //{ get; set; }

    private static UIFont _FontForTweetBody = null;
    public  static UIFont FontForTweetBody
    {
      get {
        if (_FontForTweetBody == null) {
          _FontForTweetBody = UIFont.FromName("Noteworthy-Bold", 14);
        }
        return _FontForTweetBody;
      }
    }

    private static UIFont _FontForTweetDate = null;
    public  static UIFont FontForTweetDate
    {
      get {
        if (_FontForTweetDate == null) {
          _FontForTweetDate = UIFont.FromName("Noteworthy-Bold", 11);
        }
        return _FontForTweetDate;
      }
    }

    private static UIFont _FontForTweetHeader = null;
    public  static UIFont FontForTweetHeader
    {
      get {
        if (_FontForTweetHeader == null) {
          _FontForTweetHeader = UIFont.FromName("Noteworthy-Bold", 18);
        }
        return _FontForTweetHeader;
      }
    }

    public static UIFont GetFont(int _size)
    {
      return UIFont.FromName("Noteworthy-Bold", _size);
    }

    public static float ViewHeight {
      get {
        return UIScreen.MainScreen.Bounds.Height - 69f;
      }
    }
  }

  public class BoolEventArgs : EventArgs
  {
    public bool Value { get; set; }
    public BoolEventArgs(bool _value) : base()
    {
      Value = _value;
    }
  }

  public class WeatherInfo
  {
    public string        Explanation { get; set; }
    public double        HighTempC   { get; set; }
    public double        HighTempF   { get; set; }
    public double        HighTemp    { get { return App.IsCelsius ? HighTempC : HighTempF; } }
    public double        LowTempC    { get; set; }
    public double        LowTempF    { get; set; }
    public double        LowTemp     { get { return App.IsCelsius ? LowTempC : LowTempF; } }
    public double        Pressure    { get; set; }
    public WindDirection WindDir     { get; set; }
    public double        WindSpeed   { get; set; }
    public DateTime      Date        { get; set; }
    public string        Grouping    { get { return this.Date.ToString("MM/yy"); } }
  }

  public class Tweet
  {
    public string   Text   { get; set; }
    public DateTime Date   { get; set; }
    public string   URL    { get; set; }
  }

  public enum WindDirection
  {
    N, S, E, W
  }

  public static class Sys
  {
    public static void Timeout(double _seconds, Action _action)
    {
      NSTimer.CreateScheduledTimer(_seconds, () => { 
        if (_action != null) _action();
      });
    }
  }

  public static class ExtensionMethods
  {
    public static SizeF GetSize(this string _text, UIFont _font, SizeF? _bounds = null, UILineBreakMode _break = UILineBreakMode.WordWrap)
    {
      return _text.ToNSString().StringSize(_font, _bounds ?? new SizeF(300, 411), _break);
    }

    public static NSString ToNSString(this string item)
    {
      return NSString.FromData(item, NSStringEncoding.UTF8);  
    }
  }
}

