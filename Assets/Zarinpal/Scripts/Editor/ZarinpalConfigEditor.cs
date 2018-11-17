using Soomla;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ZarinpalConfig))]
public class ZarinpalConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var _enableProp = serializedObject.FindProperty("_enable");
        var merchantIDProp = serializedObject.FindProperty("_merchantID");
        var autoVerifyProp = serializedObject.FindProperty("_autoVerifyPurchase");
        var _schemeProp = serializedObject.FindProperty("_scheme");
        var _hostProp = serializedObject.FindProperty("_host");
        var logEnabled = serializedObject.FindProperty("_logEnabled");


        EditorGUILayout.LabelField("Zarinpal Unity Setting");
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(_enableProp);
        EditorGUILayout.PropertyField(merchantIDProp);
        EditorGUILayout.PropertyField(autoVerifyProp);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Provide a unique scheme and host\nOthewise your app may conflicts with other apps.\nZarinpal use this scheme and host to identify your app\nand return the purchase result. ",GUILayout.Height(60));
        EditorGUILayout.PropertyField(_schemeProp);
        EditorGUILayout.PropertyField(_hostProp);
        EditorGUILayout.LabelField(string.Format("{0}://{1}/?Authority=<authority>&Status=OK", _schemeProp.stringValue,
            _hostProp.stringValue));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(logEnabled);

        if (GUILayout.Button("Update Manifest & Files"))
        {
            handleZarinpalJars(!_enableProp.boolValue);
            ZarinpalManifestTools.GenerateManifest();
        }

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
                                               "/Plugins/Android/purchase-0.0.3-beta.aar");
                FileUtil.DeleteFileOrDirectory(Application.dataPath +
                                               "/Plugins/Android/purchase-0.0.3-beta.aar.meta");
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
                FileUtil.CopyFileOrDirectory(bpRootPath + "purchase-0.0.3-beta.aar",
                    Application.dataPath + "/Plugins/Android/purchase-0.0.3-beta.aar");
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
