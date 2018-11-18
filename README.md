
This plugin uses Zarinpal Android Sdk that is available here : https://www.zarinpal.com/lab/%D9%86%D9%85%D9%88%D9%86%D9%87-%D9%BE%D8%B1%D8%AF%D8%A7%D8%AE%D8%AA-%D8%AF%D8%B1%D9%88%D9%86-%D8%A8%D8%B1%D9%86%D8%A7%D9%85%D9%87-%D8%A7%D9%86%D8%AF%D8%B1%D9%88%DB%8C%D8%AF/


Import zarinpal unitypackage to your project .

Select Zarinpal/Setting :

MercantID : Set your MerchantID that you get from zarinpal.
Auto Verify : If you want to verify pucrahse in client automatically , then select this option.
Scheme/Host : Choose a unique scheme and host , for example : if you set "gt-club" as scheme and "zarinpal_result" as host ,
Then zarinpal uses "gt-club://zarinpal_result" uri to return the purchase result to your unity game.

Everything is ok . Hit "update manifest and files" to update your manifest and copy required files to your project.

You can now take a look at zarinpal example scene and scripts to see how to use the plugin in your code.You must first call

Zarinpal.Initialize() at the start of your game and then call Zarinpal.Purchase(int amount,string desc) to make a purchase via zarinpal webgate . Furthermore use Callback to be aware when a purchase is succeed or failed . For example subscribe to event Zarinpal.PurchaseSucceed using this line of code :

Zarinpal.PurchaseSucceed+=(string productID,string authority)=>{
  Debug.Log("purchase succeed with authority : "+authority);
}


If you wish to change the way Zarinpal activity work or applying your style to the activity , you can go and do it here :
https://github.com/mujpir/UnityZarinpalPurchaseAndroidStudio
After customizing android part of the plugin ,  then come back here and change the c# code in unity to make plugin works with the way you have changed.

Happy making games.

Mohtaba Pirveisi
Game developer at Darbache Studio.
