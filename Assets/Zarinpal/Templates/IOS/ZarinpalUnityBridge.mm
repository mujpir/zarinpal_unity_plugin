//
//  ZarinpalUnityBridge.m
//  ZarinpalUnityPlugin
//
//  Created by Mojtaba Pirveisi on 3/16/19.
//  Copyright © 2019 Mojtaba Pirveisi. All rights reserved.
//

#import <Foundation/Foundation.h>
#include "ZarinpalUnityPlugin-Swift.h"



#pragma mark - C interface

extern "C" {
    
    void _zu_initialize(const char* merchantID)
    {
        [[ZarinpalUnityWrapper shared]initializeWithMerchantID:[NSString stringWithUTF8String:merchantID]];
    }
    
    void _zu_startPurchaseFlow(int amount,const char* productID,const char* desc)
    {
        [[ZarinpalUnityWrapper shared] startPurchaseFlowWithAmount:amount productID:[NSString stringWithUTF8String:productID] desc:[NSString stringWithUTF8String:desc]];
    }
    
}
