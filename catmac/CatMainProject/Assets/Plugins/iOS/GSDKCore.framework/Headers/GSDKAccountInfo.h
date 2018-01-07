//
//  GSDKAccountInfo.h
//  sanguo_7659
//
//  Created by apple on 16/11/29.
//
//

#ifndef GSDKAccountInfo_h
#define GSDKAccountInfo_h

@interface GSDKAccountInfo : NSObject
@property bool loggedIn;               // 是否已登录 (depreciated)
@property (copy) NSString* uID;
@property (copy) NSString* username;
@property (copy) NSString* password;
@property (copy) NSString* characterID;
@property bool binded;
@property (copy) NSString* serverID;
@property (copy) NSMutableDictionary* extraJsonDict;
- (GSDKAccountInfo*) init;
- (NSMutableDictionary*) toDictionary;
- (NSString*) toString;

@end

#endif /* GSDKAccountInfo_h */
