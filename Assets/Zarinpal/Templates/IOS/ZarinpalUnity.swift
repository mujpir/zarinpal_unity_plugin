//
//  ZarinpalUnity.swift
//  ZarinpalUnityPluginXcode
//
//  Created by Mojtaba Pirveisi on 3/16/19.
//  Copyright © 2019 Mojtaba Pirveisi. All rights reserved.
//


import UIKit

class ZarinpalUnity : ZarinPalPaymentDelegate {
    
    let kCallbackTarget = "ZarinpaliOS"
    var merchantID : String = "" ;
    var viewController : UIViewController?;
    func didSuccess(refID: String, authority: String, builder: ZarinPal.Builder) {
        //when Payment is Success and return:
        //refID: this is transaction id.
        //authority: this is a payment unique id
        //payment : included payment details ex: amount , description
        print(refID);
        var message : String = "Payment is success with refid : \(refID)";
        UnitySendMessage(kCallbackTarget,"OnPaymentVerificationSucceed",refID);
    }
    
    func didFailure(code: Int, authority: String?) {
        //when Payment is failure and return:
        //status : ZarinPal failure codes
        //authority: this is a payment unique id
        
        var error : String = "Purchase failed with code : \(code)";
        print(error);
        UnitySendMessage(kCallbackTarget,"OnPurchaseFailed",String(error));
    }
    
    func initialize(merchantID:String)
    {
        self.merchantID = merchantID ;
        viewController = UIApplication.shared.keyWindow!.rootViewController;
        UnitySendMessage(kCallbackTarget,"OnStoreInitialized","nullMessages");
    }
    
    func startPurchaseFlow(amount : Int , productID : String , desc : String) {
        
        do{
            let zarinpal =   ZarinPal.Builder(vc: viewController!, merchantID: self.merchantID, amount:amount, description: desc);
            
            zarinpal.indicatorColor = UIColor.black;  //this set indicator color *optional
            zarinpal.title = "Payment Gateway";  //this set title of payment page *optional
            zarinpal.pageBackgroundColor = UIColor.lightGray; // this set background payment color *optional
            zarinpal.email = "email@gmail.com"; //this set email *optional
            zarinpal.mobile = "09355106005"; //this set mobile *optional
            zarinpal
                .build()
                .start(delegate: self);
            UnitySendMessage(kCallbackTarget,"OnPurchaseStarted","nullMessages");
            print("OnPurchaseStarted")
        }
        catch{
            var error : String = "Unexpected error on starting a purchase : \(error)";
            UnitySendMessage(kCallbackTarget,"OnPurchaseFailedToStart",error);
            print(error)
        }
    }
    
}
