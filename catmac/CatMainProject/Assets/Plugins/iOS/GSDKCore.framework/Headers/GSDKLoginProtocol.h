//
//  Header.h
//  sanguo_7659
//
//  Created by apple on 16/11/29.
//
//

#ifndef GSDKLoginProtocol_h
#define GSDKLoginProtocol_h

#import "GSDKLoginResultInfo.h"

@protocol GSDKLoginProtocol <NSObject>

- (void) loginCallback:(GSDKLoginResultInfo*)loginResultInfo;

@end

#endif /* GSDKLoginProtocol_h */
