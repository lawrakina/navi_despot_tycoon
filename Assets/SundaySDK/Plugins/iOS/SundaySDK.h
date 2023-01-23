#import <Foundation/Foundation.h>


@interface SundaySDK : NSObject
+ (SundaySDK *)sharedInstance;
- (void)requestIDFA;
- (NSUInteger)getTrackingAuthorizationStatus;
@end


