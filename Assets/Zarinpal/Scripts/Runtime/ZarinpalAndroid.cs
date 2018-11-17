using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZarinpalAndroid : MonoBehaviour,IZarinpalPlatform
{
    private AndroidJavaClass _zarinpalJavaClass;
    private AndroidJavaObject _zarinpalJavaObject;
    private static ZarinpalAndroid _instance;

    public static ZarinpalAndroid CreateInstance()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<ZarinpalAndroid>();

            if (_instance == null)
            {
                _instance = new GameObject("ZarinpalAndroid").AddComponent<ZarinpalAndroid>();
                DontDestroyOnLoad(_instance.gameObject);
            }
        }
        return _instance;
    }

    public string MerchantID { get; private set; }
    public bool AutoVerifyPurchase { get; private set; }
    public string Callback { get; private set; }
    public bool IsInitialized { get; private set; }
    public string PurchasingItemID { get; private set; }

    public void Initialize(string merchantID,bool verifyPurchase,string schemeCallback)
    {
        MerchantID = merchantID;
        AutoVerifyPurchase = verifyPurchase;
        Callback = schemeCallback;
        _zarinpalJavaClass = new AndroidJavaClass("com.kingcodestudio.unityzarinpaliab.ZarinpalActivity");
        _zarinpalJavaClass.CallStatic("initialize", merchantID, verifyPurchase, schemeCallback);
    }

    public void Purchase(long amount,string desc, string productID)
    {
        PurchasingItemID = productID;
        _zarinpalJavaClass.CallStatic("startPurchaseFlow", amount, productID, desc);
    }

    public event Action StoreInitialized;
    public event Action PurchaseStarted;
    public event Action<string> PurchaseFailedToStart;
    public event Action<string,string> PurchaseSucceed;
    public event Action PurchaseFailed;
    public event Action PurchaseCanceled;
    public event Action<string> PaymentVerificationStarted;
    public event Action<string> PaymentVerificationSucceed;
    public event Action PaymentVerificationFailed;



    #region Callbacks

    private void OnStoreInitialized(string nullMessage)
    {
        IsInitialized = true;
        var handler = StoreInitialized;
        if (handler != null) handler();
    }

    private void OnPurchaseStarted(string nullMessage)
    {
        var handler = PurchaseStarted;
        if (handler != null) handler();
    }

    private void OnPurchaseFailedToStart(string error)
    {
        var handler = PurchaseFailedToStart;
        if (handler != null) handler(error);
    }

    private void OnPurchaseSucceed(string authority)
    {
        var handler = PurchaseSucceed;
        if (handler != null) handler(PurchasingItemID,authority);
    }

    private void OnPurchaseFailed(string error)
    {
        var handler = PurchaseFailed;
        if (handler != null) handler();
    }

    protected virtual void OnPurchaseCanceled()
    {
        var handler = PurchaseCanceled;
        if (handler != null) handler();
    }

    private void OnPaymentVerificationStarted(string url)
    {
        var handler = PaymentVerificationStarted;
        if (handler != null) handler(url);
    }

    private void OnPaymentVerificationSucceed(string refID)
    {
        var handler = PaymentVerificationSucceed;
        if (handler != null) handler(refID);
    }

    private void OnPaymentVerificationFailed(string error)
    {
        var handler = PaymentVerificationFailed;
        if (handler != null) handler();
    }

    #endregion
}
