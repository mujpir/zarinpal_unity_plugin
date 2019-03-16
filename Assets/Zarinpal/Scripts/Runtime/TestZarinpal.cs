using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestZarinpal : MonoBehaviour
{
    [SerializeField] private Text m_text;
    private AndroidJavaClass zarinpalJavaClass;
    private AndroidJavaObject mZarinpalJavaObject;

    void Start()
    {
    }

    public void InitializePurchase()
    {
        zarinpalJavaClass = new AndroidJavaClass("com.kingcodestudio.unityzarinpaliab.ZarinpalUnityFragment");
        zarinpalJavaClass.CallStatic("start", gameObject.name);
        mZarinpalJavaObject = zarinpalJavaClass.GetStatic<AndroidJavaObject>("instance");



        m_text.text = "plugin initialized ";
        ZarinpalLog.Log("plugin initialized");
    }

    public void RequestPayment()
    {
        mZarinpalJavaObject.Call("purchase", 100L,"pardakht");
    }

    public void VerifyPurchase()
    {
        mZarinpalJavaObject.Call("verifyPurchase");
    }

    public void OnErrorOnPaymentRequest(string error)
    {
        m_text.text = "an error occured in requesting for payment : " + error;
        ZarinpalLog.Log("an error occured in requesting for payment : " + error);
    }

    public void OnPaymentProcessStarted()
    {
        m_text.text = "payment process started ";
        ZarinpalLog.Log("payment process started");
    }

    public void OnPaymentVerificationSucceed(string refID)
    {
        m_text.text = "payment verification succeed refid :"+ refID;
        ZarinpalLog.Log("payment verification succeed refid :" + refID);
    }

    public void OnPaymentVerificationFailed()
    {
        m_text.text = "payment verification failed ";
        ZarinpalLog.Log("payment verification failed");
    }
}
