//
//  GSDKVerifyAccountInfo.h
//  sanguo_7659
//
//  Created by apple on 16/11/29.
//
//

#ifndef GSDKVerifyAccountInfo_h
#define GSDKVerifyAccountInfo_h

#import "EGSDKVerifyAccountStatus.h"

@interface GSDKVerifyAccountInfo : NSObject

@property enum EGSDKVerifyAccountStatus status;
@property (copy) NSString* token;
@property (copy) NSString* token2;
@property (copy) NSString* token3;
@property (copy) NSString* platformUID;
@property (copy) NSString* serverTime;
@property (retain) NSMutableDictionary* extraJsonDict;
@property int internalErrorCode;
@property (copy) NSString* errorMsg;

@end

#endif /* GSDKVerifyAccountInfo_h */
