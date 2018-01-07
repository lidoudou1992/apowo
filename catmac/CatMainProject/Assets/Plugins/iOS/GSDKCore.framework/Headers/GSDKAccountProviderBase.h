//
//  AccountProviderBase.h
//  sanguo_7659
//
//  Created by apple on 16/11/26.
//
//

#ifndef GSDKAccountProviderBase_h
#define GSDKAccountProviderBase_h

#import "GSDKLoginResultInfo.h"
#import "GSDKLoginProtocol.h"
#import "GSDKVerifyAccountProtocol.h"
#import "GSDKVerifyAccountInfo.h"
#import "GSDKPlatformBase.h"


@interface GSDKAccountProviderBase : NSObject <GSDKVerifyAccountProtocol>

@property (assign) GSDKPlatformBase* platformBase;
@property (copy) NSString* gSDKGameLoginAuthURL;
@property (nonatomic, strong) void (^curLoginHandler) (GSDKLoginResultInfo*);

- (GSDKAccountProviderBase*) initWithPlatform: (GSDKPlatformBase*) platformBase;

- (void) startVerifyAccount:(NSString *) userIDStr withSession:(NSString *) sessionStr withExtraURLParam:(NSMutableDictionary*)extraURLParams withVerifyHandler:(void(^)(GSDKVerifyAccountInfo*)) verifyHandler;

- (void) defaultPlatformLoginSucceedProcess:(NSString*) platformUID withAuthToken:(NSString*)authToken withVerifyAccountSucceedHandler:(void (^) (GSDKVerifyAccountInfo*))verifyAccountHandler withLoginHandler:(void (^) (GSDKLoginResultInfo*))loginHandler;

- (void) defaultPlatformLoginSucceedProcess:(NSString*) platformUID withAuthToken:(NSString*)authToken withExtraUrlParams:(NSMutableDictionary*)extraUrlParams withVerifyAccountSucceedHandler:(void (^) (GSDKVerifyAccountInfo*))verifyAccountHandler withLoginHandler:(void (^) (GSDKLoginResultInfo*))loginHandler;

- (void) startLoginWithGUI:(void (^) (GSDKLoginResultInfo*)) handler;

@end

#endif /* GSDKAccountProviderBase_h */
