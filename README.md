Verion 1.2 beta changelogs
  - Add support iOS platform : you can now make purchasing on iOS Devices version 8 or higher.
  - Fix error when copying android manifest to plugins folder in Mac OS.
  - Fix a few bugs


How to you zarinpal in unity:

1 .  Import zarinpal_unity.unitypackage 1.2 to your project .

2 .  After importing , click on Zarinpal/Setting menu to setup the plugin.

MercantID : Set your MerchantID that you get from zarinpal.
Auto Verify : If you want to verify pucrahse in client automatically , then select this option.
Scheme/Host : Choose a unique scheme and host , for example : if you set "gt-club" as scheme and "zarinpal_result" as host ,
Then zarinpal uses "gt-club://zarinpal_result" uri to return the purchase result to your unity game.

3 . Everything is ok . Hit "update manifest and files" to update your manifest and copy required files to your project.

4 . Use Zarinpal.Initialize() method at the start of your script to makes zarinpal initialized.

You can now take a look at zarinpal example scene and scripts to see how to use the plugin in your code.
You must first call Zarinpal.Initialize() at the start of your game . Please note to call it once.


5 . Use Zarinpal.Purchase(int amount,string desc) to make a purchase . 
then call Zarinpal.Purchase(int amount,string desc) to make a purchase via zarinpal webgate .

6 . Use Callback to be aware when a purchase is succeed or failed . For example subscribe to event Zarinpal.PurchaseSucceed using this line of code :

Zarinpal.PurchaseSucceed+=(string productID,string authority)=>{
  Debug.Log("purchase succeed with authority : "+authority);
}


On Android : If you wish to change the way Zarinpal activity work or applying your style to the activity , you can go and do it here :
https://github.com/mujpir/UnityZarinpalPurchaseAndroidStudio
After customizing android part of the plugin ,  then come back here and change the c# code in unity to make plugin works with the way you have changed.

Happy making games.

Mojtaba Pirveisi
Game developer at Darbache Studio.
