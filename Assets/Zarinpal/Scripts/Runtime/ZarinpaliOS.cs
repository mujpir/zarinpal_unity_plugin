using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
#if UNITY_IOS
public class ZarinpaliOS : MonoBehaviour,IZarinpalPlatform 
{

	private static ZarinpaliOS _instance;
	private static Guid? _transactionID;
	
	public static ZarinpaliOS CreateInstance()
	{
		if (_instance == null)
		{
			if (_instance == null)
			{
				_instance = new GameObject("ZarinpaliOS").AddComponent<ZarinpaliOS>();
				DontDestroyOnLoad(_instance.gameObject);
			}
		}
		return _instance;
	}
	
	public string MerchantID { get; private set; }
	
	/// <summary>
	/// AutoVerifyPurchase is always true on iOS
	/// </summary>

	public bool AutoVerifyPurchase
	{
		get { return true; }
	}

	/// <summary>
	/// Callback is not necessary for iOS
	/// </summary>
	public string Callback
	{
		get { return null; }
	}
	public bool IsInitialized { get; private set; }
	public string PurchasingItemID { get; private set; }
	public void Initialize(string merchantID, bool verifyPurchase, string callbackScheme)
	{
		MerchantID = merchantID;
		_zu_initialize(merchantID);
	}

	public void Purchase(long amount, string productID, string desc)
	{
		_transactionID = Guid.NewGuid();
		PurchasingItemID = productID;
		_zu_startPurchaseFlow((int) amount,productID,desc);
	}

	public event Action StoreInitialized;
	public event Action PurchaseStarted;
	public event Action<string> PurchaseFailedToStart;
	
#pragma warning disable 0067
	/// <summary>
	/// Not supported in iOS . use PaymentVerificationSucceed event instead.
	/// </summary>
	public event Action<string, string> PurchaseSucceed;
	
	public event Action PurchaseFailed;
	/// <summary>
	/// Not supported in iOS . use PurchaseFailed event instead.
	/// </summary>
	public event Action PurchaseCanceled;
	
	/// <summary>
	/// Not supported in iOS .
	/// </summary>
	public event Action<string> PaymentVerificationStarted;
	
	public event Action<string> PaymentVerificationSucceed;
	
	/// <summary>
	/// Not supported in iOS .
	/// </summary>
	public event Action PaymentVerificationFailed;
	
#pragma warning restore 0067 
	
	
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
	    /*
        if (AutoVerifyPurchase)
        {
            var handler = PurchaseSucceed;
            if (handler != null) handler(PurchasingItemID, authority);
        }
        else
        {
            if (_transactionID.HasValue)
            {
                _transactionID = null;
                var handler = PurchaseSucceed;
                if (handler != null) handler(PurchasingItemID, authority);
            }
        }
        */
    }

    private void OnPurchaseFailed(string error)
    {
        if (_transactionID.HasValue)
        {
            _transactionID = null;
            var handler = PurchaseFailed;
            if (handler != null) handler();
        }
    }

    protected virtual void OnPurchaseCanceled()
    {
	    /*
        if (_transactionID.HasValue)
        {
            _transactionID = null;
            var handler = PurchaseCanceled;
            if (handler != null) handler();
        }
        */
    }

    private void OnPaymentVerificationStarted(string url)
    {
        var handler = PaymentVerificationStarted;
        if (handler != null) handler(url);
    }

    private void OnPaymentVerificationSucceed(string refID)
    {
        if (_transactionID.HasValue)
        {
            _transactionID = null;
            var handler = PaymentVerificationSucceed;
            if (handler != null) handler(refID);
        }
    }

    private void OnPaymentVerificationFailed(string error)
    {
	    /*
        if (_transactionID.HasValue)
        {
            _transactionID = null;
            var handler = PaymentVerificationFailed;
            if (handler != null) handler();
        }
        */
    }

    #endregion
    
    
    
    #region C-Extern

    [DllImport("__Internal")]
    private static extern void _zu_initialize(string merchantID);
    
    [DllImport("__Internal")]
    private static extern void _zu_startPurchaseFlow(int amount,string productID,string desc);

    #endregion
}

#endif
