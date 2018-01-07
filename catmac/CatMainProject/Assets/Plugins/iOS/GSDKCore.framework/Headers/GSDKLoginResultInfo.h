//
//  LoginResultInfo.h
//  sanguo_7659
//
//  Created by apple on 16/11/29.
//
//

#ifndef GSDKLoginResultInfo_h
#define GSDKLoginResultInfo_h

#import "GSDKAccountInfo.h"
#import "EGSKLoginResultStatus.h"

// @class GSDKAccountInfo;


@interface GSDKLoginResultInfo : NSObject

@property EGSDKLoginResultStatus status;
@property int internalErrorCode;
@property (copy) NSString* errorMsg;
@property (retain) GSDKAccountInfo* accountInfo;
@property bool noVerify;

- (GSDKLoginResultInfo*) init;
- (NSString*) toString;

@end

#endif /* GSDKLoginResultInfo_h */
