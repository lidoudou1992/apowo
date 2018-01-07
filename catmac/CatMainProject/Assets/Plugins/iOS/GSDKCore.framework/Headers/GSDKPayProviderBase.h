//
//  GSDKPayProviderBase.h
//  sanguo_7659
//
//  Created by apple on 16/11/29.
//
//

#ifndef GSDKPayProviderBase_h
#define GSDKPayProviderBase_h

#import "GSDKPlatformBase.h"
//@class GSDKPlatformBase;
#import "GSDKPayInfo.h"
#import "GSDKGetOrderInfo.h"
#import "GSDKPayResultInfo.h"

@interface GSDKPayProviderBase : NSObject

@property (assign) GSDKPlatformBase* platformBase;
@property (copy) void (^curPayHandler) (GSDKPayResultInfo*);
@property (retain) GSDKPayInfo* curPayInfo;

- (GSDKPayProviderBase*) initWithPlatform: (GSDKPlatformBase*) platformBase;

- (void) startPayWithGUI: (GSDKPayInfo*) payInfo withPayHandler:(void(^)(GSDKPayResultInfo*))payHandler;

- (void) startGetOrder: (GSDKPayInfo*) payInfo withExtraURLParamDict:(NSMutableDictionary*) extraURLParamDict withGetOrderHandler:(void(^)(GSDKGetOrderInfo*))getOrderHandler;

@end

#endif /* GSDKPayProviderBase_h */
