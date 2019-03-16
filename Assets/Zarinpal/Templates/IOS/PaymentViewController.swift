//
//  PaymentViewController.swift
//  ZarinPalSDKPayment
//
//  Created by ImanX on 12/9/17.
//  Copyright © 2017 ImanX. All rights reserved.
//

import Foundation
import UIKit
import WebKit
class PaymentViewController: UIViewController , UIWebViewDelegate{
    
    var zarinpal:ZarinPal!;
    
    @IBOutlet weak var webKit: UIWebView!
    @IBOutlet weak var indicator: UIActivityIndicatorView!
    @IBOutlet weak var lblTitle: UILabel!
    
  
    override func viewDidLoad() {
        super.viewDidLoad();
        
        if let color = zarinpal.builder.indicatorColor {
            self.indicator.color = color;
        }
        
        if let backgroundColor = zarinpal.builder.pageBackgroundColor {
            self.view.backgroundColor = backgroundColor;
        }
        
        if let title = zarinpal.builder.title {
            self.lblTitle.text = title;
            self.lblTitle.isHidden = false;
        }
    
        self.webKit.delegate = self;
        let request = HttpRequest(url: URLs.PAYMENT_REQUEST_URL(), method: .Post);
        
        request.params = [
            "MerchantID" : zarinpal.builder.merchantID,
            "Amount" : zarinpal.builder.amount,
            "Description" : zarinpal.builder.description,
            "CallbackURL" : "pay://zarinpal"
        ];
        
      
        
        request.request { (response) in
            let status = response["Status"] as! Int;
            if status == 100 {
                let authority = response["Authority"] as! String;
                let url = URL(string: URLs.START_PG_URL(authority));
                self.webKit.loadRequest(URLRequest(url: url!));
                return;
            }
            
            
            self.zarinpal.delegate.didFailure(code: status,authority: nil);
        }
        
        
    }
    
    func dismiss() {
        DispatchQueue.main.async {
            self.dismiss(animated: true, completion: {
                print("dismiss");
            })
        }
    }
    
    
    func webViewDidFinishLoad(_ webView: UIWebView) {
        indicator.isHidden = true;
        webKit.isHidden = false;
    }
   
    
    func verification(isOK:Bool , authority:String) {
        webKit.isHidden = true;
        indicator.isHidden = false;
        
        if !isOK {
            zarinpal.delegate.didFailure(code: -100, authority:authority);
            self.dismiss();
            return;
        }
        
        
        let request = HttpRequest(url: URLs.VERIFICATION_URL(), method: .Post);
        
        request.params = [
            "MerchantID" : zarinpal.builder.merchantID,
            "Amount" : zarinpal.builder.amount,
            "Authority" : authority,
        ];
        
        request.request { (response) in
            let status = response["Status"] as! Int;
            if status == 100 {
                let refID = response["RefID"] as! Int;
                self.zarinpal.delegate.didSuccess(refID: refID.description, authority: authority, builder: self.zarinpal.builder);
                self.dismiss();
                return;
            }
            
            self.zarinpal.delegate.didFailure(code: status, authority: authority);
            self.dismiss();
        
        }
    }
    
    
    func webView(_ webView: UIWebView, shouldStartLoadWith request: URLRequest, navigationType: UIWebView.NavigationType) -> Bool {
        
        let callback = request.url;
        
        
        guard let host = callback?.host else{
            return false;
        }
        
        guard let scheme = callback?.scheme else {
            return false;
        }
        
        if (scheme + "://" + host )  == "pay://zarinpal" {
            
            let isOK = callback?.parse(query: "Status") == "OK";
            let authority = callback?.parse(query: "Authority");
            self.verification(isOK: isOK, authority: authority!);
        }
        
        return true;
        
    }
    

    
    @IBAction func closeClicked(_ sender: UIBarButtonItem){
        zarinpal.delegate.didFailure(code: -101, authority: nil);
        self.dismiss();
    }
    
    
    
}


extension URL {
    
    func parse(query:String) -> String? {
        if let uri = URLComponents(string: self.absoluteString){
            return uri.queryItems?.first(where: {$0.name == query})?.value;
        }
        
        return nil;
    }
    
}
