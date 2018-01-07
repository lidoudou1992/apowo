//
//  EGSDKGetOrderStatus.h
//  sanguo_7659
//
//  Created by apple on 16/11/30.
//
//

#ifndef EGSDKGetOrderStatus_h
#define EGSDKGetOrderStatus_h

typedef enum {
    eGSDKGetOrderStatus_Succeed = 0,
    eGSDKGetOrderStatus_Failed = 1,
    eGSDKGetOrderStatus_ProviderNotAvailable = 6,
    eGSDKGetOrderStatus_APINotSupported = 7,
    eGSDKGetOrderStatus_ServerError = 8,
    eGSDKGetOrderStatus_JSONError = 9,
    eGSDKGetOrderStatus_InternalError = 100
} EGSDKGetOrderStatus;

#endif /* EGSDKGetOrderStatus_h */
