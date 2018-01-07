//
//  ApowoAccountProvider.h
//  sanguo_AppleIAP
//
//  Created by apple on 16/12/6.
//
//

#ifndef ApowoAccountProvider_h
#define ApowoAccountProvider_h

// @import GSDKCore;
#import <GSDKCore/GSDKCore.h>
// #import "GSDKCore.h"

@class GSDKPlatform;

@interface ApowoAccountProvider : GSDKAccountProviderBase
@property GSDKPlatform* platformImpl;
- (ApowoAccountProvider*) initWithPlatform: (GSDKPlatform*) platform;
- (void) initialize;
- (void) startLoginWithGUI:(void(^)(GSDKLoginResultInfo*)) handler;
- (bool) guiAvailableLogin;

// +

@end


#endif /* ApowoAccountProvider_h */
