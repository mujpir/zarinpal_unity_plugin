using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZarinpalConfig : ScriptableObject
{
    [SerializeField] private string _merchantID;
    [SerializeField] private bool _autoVerifyPurchase = true;
    [SerializeField] private string _scheme = "return";
    [SerializeField] private string _host = "zarinpalpayment";
    [SerializeField] private bool _logEnabled = true;
    [SerializeField] private bool _enable = true;

    public string MerchantID
    {
        get { return _merchantID; }
    }

    public bool AutoVerifyPurchase
    {
        get { return _autoVerifyPurchase; }
    }

    public string Scheme
    {
        get { return _scheme; }
    }

    public string Host
    {
        get { return _host; }
    }

    public bool LogEnabled
    {
        get { return _logEnabled; }
    }

    public bool Enable
    {
        get { return _enable; }
    }
}
