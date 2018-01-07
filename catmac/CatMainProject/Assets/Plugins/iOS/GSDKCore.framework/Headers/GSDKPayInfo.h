//
//  GSDKPayInfo.h
//  sanguo_7659
//
//  Created by apple on 16/11/30.
//
//

#ifndef GSDKPayInfo_h
#define GSDKPayInfo_h

@interface GSDKPayInfo : NSObject

@property (copy) NSString* propName; 			// 商品名称
@property (copy) NSString* priceInCurrency;	// 真实货币价格 单位是货币的最小面值 (如人民币的1分)
@property (copy) NSString* propID; 			// 游戏内部的计费代码
@property (copy) NSString* payID;				// 在渠道注册的计费代码 （根据GSDK_PayIDMap.json来做映射，也可以在调用支付接口时由游戏层指定）
@property float exchangeRate; 		            // 真实货币与平台币的汇率 （如果调用支付接口时未指定，则读取SDK内的配置 ）
@property (copy) NSString* userID; 			// 当前玩家的游戏内唯一标识
@property (copy) NSString* userAccount; 		// 玩家登录名
@property (copy) NSString* serverID; 			// 服务器ID
@property (copy) NSString* propDesc; 			// 商品描述信息
@property (copy) NSString* gSDKOrderID;       // GSDK订单号
@property (copy) NSString* callbackExtraInfo;	// 回调透传 默认值设置为""

- (NSString*) toString;
- (void) fromJsonString:(NSString*) jsonStr;

@end

#endif /* GSDKPayInfo_h */
