using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using BarChart;
using System.Collections.Generic;
using System.Linq;

namespace CuriousWeather
{
  public partial class StatisticsViewController : UIViewController
  {
    BarChartView masterChart;
    BarChartView detailsChart;

    public StatisticsViewController () : base ("StatisticsViewController", null)
    {
      Title = "Statistics";
    }
    
    public override void DidReceiveMemoryWarning ()
    {
      base.DidReceiveMemoryWarning ();
    }
    
    public override void ViewDidLoad ()
    {
      base.ViewDidLoad ();
      seg_ChartType.SetBackgroundImage(UIImage.FromBundle("statistics_SegmentedBg" ), UIControlState.Normal     , UIBarMetrics.Default);
      seg_ChartType.SetBackgroundImage(UIImage.FromBundle("statistics_SegmentedHgh"), UIControlState.Selected   , UIBarMetrics.Default);
      seg_ChartType.SetDividerImage   (UIImage.FromBundle("seg_divide"   ), UIControlState.Highlighted | UIControlState.Normal, UIControlState.Highlighted | UIControlState.Normal, UIBarMetrics.Default); 
      seg_ChartType.ValueChanged += (s,e) => UpdateChart1Data();

      masterChart = new BarChartView ();
      masterChart.BarOffset = 2f;
      masterChart.BarWidth = 40f;
      masterChart.BarCaptionInnerColor       = UIColor.White;
      masterChart.BarCaptionInnerShadowColor = UIColor.Black;
      masterChart.BarCaptionOuterColor       = UIColor.White;
      masterChart.BarCaptionOuterShadowColor = UIColor.Black;

      masterChart.BarClick += OnBarClick;
      masterChart.Frame = new RectangleF (-40, 70, View.Bounds.Width + 40, (App.ViewHeight - 70)/2);
      masterChart.LevelsHidden = true;
      
      this.Add(masterChart);

      detailsChart = new BarChartView();
      detailsChart.BarOffset = 2f;
      detailsChart.BarWidth = 40f;
      detailsChart.BarCaptionInnerColor       = UIColor.White;
      detailsChart.BarCaptionInnerShadowColor = UIColor.Black;
      detailsChart.BarCaptionOuterColor       = UIColor.White;
      detailsChart.BarCaptionOuterShadowColor = UIColor.Black;
      detailsChart.Frame = new RectangleF (-40, masterChart.Frame.Y + masterChart.Frame.Height + 10, View.Bounds.Width + 40, (App.ViewHeight - 70)/2 - 10);
      detailsChart.LevelsHidden = true;

      this.Add(detailsChart);
      if (App.WeatherInfos.Any()) {
        UpdateChart1Data ();
      } else {
        App.ReloadData();
        App.DataLoaded += HandleDataLoaded;
      }
    }

    void HandleDataLoaded (object sender, BoolEventArgs e)
    {
      if (e.Value) {
        this.InvokeOnMainThread(() => {
          UpdateChart1Data();
          App.DataLoaded -= HandleDataLoaded;
        });
      }
    }

    private void UpdateChart1Data()
    {
      if (!App.WeatherInfos.Any()) {
        App.ReloadData(); 
        return; // -->
      }

      List<BarModel> data = null;
      switch (seg_ChartType.SelectedSegment) {
      case 0 : data = App.WeatherInfos.OrderBy(wi => wi.Date).GroupBy(wi => wi.Grouping).Select(g => new BarModel() { Value = (float)g.Average(wi => wi.LowTemp ), Legend = g.Key, ValueCaption = g.Average(wi => wi.LowTemp ).ToString("0.0" ) }).ToList(); break;
      case 1 : data = App.WeatherInfos.OrderBy(wi => wi.Date).GroupBy(wi => wi.Grouping).Select(g => new BarModel() { Value = (float)g.Average(wi => wi.HighTemp), Legend = g.Key, ValueCaption = g.Average(wi => wi.HighTemp).ToString("0.0" ) }).ToList(); break;
      case 2 : data = App.WeatherInfos.OrderBy(wi => wi.Date).GroupBy(wi => wi.Grouping).Select(g => new BarModel() { Value = (float)g.Average(wi => wi.Pressure), Legend = g.Key, ValueCaption = g.Average(wi => wi.Pressure).ToString("0.00") }).ToList(); break;
      }

      UpdateColor(data);
      masterChart.MinimumValue = data.Min(bm => bm.Value) > 0 ? (float)Math.Floor  (data.Min(bm => bm.Value) * 0.8) : (float)Math.Floor  (data.Min(bm => bm.Value));
      masterChart.MaximumValue = data.Max(bm => bm.Value) > 0 ? (float)Math.Ceiling(data.Max(bm => bm.Value)      ) : (float)Math.Ceiling(data.Max(bm => bm.Value) * 0.9);
      masterChart.ItemsSource = data;
      UpdateChart2Data(masterChart.ItemsSource.First().Legend);
    }

    private void UpdateChart2Data(string _date)
    {
      if (!App.WeatherInfos.Any()) {
        App.ReloadData(); 
        return; // -->
      }

      List<BarModel> data = null;
      switch (seg_ChartType.SelectedSegment) {
      case 0 : data = App.WeatherInfos.Where(wi => wi.Grouping == _date).OrderBy(wi => wi.Date).Select(wi => new BarModel() { Value = (float)wi.LowTemp , Legend = wi.Date.ToString("dd"), ValueCaption = wi.LowTemp .ToString("0.0") }).ToList(); break;
      case 1 : data = App.WeatherInfos.Where(wi => wi.Grouping == _date).OrderBy(wi => wi.Date).Select(wi => new BarModel() { Value = (float)wi.HighTemp, Legend = wi.Date.ToString("dd"), ValueCaption = wi.HighTemp.ToString("0.0") }).ToList(); break;
      case 2 : data = App.WeatherInfos.Where(wi => wi.Grouping == _date).OrderBy(wi => wi.Date).Select(wi => new BarModel() { Value = (float)wi.Pressure, Legend = wi.Date.ToString("dd"), ValueCaption = wi.Pressure.ToString("0.00") }).ToList(); break;
      }
      UpdateColor(data);
      detailsChart.MinimumValue = data.Min(bm => bm.Value) > 0 ? (float)Math.Floor  (data.Min(bm => bm.Value) * 0.8) : (float)Math.Floor  (data.Min(bm => bm.Value));
      detailsChart.MaximumValue = data.Max(bm => bm.Value) > 0 ? (float)Math.Ceiling(data.Max(bm => bm.Value)      ) : (float)Math.Ceiling(data.Max(bm => bm.Value) * 0.9);
      detailsChart.ItemsSource = data;
    }

    private UIColor GetColorTempLow(float _min, float _max, float _value)
    {
      return UIColor.FromHSB(((maxHlow - minHlow) * (_max + _min - _value) + _max * minHlow - _min * maxHlow)/(360*(_max - _min)), 0.87f, 0.95f);
    }

    private readonly int minHlow = 120;
    private readonly int maxHlow = 210;
    private readonly int minHpre = 220;
    private readonly int maxHpre = 360;
    private readonly int minHhgh = 0;
    private readonly int maxHhgh = 120;

    private UIColor GetColorTempHigh(float _min, float _max, float _value)
    {
      return UIColor.FromHSB(((maxHhgh - minHhgh) * (_max + _min - _value) + _max * minHhgh - _min * maxHhgh)/(360*(_max - _min)), 0.87f, 0.95f);
    }

    private UIColor GetColorPressure(float _min, float _max, float _value)
    {
      return UIColor.FromHSB(((maxHpre - minHpre) * _value + _max * minHpre - _min * maxHpre)/(360*(_max - _min)), 0.87f, 0.95f);
    }

    private void UpdateColor(IEnumerable<BarModel> _items)
    {
      var min = _items.Min(i => i.Value);
      var max = _items.Max(i => i.Value);
      foreach (var item in _items) {
        switch(seg_ChartType.SelectedSegment) {
        case 0 : item.Color = GetColorTempLow (min, max, item.Value); break;
        case 1 : item.Color = GetColorTempHigh(min, max, item.Value); break;
        case 2 : item.Color = GetColorPressure(min, max, item.Value); break;
        }
      }
    }

    void OnBarClick (object sender, BarClickEventArgs e)
    {
      UpdateChart2Data(e.Bar.Legend);
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

