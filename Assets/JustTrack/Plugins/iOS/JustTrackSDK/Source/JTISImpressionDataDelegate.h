#ifndef JTISImpressionDataDelegate_h
#define JTISImpressionDataDelegate_h

#if defined(ENABLE_IRONSOURCE_INTEGRATION) && ENABLE_IRONSOURCE_INTEGRATION

#include "IronSource.h"

@interface JustTrackImpressionDataDelegate : NSObject <ISImpressionDataDelegate>
- (void)impressionDataDidSucceed:(ISImpressionData *)impressionData;
@end

#endif

#endif /* JTISImpressionDataDelegate_h */
