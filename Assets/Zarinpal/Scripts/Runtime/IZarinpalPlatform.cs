using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IZarinpalPlatform
{
    string MerchantID { get; }
    bool AutoVerifyPurchase { get; }

    string Callback { get; }
    bool IsInitialized { get; }

    string PurchasingItemID { get; }

    void Initialize(string merchantID, bool verifyPurchase,string callbackScheme);

    void Purchase(long amount, string productID, string desc);

    event Action StoreInitialized;

    event Action PurchaseStarted;

    event Action<string> PurchaseFailedToStart;

    event Action<string,string> PurchaseSucceed;

    event Action PurchaseFailed;

    event Action PurchaseCanceled;

    event Action<string> PaymentVerificationStarted;

    event Action<string> PaymentVerificationSucceed;

    event Action PaymentVerificationFailed;
}
