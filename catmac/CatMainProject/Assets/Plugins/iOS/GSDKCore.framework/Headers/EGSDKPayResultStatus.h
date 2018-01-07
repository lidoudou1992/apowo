//
//  EGSDKPayResultStatus.h
//  sanguo_7659
//
//  Created by apple on 16/11/30.
//
//

#ifndef EGSDKPayResultStatus_h
#define EGSDKPayResultStatus_h

typedef enum {
    eGSDKPayResultStatus_Succeed = 0,
    eGSDKPayResultStatus_Failed = 1,
    eGSDKPayResultStatus_Cancelled = 2,
    eGSDKPayResultStatsu_FailedToGetOrder = 6,
    eGSDKPayResultStatsu_InternalError = 100
} EGSDKPayResultStatus;

#endif /* EGSDKPayResultStatus_h */
