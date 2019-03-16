//
//  PaymentDelegate.swift
//  ZarinPalSDKPayment
//
//  Created by ImanX on 12/9/17.
//  Copyright © 2017 ImanX. All rights reserved.
//

import Foundation
public protocol ZarinPalPaymentDelegate {
    func didSuccess(refID:String , authority:String , builder:ZarinPal.Builder);
    func didFailure(code:Int, authority:String?);
}
