//
//  GSDKGetOrderInfo.h
//  sanguo_7659
//
//  Created by apple on 16/11/30.
//
//

#ifndef GSDKGetOrderInfo_h
#define GSDKGetOrderInfo_h

#import "EGSDKGetOrderStatus.h"

@interface GSDKGetOrderInfo : NSObject

@property EGSDKGetOrderStatus status;
@property (copy) NSString* order;
@property (copy) NSString* platformSign;
@property (copy) NSString* platformOrder;
@property long long createOrderTime;
@property (copy) NSString* errorMsg;
@property int internalErrorCode;
@property (copy) NSString* internalErrorMsg;
@property (copy) NSString* rawResStr;
@property (copy) NSDictionary* rawResJsonDict;

- (GSDKGetOrderInfo*) init;

- (NSMutableDictionary*) toDictionary;
- (NSString*) toString;

@end

#endif /* GSDKGetOrderInfo_h */
