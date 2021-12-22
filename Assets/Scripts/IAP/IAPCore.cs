using System;
using UnityEngine;
using UnityEngine.Purchasing; 

public class IAPCore : MonoBehaviour, IStoreListener 
{
    private static IStoreController _StoreController;          
    private static IExtensionProvider _StoreExtensionProvider; 

    public static string kit10k = "com.zephyrusteam.planetdriver.kit10k"; 
    public static string kit4k = "com.zephyrusteam.planetdriver.kit4k"; 
    public static string kit1k = "com.zephyrusteam.planetdriver.kit1k"; 

    void Start()
    {
        if (_StoreController == null) 
            InitializePurchasing();
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
            return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        
        builder.AddProduct(kit10k, ProductType.NonConsumable);
        builder.AddProduct(kit4k, ProductType.NonConsumable);
        builder.AddProduct(kit1k, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyKit10k()
    {
        BuyProductID(kit10k);
    }

    public void BuyKit4k()
    {
        BuyProductID(kit4k);
    }

    public void BuyKit1k()
    {
        BuyProductID(kit1k);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized()) 
        {
            Product product = _StoreController.products.WithID(productId);  

            if (product != null && product.availableToPurchase) 
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                _StoreController.InitiatePurchase(product); 
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
    {
        if (String.Equals(args.purchasedProduct.definition.id, kit10k, StringComparison.Ordinal)) 
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            Player.Instance.Coins += 10000;

        }
        else if (String.Equals(args.purchasedProduct.definition.id, kit4k, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            Player.Instance.Coins += 4000;
        }
        else if (String.Equals(args.purchasedProduct.definition.id, kit1k, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            Player.Instance.Coins += 1000;
        }
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        return PurchaseProcessingResult.Complete;
    }

    public void RestorePurchases() 
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer) 
        {
            Debug.Log("RestorePurchases started ...");

            var apple = _StoreExtensionProvider.GetExtension<IAppleExtensions>();

            apple.RestoreTransactions((result) =>
            {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }
    

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        _StoreController = controller;
        _StoreExtensionProvider = extensions;
    }

    private bool IsInitialized()
    {
        return _StoreController != null && _StoreExtensionProvider != null;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }



}
