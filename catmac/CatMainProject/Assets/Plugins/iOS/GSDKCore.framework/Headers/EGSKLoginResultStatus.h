//
//  ELoginResultStatus.h
//  sanguo_7659
//
//  Created by apple on 16/11/29.
//
//

#ifndef EGSDKLoginResultStatus_h
#define EGSDKLoginResultStatus_h

typedef enum {
    eGSDKLoginResultStatus_Succeed = 0,
    eGSDKLoginResultStatus_UsernameFormatError = 1,
    eGSDKLoginResultStatus_UsernameNotExist = 2,
    eGSDKLoginResultStatus_PasswordFormatError = 3,
    eGSDKLoginResultStatus_WrongPassword = 4,
    eGSDKLoginResultStatus_Cancelled = 5,
    eGSDKLoginResultStatus_VerifyFailed = 6,
    eGSDKLoginResultStatus_Failed = 7,
    eGSDKLoginResultStatus_InternalError = 100
} EGSDKLoginResultStatus;


#endif /* EGSDKLoginResultStatus_h */
