//
//  GSDKPlatform.h
//  apowo
//
//  Created by apple on 16/12/15.
//  Copyright © 2016年 apowo. All rights reserved.
//

#ifndef GSDKPlatform_h
#define GSDKPlatform_h

#import "ApowoAccountProvider.h"
#import "AppstorePayProvider.h"

@interface GSDKPlatform : GSDKPlatformBase

@property (strong) ApowoAccountProvider* accountProviderImpl;
@property (strong) AppstorePayProvider* payProviderImpl;

@property (copy) NSString* apowoSDK_AppID;
@property (copy) NSString* apowoSDK_AppKey;
@property (copy) NSString* apowoSDK_DistributeChannel;

+ (GSDKPlatform*) instance;

- (GSDKPlatform*) init;

- (void) initialize:(UIViewController*) rootViewController;

- (bool) isInternalNetworkPlatform;
- (bool) isAPIAvailable_RequestExit;

// +


@end

#endif /* GSDKPlatform_h */
