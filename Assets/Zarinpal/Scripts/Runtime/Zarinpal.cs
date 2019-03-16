using System;
using UnityEngine;

public class Zarinpal : MonoBehaviour
{
    private static IZarinpalPlatform _platform;
    /// <summary>
    /// Occures when store is successfully initialized
    /// </summary>
    public static event Action StoreInitialized;
    /// <summary>
    /// Occures when store failed to initialized
    /// </summary>
    public static event Action<string> StoreInitializeFailed;
    /// <summary>
    /// Occures when zarinpal initiaate a purchase
    /// </summary>
    public static event Action PurchaseStarted;
    /// <summary>
    /// Occures when zarinpal can not start a purchase flow.It may be caused by invalid merchant id or unavailabilty of zarinpal service.
    /// </summary>
    public static event Action<string> PurchaseFailedToStart;
    /// <summary>
    /// Occures when a purchase completed by user but still not verified.
    /// </summary>
    public static event Action<string,string> PurchaseSucceed;
    /// <summary>
    /// Occures when a purchase failed.
    /// </summary>
    public static event Action PurchaseFailed;
    /// <summary>
    /// Occures when a purchase canceled by user.
    /// </summary>
    public static event Action PurchaseCanceled;
    /// <summary>
    /// Occures when zarinpal started to verify purchase
    /// </summary>
    public static event Action<string> PaymentVerificationStarted;
    /// <summary>
    /// Occures when payment verified by zarinpal and would be valid.You can award your user here
    /// </summary>
    public static event Action<string> PaymentVerificationSucceed;
    /// <summary>
    /// Occures when payment verified by zarinpal and would NOT be valid or verification failed at first.
    /// </summary>
    public static event Action PaymentVerificationFailed;

    public static bool Initialized
    {
        get
        {
            if (_platform == null)
            {
                return false;
            }
            return _platform.IsInitialized;
        }
    }


    public static string PurchasingItemID
    {
        get
        {
            if (_platform == null)
            {
                return null;
            }
            return _platform.PurchasingItemID;
        }
    }

    /// <summary>
    /// Initialize Zarinpal . Call this once is start up of your game.
    /// </summary>
    public static void Initialize()
    {
        if (_platform != null)
        {
            if (Initialized)
            {
                var message = "Zarinpal is already initialized.Please make sure you call 'Initialize' once.";
                OnStoreInitializeFailed(message);
                ZarinpalLog.LogWarning(message);
            }
            else
            {
                var message = "Platform has been created but not initialized . There may be an error. Please see logs for more details";
                OnStoreInitializeFailed(message);
                ZarinpalLog.LogError(message);
            }
            return;
        }
        
#if UNITY_EDITOR
        _platform = new ZarinpalEditor();
#elif UNITY_IOS
        _platform = ZarinpaliOS.CreateInstance();
#elif UNITY_ANDROID
        _platform = ZarinpalAndroid.CreateInstance();
#endif
        
        //Subscribing events
        _platform.StoreInitialized += OnStoreInitialized;
        _platform.PurchaseStarted += OnPurchaseStarted;
        _platform.PurchaseFailedToStart += OnPurchaseFailedToStart;
        _platform.PurchaseSucceed += OnPurchaseSucceed;
        _platform.PurchaseFailed += OnPurchaseFailed;
        _platform.PurchaseCanceled += OnPurchaseCanceled;
        _platform.PaymentVerificationStarted += OnPaymentVerificationStarted;
        _platform.PaymentVerificationSucceed += OnPaymentVerificationSucceed;
        _platform.PaymentVerificationFailed += OnPaymentVerificationFailed;
        
        
        if (Initialized)
        {
            var message = "Zarinpal is already initialized.Please make sure you call 'Initialize' once.";
            OnStoreInitializeFailed(message);
            ZarinpalLog.LogWarning(message);
            return;
        }

        var setting = Resources.Load<ZarinpalConfig>("ZarinpalSetting");

        if (setting == null)
        {
            var message =
                "Could not find zarinpal config file.Make sure you have setup zarinpal setting in Zarinpal/Setting";
            OnStoreInitializeFailed(message);
            ZarinpalLog.LogWarning(message);
            return;
        }

        if (string.IsNullOrEmpty(setting.MerchantID) || setting.MerchantID== "MY_ZARINPAL_MERCHANT_ID")
        {
            var message = "Invalid MerchantID.Please go to menu : Zarinpal/Setting to set a valid merchant id";
            OnStoreInitializeFailed(message);
            ZarinpalLog.LogWarning(message);
            return;
        }

        var scheme = setting.Scheme;
        var host = setting.Host;
        
#if !UNITY_EDITOR && UNITY_ANDROID
        if (string.IsNullOrEmpty(setting.Scheme) || string.IsNullOrEmpty(setting.Host)
            || setting.Scheme=="MY_SCHEME" || setting.Host=="MY_HOST")
        {
            var message = "Scheme or Host Can not be null or Empty.Please go to menu : Zarinpal/Setting to set a valid Scheme and Host";
            OnStoreInitializeFailed(message);
            ZarinpalLog.LogWarning(message);
            return;
        }
#else
        scheme = string.Empty;
        host = string.Empty;
#endif

        if (_platform==null)
        {
            var message = "Platform is not supported";
            OnStoreInitializeFailed(message);
            ZarinpalLog.LogError(message);
            return;
        }

        _platform.Initialize(setting.MerchantID, setting.AutoVerifyPurchase, string.Format("{0}://{1}", scheme, host));
    }

    /// <summary>
    /// Start a zarinpal purchase
    /// </summary>
    /// <param name="amount">your product/service price in toman</param>
    /// <param name="desc">payment description.please note it can not be null or empty</param>
    /// <param name="productID">the id of product you are purchasing</param>
    public static void Purchase(long amount, string desc,string productID="na")
    {
        if (amount < 100)
        {
            var message = "Purchase is not valid.Amount can not be less than 100 toman.";
            OnPurchaseFailedToStart(message);
            ZarinpalLog.LogError(message);
            return;
        }

        if (string.IsNullOrEmpty(desc))
        {
            var message = "Purchase is not valid.Description can not be null or empty .Please provide a valid description";
            OnPurchaseFailedToStart(message);
            ZarinpalLog.LogError(message);
            return;
        }

        if (_platform == null || !_platform.IsInitialized)
        {
            var message = "Purchase is not valid.Platform is not supported or is not initialized yet.Please Call initialize first";
            OnPurchaseFailedToStart(message);
            ZarinpalLog.LogError(message);
            return;
        }

        if (string.IsNullOrEmpty(productID))
        {
            productID = "unknown product";
        }

        _platform.Purchase(amount, desc, productID);
    }


    #region Callbacks
    protected static void OnStoreInitialized()
    {
        var handler = StoreInitialized;
        if (handler != null) handler();
    }

    private static void OnStoreInitializeFailed(string error)
    {
        var handler = StoreInitializeFailed;
        if (handler != null) handler(error);
    }

    protected static void OnPurchaseStarted()
    {
        var handler = PurchaseStarted;
        if (handler != null) handler();
    }

    protected static void OnPurchaseFailedToStart(string message)
    {
        var handler = PurchaseFailedToStart;
        if (handler != null) handler(message);
    }

    protected static void OnPurchaseSucceed(string productID, string authority)
    {
        var handler = PurchaseSucceed;
        if (handler != null) handler(productID, authority);
    }

    protected static void OnPurchaseFailed()
    {
        var handler = PurchaseFailed;
        if (handler != null) handler();
    }

    protected static void OnPurchaseCanceled()
    {
        var handler = PurchaseCanceled;
        if (handler != null) handler();
    }

    protected static void OnPaymentVerificationStarted(string obj)
    {
        var handler = PaymentVerificationStarted;
        if (handler != null) handler(obj);
    }

    protected static void OnPaymentVerificationSucceed(string obj)
    {
        var handler = PaymentVerificationSucceed;
        if (handler != null) handler(obj);
    }

    protected static void OnPaymentVerificationFailed()
    {
        var handler = PaymentVerificationFailed;
        if (handler != null) handler();
    }

    #endregion
}
