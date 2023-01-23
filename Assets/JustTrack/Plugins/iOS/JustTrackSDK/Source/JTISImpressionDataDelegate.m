//
//  JTImpressionDataDelegate.m
//  JustTrackSDK
//
//  Created by Anselm Jonas Scholl on 09.12.20.
//

#import <Foundation/Foundation.h>

#if defined(ENABLE_IRONSOURCE_INTEGRATION) && ENABLE_IRONSOURCE_INTEGRATION

#include "JTISImpressionDataDelegate.h"
#include "JustTrackSDK-Swift.h"

@implementation JustTrackImpressionDataDelegate

- (void)impressionDataDidSucceed:(ISImpressionData *)impressionData {
    if (impressionData == NULL) {
        return;
    }
    [[NativeBridge shared] ironSourceImpressionDidSucceedWithAdUnit:impressionData.ad_unit
                                                          adNetwork:impressionData.ad_network
                                                          placement:impressionData.placement
                                                          abTesting:impressionData.ab
                                                        segmentName:impressionData.segment_name
                                                       instanceName:impressionData.instance_name
                                                            revenue:impressionData.revenue];
}

@end

#endif
