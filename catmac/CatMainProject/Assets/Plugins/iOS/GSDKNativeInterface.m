
#import <apowo/apowo.h> // NOTE will be changed
#import <ApowoSDK/ApowoSDK.h> // NOTE will be changed
#import <ApowoCocoaBaseLib/ApowoCocoaBaseLib.h>
#import <GSDKCore/GSDKCore.h>
#import <GSDKCore/GSDKPayInfo.h>
#import <apowo/GSDKPlatform.h>

#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

void GSDKNI_Initialize() {
    UIWindow* uiWindow = [[UIWindow alloc] initWithFrame:[[UIScreen mainScreen] bounds]];
    UIViewController* uiViewController = [uiWindow rootViewController];
    [uiWindow setUserInteractionEnabled:true];
    [uiWindow makeKeyAndVisible];
    [[GSDKPlatform instance] initialize:uiViewController];
}

char* GSDKNI_GetGSDKAppInfoJson() {
    return MakeStringCopy(@"{}");
}

bool GSDKNI_IsInternalNetworkPlatform() {
    return false;
}

bool GSDKNI_SupportPlatformAccount() {
    return [[GSDKPlatform instance] supportPlatformAccount];
}

bool GSDKNI_GuiAvailableLogin() {
    [[[GSDKPlatform instance] accountProviderImpl] guiAvailableLogin];
}

bool GSDKNI_IsAPIAvailable_RequestExit() {
    return [[GSDKPlatform instance] isAPIAvailable_RequestExit];
}

bool GSDKNI_GuiAvailableRegist() {
    return false;
}

bool GSDKNI_GuiAvailableChangePass() {
    return false;
}

bool GSDKNI_GuiAvailableRestorePass() {
    return false;
}

bool GSDKNI_GuiAvailableChangeAccount() {
    return false;
}

float GSDKNI_GetExchangeRate() {
    return [[[GSDKPlatform instance] payProviderImpl] getExchangeRate];
}

char* GSDKNI_GetExchangeName() {
    return MakeStringCopy([[[GSDKPlatform instance] payProviderImpl] getExchangeName]);
}

char* GSDKNI_GetAccountProviderName() {
    return MakeStringCopy(@"not supported");
}

void GSDKNI_OnAppExiting() {
}

void GSDKNI_SetServerID(const char* id) {
}

void GSDKNI_SetCharacterID(const char* id) {
}

void GSDKNI_StartLoginWithGUI() {
    [[[GSDKPlatform instance] accountProviderImpl] startLoginWithGUI:^(GSDKLoginResultInfo* loginResultInfo)  {
        dispatch_async(dispatch_get_main_queue(), ^{
            NSString* jsonStr = [loginResultInfo toString];
            UnitySendMessage("GSDKProxy", "OnJavaLoginResult", [jsonStr UTF8String]);
        });
    }];
}

void GSDKNI_StartRestorePassWithGUI() {
    // nothing to do here
}

void GSDKNI_StartChangePassWithGUI() {
    // nothing to do here
}

void GSDKNI_StartMainVerUpgrade() {
    // nothing to do here
}

void GSDKNI_StartPayWithGUI(const char* payInfoJsonStr) {
    GSDKPayInfo* payInfo = [[GSDKPayInfo alloc] init];
    NSString* str = [NSString stringWithUTF8String:payInfoJsonStr];
    [payInfo fromJsonString:str];
    [[[GSDKPlatform instance] payProviderImpl] startPayWithGUI:payInfo withPayHandler:^(GSDKPayResultInfo* payResultInfo) {
        UnitySendMessage("GSDKProxy", "OnJavaPayResult", [[payResultInfo toString] UTF8String]);
    }
     ];
}

void GSDKNI_StartCheckOrder(const char* orderID) {
    // nothing to do here
}

void GSDKNI_SetGSDKAppInfoJson(const char* infoJsonStr) {
    // nothing to do here
}
