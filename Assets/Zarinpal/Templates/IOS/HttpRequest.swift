 //
//  HttpRequest.swift
//  ZarinPalSDKPayment
//
//  Created by ImanX on 12/9/17.
//  Copyright © 2017 ImanX. All rights reserved.
//

import Foundation
class HttpRequest{
    private var _URL:String!;
    private var _method:Method!;
    private var _data:Data?;

    
    public init(url:String , method:Method) {
        self._URL = url;
        self._method = method
    }
    
    
    public var params:[String:Any]?{
        set{
            do{
            self._data = try JSONSerialization.data(withJSONObject: newValue, options: .prettyPrinted)
            }catch let error{
                print(error);
            }
            
        }
        get{return nil}
    }

    
    public func request(compelition: @escaping ([String:Any]) ->Void) {
        var request = URLRequest(url: URL(string: self._URL)!);
        request.httpMethod = self._method.rawValue;
        request.addValue("application/json", forHTTPHeaderField: "Content-Type")
        request.addValue("application/json", forHTTPHeaderField: "Accept")
       
        if let body = self._data  {
            request.httpBody = body;
        }
        
        let task = URLSession.shared.dataTask(with: request) { (data, response, error) in
          
            guard error == nil else {
                return
            }
            
            guard let data = data else {
                return
            }
            
            do {
                //create json object from data
                if let json = try JSONSerialization.jsonObject(with: data, options: .mutableContainers) as? [String: Any] {
                    compelition(json);
                }
            } catch let error {
                print(error.localizedDescription)
            }
        }
        task.resume();
        
        
    }
    
    
    
    

}





public enum Method: String{
    case Post = "POST"
    case Get = "GET"
}
