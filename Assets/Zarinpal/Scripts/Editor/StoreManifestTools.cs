/// Copyright (C) 2012-2014 Soomla Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///      http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;

namespace Soomla.Store
{
#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	public class StoreManifestTools : IManifestTools
    {
#if UNITY_EDITOR
		static StoreManifestTools instance = new StoreManifestTools();
        private static ZarinpalConfig setting;

        static StoreManifestTools()
		{
			ZarinpalManifestTools.ManTools.Add(instance);
		}

		public void ClearManifest(){
			RemoveZarinpalBPFromManifest();
        }
		public void UpdateManifest() {
			HandleZarinpalBPManifest ();
        }

		public void HandleZarinpalBPManifest()
		{
			if (StoreSettings.Enable) {
				AddZarinpalBPToManifest();
			} else {
				RemoveZarinpalBPFromManifest();
			}
		}

		private void AddZarinpalBPToManifest(){
			ZarinpalManifestTools.SetPermission("android.permission.INTERNET");
			ZarinpalManifestTools.AddActivity("com.kingcodestudio.unityzarinpaliab.ZarinpalActivity",new Dictionary<string, string>()
			{
			    {"theme","@android:style/Theme.DeviceDefault.Light.Dialog.NoActionBar.MinWidth" }
			});
		    XmlElement activityElement = ZarinpalManifestTools.FindElementWithTagAndName("activity",
		        "com.kingcodestudio.unityzarinpaliab.ZarinpalActivity");
		    XmlElement intentElement = ZarinpalManifestTools.AppendElementIfMissing("intent-filter", null, null, activityElement);
		    ZarinpalManifestTools.AppendElementIfMissing("action", "android.intent.action.VIEW",
		        new Dictionary<string, string>(),intentElement);
		    ZarinpalManifestTools.AppendElementIfMissing("category", "android.intent.category.DEFAULT",
		        new Dictionary<string, string>(), intentElement);


		    ZarinpalManifestTools.AddActivity("com.kingcodestudio.unityzarinpaliab.ZarinpalResultActivity", new Dictionary<string, string>()
		    {
		        {"theme","@android:style/Theme.DeviceDefault.Light.Dialog.NoActionBar.MinWidth" }
		    });
            XmlElement activityResultElement = ZarinpalManifestTools.FindElementWithTagAndName("activity",
		        "com.kingcodestudio.unityzarinpaliab.ZarinpalResultActivity");
		    XmlElement intentResultElement = ZarinpalManifestTools.AppendElementIfMissing("intent-filter", null, null, activityResultElement);
		    ZarinpalManifestTools.AppendElementIfMissing("action", "android.intent.action.VIEW",
		        new Dictionary<string, string>(), intentResultElement);
		    ZarinpalManifestTools.AppendElementIfMissing("category", "android.intent.category.DEFAULT",
		        new Dictionary<string, string>(), intentResultElement);
		    ZarinpalManifestTools.AppendElementIfMissing("category", "android.intent.category.BROWSABLE",
		        new Dictionary<string, string>(), intentResultElement);
		    var scheme = StoreSettings.Scheme;
		    var host = StoreSettings.Host;
		    ZarinpalManifestTools.RemoveElement("data", null, intentResultElement);
		    ZarinpalManifestTools.AppendElementIfMissing("data", null,
		        new Dictionary<string, string>()
		        {
		            {"scheme",scheme },
		            {"host",host },
		        }, intentResultElement);
        }

		private void RemoveZarinpalBPFromManifest(){
            // removing Iab Activity
            if (!StoreSettings.Enable)
            {
                ZarinpalManifestTools.RemoveActivity("com.kingcodestudio.unityzarinpaliab.ZarinpalActivity");
                ZarinpalManifestTools.RemoveActivity("com.kingcodestudio.unityzarinpaliab.ZarinpalResultActivity");
            }
		}

        public ZarinpalConfig StoreSettings
        {
            get
            {
                if(setting==null)
                    setting = AssetDatabase.LoadAssetAtPath<ZarinpalConfig>("Assets/Zarinpal/Resources/ZarinpalSetting.asset");
                return setting;
            }
        }
#endif
    }
}