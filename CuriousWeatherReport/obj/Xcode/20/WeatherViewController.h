// WARNING
// This file has been generated automatically by Xamarin Studio to
// mirror C# types. Changes in this file made by drag-connecting
// from the UI designer will be synchronized back to C#, but
// more complex manual changes may not transfer correctly.


#import <UIKit/UIKit.h>
#import <MapKit/MapKit.h>
#import <Foundation/Foundation.h>
#import <CoreGraphics/CoreGraphics.h>


@interface WeatherViewController : UIViewController {
	UILabel *_lbl_TempHigh;
	UILabel *_lbl_TempLow;
	UILabel *_lbl_Wind;
	UILabel *_lbl_Pressure;
	UIImageView *_img_Wind;
	UIImageView *_img_Sun_Face;
	UIImageView *_img_Sun_Rays;
	UIImageView *_img_lbl_Pressure;
	UIImageView *_img_lbl_Wind;
	UIImageView *_img_lbl_Temp;
}

@property (nonatomic, retain) IBOutlet UILabel *lbl_TempHigh;

@property (nonatomic, retain) IBOutlet UILabel *lbl_TempLow;

@property (nonatomic, retain) IBOutlet UILabel *lbl_Wind;

@property (nonatomic, retain) IBOutlet UILabel *lbl_Pressure;

@property (nonatomic, retain) IBOutlet UIImageView *img_Wind;

@property (nonatomic, retain) IBOutlet UIImageView *img_Sun_Face;

@property (nonatomic, retain) IBOutlet UIImageView *img_Sun_Rays;

@property (nonatomic, retain) IBOutlet UIImageView *img_lbl_Pressure;

@property (nonatomic, retain) IBOutlet UIImageView *img_lbl_Wind;

@property (nonatomic, retain) IBOutlet UIImageView *img_lbl_Temp;

@end
