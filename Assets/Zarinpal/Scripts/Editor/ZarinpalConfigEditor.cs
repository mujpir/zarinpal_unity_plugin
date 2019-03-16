using System;
using System.IO;
using Soomla;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ZarinpalConfig))]
public class ZarinpalConfigEditor : Editor
{
    private bool m_changed = false;
    private bool _mFoldoutAndroid;
    private bool _mFoldoutIOS;

    public override void OnInspectorGUI()
    {
        var color = GUI.color;
        var _enableProp = serializedObject.FindProperty("_enable");
        var merchantIDProp = serializedObject.FindProperty("_merchantID");
        var autoVerifyProp = serializedObject.FindProperty("_autoVerifyPurchase");
        var _schemeProp = serializedObject.FindProperty("_scheme");
        var _hostProp = serializedObject.FindProperty("_host");
        var logEnabled = serializedObject.FindProperty("_logEnabled");


        EditorGUI.BeginChangeCheck();
        EditorGUILayout.LabelField("Zarinpal Unity Setting");
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(_enableProp);
        EditorGUILayout.PropertyField(merchantIDProp);
        EditorGUILayout.PropertyField(autoVerifyProp);
        EditorGUILayout.Space();
        
        var changed = EditorGUI.EndChangeCheck();

        
        _mFoldoutAndroid = EditorGUILayout.Foldout(_mFoldoutAndroid, "Android setting");
        if (_mFoldoutAndroid)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField(
                "Provide a unique scheme and host for android\nOthewise your app may conflicts with other apps.\nZarinpal use this scheme and host to identify your app\nand return the purchase result. ",
                GUILayout.Height(60));
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_schemeProp);
            EditorGUILayout.PropertyField(_hostProp);
            
            changed = EditorGUI.EndChangeCheck();

            EditorGUILayout.LabelField(string.Format("{0}://{1}/?Authority=<authority>&Status=OK",
                _schemeProp.stringValue,
                _hostProp.stringValue));
            EditorGUI.indentLevel--;
        }
        
        EditorGUILayout.Space();
        

        
        _mFoldoutIOS = EditorGUILayout.Foldout(_mFoldoutIOS, "IOS setting");
        if (_mFoldoutIOS)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("IOS Sdk version : " +PlayerSettings.iOS.targetOSVersionString);
            try
            {
                if (Convert.ToSingle(PlayerSettings.iOS.targetOSVersionString) < 8F)
                {
                    color = GUI.color;
                    GUI.color = Color.red;
                    EditorGUILayout.LabelField("Zarinpal need sdk version 8.0 or higher");

                    GUI.color = Color.green;
                    if (GUILayout.Button("Set IOS SDK to 8.0"))
                    {
                        PlayerSettings.iOS.targetOSVersionString = "8.0";
                    }

                    GUI.color = color;
                }
            }
            catch (Exception e)
            {
                
            }

            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(logEnabled);

        if (changed)
        {
            m_changed = true;
        }

        color = GUI.color;
        if (m_changed)
        {
            EditorGUILayout.Space();
            GUI.color = Color.red;
            EditorGUILayout.LabelField("Hit Update to make changes affected.");
            GUI.color = color;
        }

        color = GUI.color;
        if (m_changed)
        {
            GUI.color = Color.green;
        }
        if (GUILayout.Button("Update Manifest & Files"))
        {
            var pluginDirectoryAndroid = Path.Combine(Application.dataPath, "Plugins/Android");
            if (!Directory.Exists(pluginDirectoryAndroid))
            {
                Directory.CreateDirectory(pluginDirectoryAndroid);
            }
            
            var pluginDirectoryIOS = Path.Combine(Application.dataPath, "Plugins/IOS");
            if (!Directory.Exists(pluginDirectoryIOS))
            {
                Directory.CreateDirectory(pluginDirectoryIOS);
            }
            
            handleZarinpalJars(!_enableProp.boolValue);
            handleZarinpalIOS(!_enableProp.boolValue);
            ZarinpalManifestTools.GenerateManifest();
            AssetDatabase.Refresh();
            m_changed = false;
        }

        GUI.color = color;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Author : Mojtaba Pirveisi");
        EditorGUILayout.LabelField("@ 2018 Darbache-Studio");

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }


    static void handleZarinpalJars(bool remove)
    {
        try
        {
            if (remove)
            {
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/Android/library-1.0.19.jar");
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/Android/library-1.0.19.jar.meta");
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/Android/purchase-0.0.8-beta.aar");
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/Android/purchase-0.0.8-beta.aar.meta");
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/Android/UnityZarinpalPurchase.aar");
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/Android/UnityZarinpalPurchase.aar.meta");
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/Android/constraint-layout-1.0.0-alpha7.aar");
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/Android/constraint-layout-1.0.0-alpha7.aar.meta");
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/Android/constraint-layout-solver-1.0.0-alpha7.jar");
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/Android/constraint-layout-solver-1.0.0-alpha7.jar.meta");

            }
            else
            {

                string bpRootPath = Application.dataPath +
                                    "/Zarinpal/Templates/";
                FileUtil.CopyFileOrDirectory(bpRootPath + "library-1.0.19.jar",
                    Application.dataPath + "/Plugins/Android/library-1.0.19.jar");
                FileUtil.CopyFileOrDirectory(bpRootPath + "purchase-0.0.8-beta.aar",
                    Application.dataPath + "/Plugins/Android/purchase-0.0.8-beta.aar");
                FileUtil.CopyFileOrDirectory(bpRootPath + "UnityZarinpalPurchase.aar",
                    Application.dataPath + "/Plugins/Android/UnityZarinpalPurchase.aar");
                FileUtil.CopyFileOrDirectory(bpRootPath + "constraint-layout-1.0.0-alpha7.aar",
                    Application.dataPath + "/Plugins/Android/constraint-layout-1.0.0-alpha7.aar");
                FileUtil.CopyFileOrDirectory(bpRootPath + "constraint-layout-solver-1.0.0-alpha7.jar",
                    Application.dataPath + "/Plugins/Android/constraint-layout-solver-1.0.0-alpha7.jar");
            }
        }
        catch
        {
        }
    }
    
    
    
        static void handleZarinpalIOS(bool remove)
    {
        try
        {
            if (remove)
            {
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/IOS/HttpRequest.swift");
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/IOS/HttpRequest.swift.meta");
                
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/IOS/PaymentViewController.swift");
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/IOS/PaymentViewController.swift.meta");
                
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/IOS/URLs.swift");
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/IOS/URLs.swift.meta");
                
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/IOS/ZarinPal.swift");
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/IOS/ZarinPal.swift.meta");
                
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/IOS/ZarinPalPaymentDelegate.swift");
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/IOS/ZarinPalPaymentDelegate.swift.meta");
                
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/IOS/ZarinPalSDKPayment.h");
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/IOS/ZarinPalSDKPayment.h.meta");
                
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/IOS/ZarinpalUnity.swift");
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/IOS/ZarinpalUnity.swift.meta");
                
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/IOS/ZarinpalUnityBridge.mm");
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/IOS/ZarinpalUnityBridge.mm.meta");
                
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/IOS/ZarinpalUnityPlugin-Bridging-Header.h");
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/IOS/ZarinpalUnityPlugin-Bridging-Header.h.meta");
                
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/IOS/ZarinpalUnityWrapper.swift");
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/IOS/ZarinpalUnityWrapper.swift.meta");
                

            }
            else
            {

                string bpRootPath = Application.dataPath +
                                    "/Zarinpal/Templates/IOS/";
                
                FileUtil.CopyFileOrDirectory(bpRootPath + "HttpRequest.swift",
                    Application.dataPath + "/Plugins/IOS/HttpRequest.swift");
                
                FileUtil.CopyFileOrDirectory(bpRootPath + "PaymentViewController.swift",
                    Application.dataPath + "/Plugins/IOS/PaymentViewController.swift");
                
                FileUtil.CopyFileOrDirectory(bpRootPath + "URLs.swift",
                    Application.dataPath + "/Plugins/IOS/URLs.swift");
                
                FileUtil.CopyFileOrDirectory(bpRootPath + "ZarinPal.swift",
                    Application.dataPath + "/Plugins/IOS/ZarinPal.swift");
                
                FileUtil.CopyFileOrDirectory(bpRootPath + "ZarinPalPaymentDelegate.swift",
                    Application.dataPath + "/Plugins/IOS/ZarinPalPaymentDelegate.swift");
                
                FileUtil.CopyFileOrDirectory(bpRootPath + "ZarinPalSDKPayment.h",
                    Application.dataPath + "/Plugins/IOS/ZarinPalSDKPayment.h");
                
                FileUtil.CopyFileOrDirectory(bpRootPath + "ZarinpalUnity.swift",
                    Application.dataPath + "/Plugins/IOS/ZarinpalUnity.swift");
                
                FileUtil.CopyFileOrDirectory(bpRootPath + "ZarinpalUnityBridge.mm",
                    Application.dataPath + "/Plugins/IOS/ZarinpalUnityBridge.mm");
                
                FileUtil.CopyFileOrDirectory(bpRootPath + "ZarinpalUnityPlugin-Bridging-Header.h",
                    Application.dataPath + "/Plugins/IOS/ZarinpalUnityPlugin-Bridging-Header.h");
                
                FileUtil.CopyFileOrDirectory(bpRootPath + "ZarinpalUnityWrapper.swift",
                    Application.dataPath + "/Plugins/IOS/ZarinpalUnityWrapper.swift");
            }
        }
        catch
        {
        }
    }
        

    [MenuItem("Zarinpal/Setting")]
    static void ShowConfig()
    {
        string path = "Assets/Zarinpal/Resources/ZarinpalSetting.asset";
        var config = AssetDatabase.LoadAssetAtPath<ZarinpalConfig>(path);
        if (config == null)
        {
            config = ZarinpalConfig.CreateInstance<ZarinpalConfig>();
            AssetDatabase.CreateAsset(config, path);
            AssetDatabase.SaveAssets();
        }

        Selection.activeObject = config;
    }
}
