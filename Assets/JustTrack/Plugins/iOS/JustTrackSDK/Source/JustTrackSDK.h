#import <Foundation/Foundation.h>

//! Project version number for JustTrackSDK.
FOUNDATION_EXPORT double JustTrackSDKVersionNumber;

//! Project version string for JustTrackSDK.
FOUNDATION_EXPORT const unsigned char JustTrackSDKVersionString[];

@interface JustTrackObjCBridge : NSObject
+ (void) initIronSourceIntegration:(NSString * _Nonnull) userId;
@end
