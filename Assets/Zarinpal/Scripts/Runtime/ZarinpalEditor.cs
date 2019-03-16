using System;

public class ZarinpalEditor : IZarinpalPlatform
{
    public string MerchantID { get; private set; }
    public bool AutoVerifyPurchase { get; private set; }
    public string Callback { get; private set; }
    public bool IsInitialized { get; private set; }
    public string PurchasingItemID { get; private set; }

    public void Initialize(string merchantID, bool verifyPurchase,string schemeCallback)
    {
        Log("initializing zarinpal with merchant-id : {0} , autoVerify : {1} , callback : {2}",merchantID,verifyPurchase, schemeCallback);
        MerchantID = merchantID;
        AutoVerifyPurchase = verifyPurchase;
        Callback = schemeCallback;
        IsInitialized = true;
        OnStoreInitialized();
    }

    public void Purchase(long amount, string desc,string productID)
    {
        OnPurchaseStarted();
        Log("purchasing amount of : {0} toman , desc : {1} , productID : {2}", amount, desc,productID);
        var authority = "fake_authority_00000000000000000" + Guid.NewGuid();
        PurchasingItemID = productID;
        OnPurchaseSucceed(productID,authority);
        if (AutoVerifyPurchase)
        {
            OnPaymentVerificationStarted(authority);
            OnPaymentVerificationSucceed("fake_ref_id_" + Guid.NewGuid());
        }
    }

    private void Log(string log)
    {
        ZarinpalLog.Log(log);
    }

    private void Log(string log,params object[] args)
    {
        ZarinpalLog.Log(string.Format(log,args));
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

    protected virtual void OnStoreInitialized()
    {
        var handler = StoreInitialized;
        if (handler != null) handler();
    }

    protected virtual void OnPurchaseStarted()
    {
        var handler = PurchaseStarted;
        if (handler != null) handler();
    }

    protected virtual void OnPurchaseFailedToStart(string error)
    {
        var handler = PurchaseFailedToStart;
        if (handler != null) handler(error);
    }

    protected virtual void OnPurchaseSucceed(string productID,string authority)
    {
        var handler = PurchaseSucceed;
        if (handler != null) handler(productID, authority);
    }

    protected virtual void OnPurchaseFailed()
    {
        var handler = PurchaseFailed;
        if (handler != null) handler();
    }

    protected virtual void OnPurchaseCanceled()
    {
        var handler = PurchaseCanceled;
        if (handler != null) handler();
    }

    protected virtual void OnPaymentVerificationStarted(string obj)
    {
        var handler = PaymentVerificationStarted;
        if (handler != null) handler(obj);
    }

    protected virtual void OnPaymentVerificationSucceed(string obj)
    {
        var handler = PaymentVerificationSucceed;
        if (handler != null) handler(obj);
    }

    protected virtual void OnPaymentVerificationFailed()
    {
        var handler = PaymentVerificationFailed;
        if (handler != null) handler();
    }
}
