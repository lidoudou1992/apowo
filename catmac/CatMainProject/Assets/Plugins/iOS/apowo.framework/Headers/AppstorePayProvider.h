//
//  AppstorePayProvider.h
//  sanguo_AppleIAP
//
//  Created by apple on 16/12/5.
//
//

#ifndef AppstorePayProvider_h
#define AppstorePayProvider_h

#import <StoreKit/StoreKit.h>
#import <GSDKCore/GSDKCore.h>
// #import "GSDKCore.h"
@class GSDKPlatform;
@class IAPNotifyManager;

@interface AppstorePayProvider : GSDKPayProviderBase <SKProductsRequestDelegate, SKPaymentTransactionObserver>
@property (assign) GSDKPlatform* platformImpl;
@property (strong) SKProductsRequest* curProductsRequest;
@property (strong) SKPaymentTransaction* curPaymentTransaction;
@property (strong) IAPNotifyManager* iapNotifyManager;
- (AppstorePayProvider*) initWithPlatform: (GSDKPlatform*) platform;
- (void) initialize;
- (void) startPayWithGUI: (GSDKPayInfo*) payInfo withPayHandler:(void(^)(GSDKPayResultInfo*))payHandler;
- (void) productsRequest:(SKProductsRequest*)request didReceiveResponse:(SKProductsRequest*)response;
- (void) paymentQueue:(SKPaymentQueue *)queue updatedTransactions:(NSArray *)transactions;
- (bool) getExchangeRate;
- (NSString*) getExchangeName;

- (BOOL) isCurrentPaymentTransaction:(SKPaymentTransaction*)transaction;
- (void) resetPayment;

// +


@end
#endif /* AppstorePayProvider_h */
