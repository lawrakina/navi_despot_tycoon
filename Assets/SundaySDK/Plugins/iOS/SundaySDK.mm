#include "SundaySDK.h"
#import <AppTrackingTransparency/AppTrackingTransparency.h>
#import <AdSupport/AdSupport.h>

@implementation SundaySDK

+ (SundaySDK *)sharedInstance {
    static SundaySDK *instance = nil;
    static dispatch_once_t token;

    dispatch_once(&token, ^{
      instance = [[SundaySDK alloc] init];
    });
    return instance;
}

- (void)requestIDFA {
    if (@available(iOS 14, *)) {
        [ATTrackingManager requestTrackingAuthorizationWithCompletionHandler:^(ATTrackingManagerAuthorizationStatus status) {
            // Tracking authorization completed. Start loading ads here.
            // [self loadAd];
            NSLog(@"Tracking Authorization Request Callback");
        }];
    } else {
        // Fallback on earlier versions
    }
}

- (NSUInteger)getTrackingAuthorizationStatus {
    if (@available(iOS 14, *)) {
        ATTrackingManagerAuthorizationStatus value = [ATTrackingManager trackingAuthorizationStatus];
        return value;
    } else {
        return 0;
    }
}

- (NSString *)getIDFA {
    return [[[ASIdentifierManager sharedManager] advertisingIdentifier] UUIDString];
}

@end

char* convertNSStringToCString(const NSString* nsString)
{
    if (nsString == NULL)
        return NULL;

    const char* nsStringUtf8 = [nsString UTF8String];
    //create a null terminated C string on the heap so that our string's memory isn't wiped out right after method's return
    char* cString = (char*)malloc(strlen(nsStringUtf8) + 1);
    strcpy(cString, nsStringUtf8);

    return cString;
}

extern "C" {
    typedef void (* CallbackFunc)(int number);

    void TrackingAuthorizationRequest() {
        NSLog(@"Tracking Authorization Request");
        [[SundaySDK sharedInstance] requestIDFA];
    }

    int GetTrackingStatus(){
        return (int) [[SundaySDK sharedInstance] getTrackingAuthorizationStatus];
    }

    char* GetIDFA(){
        return convertNSStringToCString([[SundaySDK sharedInstance] getIDFA]);
    }
}
