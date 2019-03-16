//
//  ZarinpalUnityWrapper.swift
//  ZarinpalUnityPlugin
//
//  Created by Mojtaba Pirveisi on 3/16/19.
//  Copyright © 2019 Mojtaba Pirveisi. All rights reserved.
//

import Foundation

@objc public class ZarinpalUnityWrapper: NSObject {
    
    
    @objc public static let shared = ZarinpalUnityWrapper();
    
    private let zarinpalUnity = ZarinpalUnity();
    
    @objc public func initialize(merchantID : String)
    {
        zarinpalUnity.initialize(merchantID: merchantID)
    }
    
    @objc public func startPurchaseFlow(amount : Int , productID: String , desc: String) {
        
        zarinpalUnity.startPurchaseFlow(amount: amount, productID: productID, desc: desc);
    }
}
