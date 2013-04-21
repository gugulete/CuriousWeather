using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.CoreText;

namespace CuriousWeather
{
  public partial class TweetsViewController : UIViewController
  {
    public TweetsViewController () : base ("TweetsViewController", null)
    {
      Title = "Tweets";
      TabBarItem.Image = UIImage.FromBundle ("tweets");
    }
    
    public override void DidReceiveMemoryWarning ()
    {
      base.DidReceiveMemoryWarning ();
    }
    
    public override void ViewDidLoad ()
    {
      base.ViewDidLoad ();

      if (App.Tweets.Any()) {
        this.tbl_Tweets.Delegate       = new TweetsTableViewDelegate  (App.Tweets);
        this.tbl_Tweets.DataSource     = new TweetsTableViewDatasource(App.Tweets);
      } else {
        App.ReloadData();
        App.DataLoaded += HandleDataLoaded;
      }
      this.tbl_Tweets.SeparatorStyle = UITableViewCellSeparatorStyle.None;
    }

    void HandleDataLoaded (object sender, BoolEventArgs e)
    {
      if (e.Value) {
        this.InvokeOnMainThread(() => {
          this.tbl_Tweets.Delegate       = new TweetsTableViewDelegate  (App.Tweets);
          this.tbl_Tweets.DataSource     = new TweetsTableViewDatasource(App.Tweets);
          App.DataLoaded -= HandleDataLoaded;
        });
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

  public class TweetsTableViewDelegate : UITableViewDelegate
  {
    private Dictionary<string, Tweet[]> tweets = new Dictionary<string, Tweet[]>();

    public TweetsTableViewDelegate(IEnumerable<Tweet> _tweets)
    {
      var sections = _tweets.Select(g => new { g.Date.Year, g.Date.Month }).Distinct().ToArray();
      foreach (var section in sections) {
        tweets.Add(new DateTime(section.Year, section.Month, 1).ToString("yyyy MMMM"), _tweets.Where(t => t.Date.Year == section.Year && t.Date.Month == section.Month).ToArray());
      }
    }

    public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
    {
      tableView.DeselectRow(indexPath, true);
      var tweet = tweets.ElementAt(indexPath.Section).Value[indexPath.Row];
      UIApplication.SharedApplication.OpenUrl(new NSUrl(tweet.URL));
    }

    public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
    {
      var tweet = tweets.ElementAt(indexPath.Section).Value[indexPath.Row];
      
      return tweet.Text.GetSize(App.FontForTweetBody, new SizeF(235f, 300f)).Height + 50f;
    }

    public override UIView GetViewForHeader (UITableView tableView, int section)
    {
      var title = tweets.ElementAt(section).Key;
      var bg = new UIView(new RectangleF(0,0,320,40));
      bg.BackgroundColor  =  UIColor.Clear;
      //var bg2 = new UIView(new Rectangle(0,0,320,30));
      //bg2.BackgroundColor = UIColor.FromRGBA(108/255f,195/255f,22/255f,0.6f);
      var bg2 = new UIImageView(new Rectangle(0,0,320,30));
      bg2.Image = UIImage.FromBundle("tweet_Header");

      var lbl_Header             = new UILabel(new RectangleF(10, 0, 300, 30));
      lbl_Header.Text            = title;
      lbl_Header.TextAlignment   = UITextAlignment.Left;
      lbl_Header.BackgroundColor = UIColor.Clear;
      lbl_Header.TextColor       = UIColor.White;
      lbl_Header.Font            = App.FontForTweetHeader;

      bg2.Add(lbl_Header);
      bg.Add(bg2);
      return bg;
    }

    public override float GetHeightForHeader (UITableView tableView, int section)
    {
      return 40;
    }
  }

  public class TweetsTableViewDatasource : UITableViewDataSource
  {
    static  NSString kCellIdentifier = new NSString("TweetIdentifier");
    private Dictionary<string, Tweet[]> tweets = new Dictionary<string, Tweet[]>();

    public TweetsTableViewDatasource(IEnumerable<Tweet> _tweets)
    {
      var sections = _tweets.Select(g => new { g.Date.Year, g.Date.Month }).Distinct().ToArray();
      foreach (var section in sections) {
        tweets.Add(new DateTime(section.Year, section.Month, 1).ToString("yyyy MMM"), _tweets.Where(t => t.Date.Year == section.Year && t.Date.Month == section.Month).ToArray());
      }
    }

    public override int NumberOfSections (UITableView tableView)
    {
      return tweets.Count;
    }

    public override string TitleForHeader (UITableView tableView, int section)
    {
      return tweets.ElementAt(section).Key;
    }
    
    public override int RowsInSection (UITableView tableView, int section)
    {
      return tweets.ElementAt(section).Value.Length;
    }

    public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
    {
      UITableViewCell cell = tableView.DequeueReusableCell(kCellIdentifier);
      if (cell == null) {
        cell = new UITableViewCell(UITableViewCellStyle.Default, kCellIdentifier);
      }

      cell.BackgroundColor           = UIColor.Clear;
      cell.BackgroundView            = null;

      var tweet = tweets.ElementAt(indexPath.Section).Value[indexPath.Row];
      var size  = tweet.Text.GetSize(App.FontForTweetBody, new SizeF(235, 400));

      UIImageView bg = cell.Subviews.FirstOrDefault(s => s.Tag == 1) as UIImageView;
      if (bg == null) {
        bg = new UIImageView();
        bg.Image = UIImage.FromBundle("tweet_Cell");
        bg.ContentMode = UIViewContentMode.ScaleToFill;
        bg.Tag   = 1;
        cell.Add(bg);
      }
      bg.Frame = new RectangleF(10, 0, 300, size.Height + 40);

      UILabel lbl_Text = bg.Subviews.FirstOrDefault(s => s.Tag == 2) as UILabel;
      if (lbl_Text == null) {
        lbl_Text = new UILabel();
        lbl_Text.Font            = App.FontForTweetBody;
        lbl_Text.BackgroundColor = UIColor.Clear;
        lbl_Text.TextColor       = UIColor.White;
        lbl_Text.LineBreakMode   = UILineBreakMode.WordWrap;
        lbl_Text.Lines           = 0;
        lbl_Text.Tag             = 2;
        bg.Add(lbl_Text);
      }
      lbl_Text.Frame = new RectangleF(60, 10, 235, size.Height);
      lbl_Text.Text  = tweet.Text;

      UIImageView img = bg.Subviews.FirstOrDefault(s => s.Tag == 3) as UIImageView;
      if (img == null) {
        img             = new UIImageView(new RectangleF(10, 15, 45, 45));
        img.Image       = App.ProfilePic;
        img.ContentMode = UIViewContentMode.ScaleAspectFit;
        img.Tag         = 3;
        bg.Add(img);
      }

      UILabel lbl_Date = bg.Subviews.FirstOrDefault(s => s.Tag == 4) as UILabel;
      if (lbl_Date == null) {
        lbl_Date                 = new UILabel();
        lbl_Date.Font            = App.FontForTweetDate;
        lbl_Date.BackgroundColor = UIColor.Clear;
        lbl_Date.TextColor       = UIColor.FromRGBA(220/255f,95/255f,20/255f,1);
        lbl_Date.Tag             = 4;
        bg.Add(lbl_Date);
      }
      lbl_Date.Frame = new RectangleF(180, lbl_Text.Frame.Height + 15, 120, 15);
      lbl_Date.Text  = tweet.Date.ToString("MMM dd, yyyy HH:mm");

      return cell;
    }
  }
}