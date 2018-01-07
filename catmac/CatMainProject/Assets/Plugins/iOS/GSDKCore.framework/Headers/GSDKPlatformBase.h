//
//  GSDKPlatformBase.h
//  sanguo_7659
//
//  Created by apple on 16/11/29.
//
//

#ifndef GSDKPlatformBase_h
#define GSDKPlatformBase_h

@class GSDKAccountProviderBase;
@class GSDKPayProviderBase;
@class GSDKLoginResultInfo;
@class GSDKPayResultInfo;
@class GSDKPayInfo;

#import <UIKit/UIKit.h>

@interface GSDKPlatformBase : NSObject {
    
}

@property (retain) GSDKAccountProviderBase* accountProviderBase;
@property (retain) GSDKPayProviderBase* payProviderBase;
@property (retain) UIViewController* rootViewController;

@property (copy) NSString* gSDKCompanyName;
@property (copy) NSString* gSDKGameName;
@property (copy) NSString* gSDKGameID;
@property (copy) NSString* gSDKAccountPlatformID;
@property (copy) NSString* gSDKAccountPlatformName;
@property (copy) NSString* gSDKPayPlatformID;
@property (copy) NSString* gSDKPayPlatformName;
@property (copy) NSString* gSDKAuthKey;
@property (copy) NSString* gSDKPayKey;
@property (copy) NSString* gSDKCreateOrderURL;
@property (copy) NSString* gSDKPayCallbackURL;


- (void) initialize:(UIViewController*) rootViewController;

- (long long) getServerTimeStamp;

// +
- (void) startLoginWithGUI:(void (^) (GSDKLoginResultInfo*)) handler;
- (void) startPayWithGUI:(GSDKPayInfo*)payInfo withPayHandler:(void (^) (GSDKPayResultInfo*))handler;
- (bool) supportPlatformAccount;

// Not supported
// - (NSString*) getAccountProviderName;




@end

#endif /* GSDKPlatformBase_h */
