using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class SdkdsPurchaseUtils : MonoBehaviour
{
    public static SdkdsPurchaseUtils Instance;
	public struct SdkdsPurchaseData
    {
		public string url;
		public string userID;
		public string platformType;
		public string productIdJsonArray;
		public string gameID;
		public string token;
		public string scene;
	}
	public SdkdsPurchaseData purchaseData;

	//TODO Your application's public key, encoded in base64. This is used for verification of purchase signatures. You can find your app's base64-encoded  public key in your application's page on Google Play Developer Console. Note that this is NOT your "developer public key". 
	public static string base64PublicKey = "your publickey";
    //TODO If isDebugMode=true��the log of java can be shown
    public static Boolean isDebugMode = true;

    public static bool isUseStaticInit = false;

#if UNITY_ANDROID
    private static AndroidJavaObject gpPaySdkUtils_unity;

    public static class SingletonHolder
    {
        public static AndroidJavaObject instance_context;
        static SingletonHolder()
        {
            using (AndroidJavaClass cls_UnityPlayer = SdkdsPurchaseUtils.LoadJavaClass("com.unity3d.player.UnityPlayer"))
            {
                instance_context = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            }
        }
    }
#endif

#if UNITY_IOS || UNITY_IPHONE
	[DllImport("__Internal")]
	public static extern void SDKDSRequestProducts(string productIds);

	[DllImport("__Internal")]
	public static extern void SDKDSBuyProduct(string userID, string platformType, string productID);

	[DllImport("__Internal")]
	public static extern void SDKDSInitPurchase(string url, string userID, string platformType, string productIds, string gameid, string token);

	[DllImport("__Internal")]
	public static extern void SDKDSInitPurchase(string url, string userID, string platformType, string productIds, string gameid, string token, string scene);

	[DllImport("__Internal")]
	public static extern void SDKDSRemedyTransaction();

	[DllImport("__Internal")]
	public static extern void SDKDSRestoreCompletedTransactions(string productIDsJson);

	[DllImport("__Internal")]
	public static extern void SDKDSRequestNonConsumableProductStatusWithProductIDs(string productIDsJson);

	[DllImport("__Internal")]
	public static extern void SDKDSInitPurchaseUtils();

    [DllImport("__Internal")]
    public static extern void SDKDSSetPurchaseUserID(string userID);

    [DllImport("__Internal")]
    public static extern void SDKDSSetPurchaseToken(string token);

    [DllImport("__Internal")]
    public static extern void SDKDSSetPurchaseScene(string scene);
	
	[DllImport("__Internal")]
    public static extern bool SDKDSCanRemedyTransaction();
#elif UNITY_ANDROID

#endif

    public static event Action<string> productListReceivedEvent;
	public static event Action<string> purchaseCompleteEvent;
	public static event Action<string> restoreCompleteEvent;
	public static event Action<string> requestNonConsumableProductStatusCompleteEvent;
	public static void Initialize()
	{
		if (Instance == null) 
		{
			Debug.Log ("Initialize SdkdsPurchase");
			GameObject gameobject = GameObject.Find ("PurchaseUtil");
			if (gameobject == null) {
				gameobject = new GameObject ("PurchaseUtil");
			}
			if (gameobject != null) {
				gameobject.AddComponent<SdkdsPurchaseUtils> ();	
			} 
		}
	}

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Init();
			DontDestroyOnLoad (gameObject);
        }
    }

	void Init()
	{
        Debug.Log("SdkdsPurchaseUtils.Init()");

        // Here is the sample code, you can do this
        /*this.purchaseData = new SdkdsPurchaseData();
		purchaseData.url = "";
		purchaseData.userID = "";
		purchaseData.productIdJsonArray = "";
		purchaseData.platformType = "0";
		purchaseData.gameID = "";
		purchaseData.token = "";
		purchaseData.scene = null;
#if (UNITY_EDITOR || DISBLE_PLATFORM)
#elif UNITY_ANDROID
        Debug.Log("SdkdsPurchaseUtils Init()");
        //init android java class instance
        gpPaySdkUtils_unity = SdkdsPurchaseUtils.LoadJavaClass("com.sdkds.gppay.PaySdk");
		purchaseData.url = "your url";
		purchaseData.userID = "";
		purchaseData.productIdJsonArray = "[\"60diamonds\",\"180diamonds\",\"600diamonds\"]";   //TODO replace to your product ids
		purchaseData.gameID = "";
		purchaseData.token = "";
#elif UNITY_IPHONE || UNITY_IOS
		purchaseData.url = "your url"; 
		purchaseData.userID = "";
		purchaseData.productIdJsonArray = "[\"RS3KEYS\",\"RS6SHIELDS\",\"RS8HEARTS\",\"RS_ROLE_SNOWBALL\"]";   //TODO replace to your product ids
		purchaseData.gameID = "";
		purchaseData.token = "";
#endif
        // set debug
        setDebug(isDebugMode);

		// init Purchase
		initPurchase(purchaseData.url, purchaseData.userID, purchaseData.platformType, purchaseData.productIdJsonArray, purchaseData.gameID, purchaseData.token);*/
    }

    public void setDebug(bool value)
    {
#if (UNITY_EDITOR || DISBLE_PLATFORM)
#elif UNITY_ANDROID
        gpPaySdkUtils_unity.CallStatic("setDebug", value);
#elif UNITY_IPHONE || UNITY_IOS
#else

#endif
    }

    /// <summary>
    /// purchase initialization
    /// </summary>
    /// <param name="url">host url for server verify</param>
    /// <param name="userID">user id</param>
    /// <param name="platformType">platform type for server verify, optional param, default value: "0"</param>
    /// <param name="productIdJsonArray">product ids, json array format</param>
    /// <param name="gameID">game id</param>
    /// <param name="token">token, optional param, default value: ""</param>
	public void initPurchase(string url, string userID, string platformType, string productIdJsonArray, string gameID, string token, string scene = null)
	{
		if (isUseStaticInit == true) {
			return;
		}
        Debug.Log("into initPurchase");
        Debug.Log("url " + url);
        Debug.Log("userID " + userID);
        Debug.Log("platformType " + platformType);
        Debug.Log("productIdJsonArray " + productIdJsonArray);
        Debug.Log("gameID " + gameID);
        Debug.Log("token " + token);
        Debug.Log("scene " + scene);
        Debug.Log("base64PublicKey " + base64PublicKey);

		purchaseData.url = url;
		purchaseData.userID = userID;
		purchaseData.platformType = platformType;
		purchaseData.productIdJsonArray = productIdJsonArray;
		purchaseData.gameID = gameID;
		purchaseData.token = token;
		purchaseData.scene = scene;
#if (UNITY_EDITOR || DISBLE_PLATFORM)

#elif UNITY_ANDROID
        //game object that can receive unity msg
        string unity_msg_receiver = "PurchaseUtil";       
        gpPaySdkUtils_unity.CallStatic("SdkdsInitPurchase", SingletonHolder.instance_context, base64PublicKey, url, gameID, userID, platformType, productIdJsonArray, token, unity_msg_receiver);
#elif UNITY_IOS || UNITY_IPHONE
		SDKDSInitPurchaseUtils ();
		SDKDSInitPurchase(url, userID, platformType, productIdJsonArray, gameID, token);
#else

#endif
    }

    public static void InitPurchaseSDK(string url, string userID, string platformType, string productIdJsonArray, string gameID, string token, string scene = null)
    {
    	Debug.Log("into InitPurchaseSDK");
    	Debug.Log("url " + url);
    	Debug.Log("userID " + userID);
    	Debug.Log("platformType " + platformType);
    	Debug.Log("productIdJsonArray " + productIdJsonArray);
    	Debug.Log("gameID " + gameID);
    	Debug.Log("token " + token);
    	Debug.Log("scene " + scene);
        Debug.Log("base64PublicKey " + base64PublicKey);

        isUseStaticInit = true;

		if (Instance == null)
        {    
            Debug.Log ("Initialize SdkdsPurchase");
			GameObject gameobject = GameObject.Find ("PurchaseUtil");
			if (gameobject == null) {
				gameobject = new GameObject ("PurchaseUtil");
			}
			if (gameobject != null) {
				gameobject.AddComponent<SdkdsPurchaseUtils> ();	
			} 
		}

#if (UNITY_EDITOR || DISBLE_PLATFORM)

#elif UNITY_ANDROID
		gpPaySdkUtils_unity = SdkdsPurchaseUtils.LoadJavaClass("com.sdkds.gppay.PaySdk");
        // set debug
        Instance.setDebug(isDebugMode);
        //game object that can receive unity msg
        string unity_msg_receiver = "PurchaseUtil";

        if(scene == null)
        {
        	gpPaySdkUtils_unity.CallStatic("SdkdsInitPurchase", SingletonHolder.instance_context, base64PublicKey, url, gameID, userID, platformType, productIdJsonArray, token, unity_msg_receiver);
    	}
    	else
    	{
    		gpPaySdkUtils_unity.CallStatic("SdkdsInitPurchase", SingletonHolder.instance_context, base64PublicKey, url, gameID, userID, platformType, productIdJsonArray, token, scene, unity_msg_receiver);
    	}
        
#elif UNITY_IOS || UNITY_IPHONE
		SDKDSInitPurchaseUtils ();
        if(scene == null)
        {
        	Debug.Log("init Static Purchase scene is null");
        	SDKDSInitPurchase(url, userID, platformType, productIdJsonArray, gameID, token);
    	}
    	else
    	{
    		Debug.Log("init Static Purchase scene is "+scene);
    		SDKDSInitPurchase(url, userID, platformType, productIdJsonArray, gameID, token, scene);
    	}
		
#else

#endif
    }

    /// <summary>
    /// set non-consumables products
    /// </summary>
    /// <param name="noconsume_productIdJsonArray">noconsume product ids, json array format</param>
    public void setNonConsumableProducts(string noconsume_productIdJsonArray)
    {
#if (UNITY_EDITOR || DISBLE_PLATFORM)

#elif UNITY_ANDROID
        gpPaySdkUtils_unity.CallStatic("SdkdsSetNoConsumeProduct", noconsume_productIdJsonArray);
#elif UNITY_IPHONE || UNITY_IOS
#else

#endif
    }

    /// <summary>
    /// request products
    /// </summary>
    /// <param name="productIdJsonArray">product ids, json array format</param>
    public void requestProducts(string productIdJsonArray)
	{
#if (UNITY_EDITOR || DISBLE_PLATFORM)

#elif UNITY_ANDROID
        gpPaySdkUtils_unity.CallStatic("SdkdsRequestProducts");
#elif UNITY_IPHONE || UNITY_IOS
		SDKDSRequestProducts(productIdJsonArray); 
#else

#endif
    }

    /// <summary>
    /// buy product
    /// </summary>
    /// <param name="userID">user id</param>
    /// <param name="platformType">platform type for server verify, optional param, default value: "0"</param>
    /// <param name="productID">product id</param>
    public void buyProduct(string userID, string platformType, string productID)
	{
#if (UNITY_EDITOR || DISBLE_PLATFORM)

#elif UNITY_ANDROID
        gpPaySdkUtils_unity.CallStatic("SdkdsBuyProduct", userID, platformType, productID);
#elif UNITY_IOS || UNITY_IPHONE
		SDKDSBuyProduct(userID, platformType, productID);
#else

#endif
    }

    /// <summary>
    /// Remedies the transaction.
    /// </summary>
    public void remedyTransaction() 
	{
#if (UNITY_EDITOR || DISBLE_PLATFORM)

#elif UNITY_ANDROID

#elif UNITY_IOS || UNITY_IPHONE
		SDKDSRemedyTransaction();
#else

#endif
	}

    /// <summary>
    /// Can Remedies the transaction?
    /// </summary>
    public bool canRemedyTransaction() 
    {
#if (UNITY_EDITOR || DISBLE_PLATFORM)
	    return false;
#elif UNITY_ANDROID
		return false;
#elif UNITY_IOS || UNITY_IPHONE
		return SDKDSCanRemedyTransaction();
#else
		return false;
#endif
    }
    
    /// <summary>
    /// Sets the purchase user identifier.
    /// </summary>
    /// <param name="userID">User identifier.</param>
    public void setPurchaseUserID(string userID)
    {
#if (UNITY_EDITOR || DISBLE_PLATFORM)

#elif UNITY_ANDROID

#elif UNITY_IOS || UNITY_IPHONE
        SDKDSSetPurchaseUserID(userID);
#else

#endif
    }

    /// <summary>
    /// Sets the purchase token.
    /// </summary>
    /// <param name="token">Token.</param>
    public void setPurchaseToken(string token)
    {
#if (UNITY_EDITOR || DISBLE_PLATFORM)

#elif UNITY_ANDROID

#elif UNITY_IOS || UNITY_IPHONE
        SDKDSSetPurchaseToken(token);
#else

#endif
    }

    public void setPurchaseScene(string scene)
    {
#if (UNITY_EDITOR || DISBLE_PLATFORM)

#elif UNITY_ANDROID

#elif UNITY_IOS || UNITY_IPHONE
        SDKDSSetPurchaseScene(scene);
#else

#endif
    }

    /*json data format:{\"Auto-Renewable Subscriptions\":[[\"bnb_weekly\",\"bnb_monthly\",\"bnb_yearly\"]],\"Non-Consumable\":[\"bnb_remove_ads\"]}*/
    /// <summary>
    /// Restores the completed transactions.
    /// </summary>
    /// <param name="strJson">String json.</param>
    public void restoreCompletedTransactions(string strJson)
	{
#if (UNITY_EDITOR || DISBLE_PLATFORM)

#elif UNITY_ANDROID

#elif UNITY_IOS || UNITY_IPHONE
		SDKDSRestoreCompletedTransactions(strJson);
#else

#endif	
	}
	
	/*json data format:"[[\"bnb_remove_ads\"],[\"bnb_weekly\",\"bnb_monthly\",\"bnb_yearly\"]]"*/
	/// <summary>
	/// Requests the non consumable product status with product Ids.
	/// </summary>
	/// <param name="strJson">json String.</param>
	public void requestNonConsumableProductStatusWithProductIDs(string strJson)
	{
#if (UNITY_EDITOR || DISBLE_PLATFORM)

#elif UNITY_ANDROID
		gpPaySdkUtils_unity.CallStatic("SdkdsQuerySubsTime", strJson);
#elif UNITY_IOS || UNITY_IPHONE
		SDKDSRequestNonConsumableProductStatusWithProductIDs(strJson);
#else

#endif
    }


    public void onPayCallBack(string strJson)
    {
        Debug.Log("gp_pay onPayCallBack  msg received@@@@@@   json:" + strJson);
		//TODO add your code here or Add listener for the event
		purchaseCompleteEvent (strJson);
    }

    public void onGetProductInfos(string strJson)
    {
        Debug.Log("gp_pay onGetProductInfos  msg received@@@@@@   json:" + strJson);
		//TODO add your code here or Add listener for the event
		productListReceivedEvent(strJson);
    }

	public void onRestoreProductsWithResult(string strJson)
	{
		Debug.Log("gp_pay onRestoreProductsWithResult  msg received@@@@@@   json:" + strJson);
		//TODO add your code here or Add listener for the event
		restoreCompleteEvent (strJson);
	}

	public void onRequestNonConsumableProductStatusWithResult(string strJson)
	{
		Debug.Log("gp_pay onRequestNonConsumableProductStatusWithResult  msg received@@@@@@   json:" + strJson);
		//TODO add your code here or Add listener for the event
		requestNonConsumableProductStatusCompleteEvent (strJson);
	}


	 public static AndroidJavaClass LoadJavaClass(string name){
        try{
            new AndroidJavaClass("java.lang.Class").CallStatic<AndroidJavaObject>("forName",name);
            AndroidJavaClass javaclass = new AndroidJavaClass(name);
            return javaclass;
        }
        catch (System.Exception e)
        {
            Debug.LogError("SDKDS not found "+name);
            return null;
        }
    }
}
