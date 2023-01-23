#include "JustTrackSDK.h"
#include "JustTrackSDK-Swift.h"

#pragma mark - IronSource integration

#if defined(ENABLE_IRONSOURCE_INTEGRATION) && ENABLE_IRONSOURCE_INTEGRATION

#include "IronSource.h"
#include "JTISImpressionDataDelegate.h"

@implementation JustTrackObjCBridge
+ (void) initIronSourceIntegration:(NSString * _Nonnull) userId {
    [IronSource setUserId:userId];
    JustTrackImpressionDataDelegate *delegate = [JustTrackImpressionDataDelegate alloc];
    [IronSource setImpressionDataDelegate:delegate];
}
@end

#else

@implementation JustTrackObjCBridge
+ (void) initIronSourceIntegration:(NSString * _Nonnull) userId {
    // nop
}
@end

#endif

#pragma mark - C interface

extern "C" {
    void _justtrack_sdk_rp_init(const char *apiToken, const char *trackingId, const char *trackingProvider, int inactivityTimeFrameHours, int reAttributionTimeFrameDays, int reFetchAttributionDelaySeconds) {
        NSString *nsApiToken = [NSString stringWithUTF8String:apiToken];
        NSString *nsTrackingId = [NSString stringWithUTF8String:trackingId];
        NSString *nsTrackingProvider = [NSString stringWithUTF8String:trackingProvider];
        [[NativeBridge shared] initSdkWithApiToken: nsApiToken
                                        trackingId: nsTrackingId
                                  trackingProvider: nsTrackingProvider
                          inactivityTimeFrameHours: (NSInteger) inactivityTimeFrameHours
                        reAttributionTimeFrameDays: (NSInteger) reAttributionTimeFrameDays
                  reFetchReAttributionDelaySeconds: (NSInteger) reFetchAttributionDelaySeconds];
    }

    void _justtrack_sdk_rp_get_retargeting_parameters() {
        [[NativeBridge shared] getRetargetingParameters];
    }

    const char *_justtrack_sdk_rp_get_preliminary_retargeting_parameters() {
        NSString *s = [[NativeBridge shared] getPreliminaryRetargetingParameters];
        if (s == NULL) {
            return strdup("");
        }
        return strdup([s UTF8String]);
    }

    void _justtrack_sdk_rp_publish_event(const char *name, const char *category, const char *element, const char *action, const char *dimensions) {
        NSString *nsName = [NSString stringWithUTF8String:name];
        NSString *nsCategory = [NSString stringWithUTF8String:category];
        NSString *nsElement = [NSString stringWithUTF8String:element];
        NSString *nsAction = [NSString stringWithUTF8String:action];
        NSString *nsDimensions = [NSString stringWithUTF8String:dimensions];
        [[NativeBridge shared] publishEventWithName: nsName
                                           category: nsCategory
                                            element: nsElement
                                             action: nsAction
                                         dimensions: nsDimensions];
    }

    void _justtrack_sdk_rp_publish_unit_event(const char *name, const char *category, const char *element, const char *action, const char *dimensions, double value, const char *unit) {
        NSString *nsName = [NSString stringWithUTF8String:name];
        NSString *nsCategory = [NSString stringWithUTF8String:category];
        NSString *nsElement = [NSString stringWithUTF8String:element];
        NSString *nsAction = [NSString stringWithUTF8String:action];
        NSString *nsDimensions = [NSString stringWithUTF8String:dimensions];
        NSString *nsUnit = [NSString stringWithUTF8String:unit];
        [[NativeBridge shared] publishEventWithName: nsName
                                           category: nsCategory
                                            element: nsElement
                                             action: nsAction
                                         dimensions: nsDimensions
                                              value: value
                                               unit: nsUnit];
    }

    void _justtrack_sdk_rp_get_affiliate_link(const char *channel) {
        NSString *nsChannel = channel != NULL ? [NSString stringWithUTF8String:channel] : NULL;
        [[NativeBridge shared] getAffiliateLinkWithChannel: nsChannel];
    }

    void _justtrack_sdk_rp_publish_firebase_instance_id(const char *firebaseInstanceId) {
        NSString *nsFirebaseInstanceId = [NSString stringWithUTF8String:firebaseInstanceId];
        [[NativeBridge shared] publishFirebaseInstanceIdWithFirebaseInstanceId:nsFirebaseInstanceId];
    }

    void _justtrack_sdk_rp_integrate_with_iron_source(const char *userId) {
        NSString *nsUserId = [NSString stringWithUTF8String:userId];
        [JustTrackObjCBridge initIronSourceIntegration:nsUserId];
    }

    void _justtrack_sdk_rp_handle_ironsource_revenue_event(const char *adUnit, const char *adNetwork, const char *placement, const char *abTesting, const char *segmentName, const char *instanceName, double revenue) {
        NSString *nsAdUnit = adUnit != NULL ? [NSString stringWithUTF8String:adUnit] : NULL;
        NSString *nsAdNetwork = adNetwork != NULL ? [NSString stringWithUTF8String:adNetwork] : NULL;
        NSString *nsPlacement = placement != NULL ? [NSString stringWithUTF8String:placement] : NULL;
        NSString *nsABTesting = abTesting != NULL ? [NSString stringWithUTF8String:abTesting] : NULL;
        NSString *nsSegmentName = segmentName != NULL ? [NSString stringWithUTF8String:segmentName] : NULL;
        NSString *nsInstanceName = instanceName != NULL ? [NSString stringWithUTF8String:instanceName] : NULL;
        NSNumber *nsRevenue = [NSNumber numberWithDouble:revenue];
        [[NativeBridge shared] ironSourceImpressionDidSucceedWithAdUnit: nsAdUnit
                                                              adNetwork: nsAdNetwork
                                                              placement: nsPlacement
                                                              abTesting: nsABTesting
                                                            segmentName: nsSegmentName
                                                           instanceName: nsInstanceName
                                                                revenue: nsRevenue];
    }

    void _justtrack_sdk_rp_log_debug(const char *message) {
        NSString *nsMessage = [NSString stringWithUTF8String:message];
        [[NativeBridge shared] logDebugWithMessage:nsMessage];
    }

    void _justtrack_sdk_rp_log_info(const char *message) {
        NSString *nsMessage = [NSString stringWithUTF8String:message];
        [[NativeBridge shared] logInfoWithMessage:nsMessage];
    }

    void _justtrack_sdk_rp_log_warning(const char *message) {
        NSString *nsMessage = [NSString stringWithUTF8String:message];
        [[NativeBridge shared] logWarningWithMessage:nsMessage];
    }

    void _justtrack_sdk_rp_log_error(const char *message) {
        NSString *nsMessage = [NSString stringWithUTF8String:message];
        [[NativeBridge shared] logErrorWithMessage:nsMessage];
    }
}
