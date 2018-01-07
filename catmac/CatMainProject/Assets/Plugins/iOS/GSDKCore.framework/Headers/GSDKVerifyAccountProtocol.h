//
//  GSDKVerifyAccountProtocol.h
//  sanguo_7659
//
//  Created by apple on 16/11/29.
//
//

#ifndef GSDKVerifyAccountProtocol_h
#define GSDKVerifyAccountProtocol_h

#import "GSDKVerifyAccountInfo.h"

@protocol GSDKVerifyAccountProtocol <NSObject>

- (void) verifyAccountCallback:(GSDKVerifyAccountInfo*)verifyAccountInfo;

@end

#endif /* GSDKVerifyAccountProtocol_h */
