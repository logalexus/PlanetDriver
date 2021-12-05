using System;
using UnityEngine;
using UnityEngine.Purchasing; //библиотека с покупками, будет доступна когда активируем сервисы

public class IAPCore : MonoBehaviour, IStoreListener //для получения сообщений из Unity Purchasing
{
    private static IStoreController m_StoreController;          //доступ к системе Unity Purchasing
    private static IExtensionProvider m_StoreExtensionProvider; // подсистемы закупок для конкретных магазинов

    public static string kit10k = "com.zephyrusteam.planetdriver.kit10k"; //одноразовые - nonconsumable
    public static string kit4k = "com.zephyrusteam.planetdriver.kit4k"; //одноразовые - nonconsumable или может быть подписка
    public static string kit1k = "com.zephyrusteam.planetdriver.kit1k"; //многоразовые - consumable

    void Start()
    {
        if (m_StoreController == null) //если еще не инициализаровали систему Unity Purchasing, тогда инициализируем
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized()) //если уже подключены к системе - выходим из функции
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Прописываем свои товары для добавления в билдер
        builder.AddProduct(kit10k, ProductType.NonConsumable);
        builder.AddProduct(kit4k, ProductType.NonConsumable); //или ProductType.Subscription
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
        if (IsInitialized()) //если покупка инициализирована 
        {
            Product product = m_StoreController.products.WithID(productId); //находим продукт покупки 

            if (product != null && product.availableToPurchase) //если продукт найдет и готов для продажи
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product); //покупаем
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

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) //контроль покупок
    {
        if (String.Equals(args.purchasedProduct.definition.id, kit10k, StringComparison.Ordinal)) //тут заменяем наш ID
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

    public void RestorePurchases() //Восстановление покупок (только для Apple). У гугл это автоматический процесс.
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer) //если запущенно на эпл устройстве
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();

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
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
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
