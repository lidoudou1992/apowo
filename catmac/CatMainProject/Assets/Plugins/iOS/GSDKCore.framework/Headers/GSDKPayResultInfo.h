//
//  GSDKPayResultInfo.h
//  sanguo_7659
//
//  Created by apple on 16/11/30.
//
//

#ifndef GSDKPayResultInfo_h
#define GSDKPayResultInfo_h

#include "EGSDKPayResultStatus.h"

@interface GSDKPayResultInfo : NSObject

@property EGSDKPayResultStatus status;
@property (copy) NSString* infoStr;
@property int internalErrorCode;
@property (copy) NSString* internalErrorStr;

- (NSString*) toString;

@end

#endif /* GSDKPayResultInfo_h */
