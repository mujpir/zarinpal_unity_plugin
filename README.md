<h1> Zarinpal Unity plugin </h1>


<h4>Verion 1.2 beta changelogs </h4>

  - Add support iOS platform : you can now make purchasing on iOS Devices version 8 or higher.
  - Fix error when copying android manifest to plugins folder in Mac OS.
  - Fix a few bugs


<h3>How to make zarinpal work in unity:</h3>

<h4>1. Import the plugin into your project.</h4>
Import zarinpal_unity.unitypackage 1.2 to your project.

<h4>2. Change zarinpal setting.</h4>

After importing , click on Zarinpal/Setting menu to setup the plugin.
Settings you can change :
- MercantID : Set your MerchantID that you get from zarinpal.
- Auto Verify : If you want to verify pucrahse in client automatically , then select this option.
- Scheme/Host : Choose a unique scheme and host.
for example : if you set "gt-club" as scheme and "zarinpal_result" as host ,
Then zarinpal uses "gt-club://zarinpal_result" uri to return the purchase result to your unity game.

<h4>3. Update manifest and files.</h4>
Click on "update manifest and files" button to update your manifest and copy required files into your project.

<h4>4. Initialize the plugin fisrt.
Use method <pre> Zarinpal.Initialize() </pre> at the start of your game to makes zarinpal initialized.
Please note to call it once.
You can take a look at zarinpal example scene and scripts to see how to use the plugin in your code..

<h4>5. make your purchase.</h4>
Use <pre> Zarinpal.Purchase(int amount,string productID , string desc) </pre> to make a purchase .

<h4>6. Use callbacks to get purchase result.</h4>
Use Callback to be aware when a purchase is succeed or failed
For example subscribe to event Zarinpal.PurchaseSucceed using this line of code :

<pre>
<code>
Zarinpal.PurchaseSucceed+=(string productID,string authority)=>
{
  Debug.Log("purchase succeed with authority : "+authority);
}
</code>
</pre>


On Android : If you wish to change the way Zarinpal activity work or applying your style to the activity , you can go and do it here :
https://github.com/mujpir/UnityZarinpalPurchaseAndroidStudio
After customizing android part of the plugin ,  then come back here and change the c# code in unity to make plugin works with the way you have changed.

Happy making games.

Mojtaba Pirveisi
Game developer at Darbache Studio.
