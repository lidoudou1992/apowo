//
//  CryptoUtil.h
//  sanguo_7659
//
//  Created by apple on 16/11/29.
//
//

#ifndef CryptoUtil_h
#define CryptoUtil_h

@interface CryptoUtil : NSObject

+ (NSString*) urlParamsDictToSortedString:(NSDictionary*) urlParamsDict;

+ (NSString*) md5:(NSString*) input;

@end

#endif /* CryptoUtil_h */
