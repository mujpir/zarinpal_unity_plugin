//
//  ZarinPal.swift
//  ZarinPalSDKPayment
//
//  Created by ImanX on 12/9/17.
//  Copyright © 2017 ImanX. All rights reserved.
//

import Foundation
import UIKit
open class ZarinPal {
   
    public var builder:Builder!;
    public var delegate:ZarinPalPaymentDelegate!;
    
    public init(builder:Builder) {
        self.builder = builder;
    }
    
    public func start(delegate:ZarinPalPaymentDelegate){
        self.delegate = delegate;
        let storyboard = UIStoryboard(name: "PaymentBoard", bundle: Bundle(for: PaymentViewController.self));
        let vc = storyboard.instantiateViewController(withIdentifier: "PaymentViewController") as! PaymentViewController;
        vc.zarinpal = self;
        self.builder.viewController.present(vc, animated: true, completion: nil);
    }
    
    
    open class Builder{
        
        public var viewController:UIViewController!;
        public var merchantID:String!;
        public var amount:Int!;
        public var description:String!;
        public var mobile:String?;
        public var email:String?;
        
        public var indicatorColor:UIColor?;
        public var pageBackgroundColor:UIColor?;
        public var title:String?;
        
        public init(vc:UIViewController , merchantID:String , amount:Int , description:String){
            self.viewController = vc;
            self.merchantID = merchantID;
            self.amount = amount;
            self.description = description;
        }
        
        public func build()->ZarinPal{
            return ZarinPal(builder: self);
        }
        
    
    }
    
}
