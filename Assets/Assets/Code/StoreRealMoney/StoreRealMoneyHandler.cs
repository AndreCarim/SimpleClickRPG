using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using TMPro;

public class StoreRealMoneyHandler : MonoBehaviour, IStoreListener
{
    
    [SerializeField] private GameObject LoadingOverlay;
    [SerializeField] private GemHandler gemHandler;
    [SerializeField] private SoundHandler soundHandler;
    [SerializeField] private GameObject finishBuyingGameObject;
    [SerializeField] private TextMeshProUGUI howManyGemsText;
    [SerializeField] private GameObject realMoneyMenu;

    [SerializeField] private AutoSaveHandler autoSaveHandler;


    private Action OnPurchaseCompleted;
    private IStoreController StoreController;
    private IExtensionProvider ExtensionProvider;

    public delegate void PurchaseEvent(Product Model, Action OnComplete);
    public event PurchaseEvent OnPurchase;


    private string gemSmallSize = "gem_small_size"; //250
    private string gemMediumSize = "gem_medium_size"; // 600
    private string gemBigSize = "gem_big_size";//2k
    private string gemHugeSize = "gem_huge_size";//10k

    [SerializeField] private Button gemSmallSizeButton;
    [SerializeField] private Button gemMediumSizeButton;
    [SerializeField] private Button gemBigSizeButton;
    [SerializeField] private Button gemHugeSizeButton;



    private async void Awake()
    {
        InitializationOptions options = new InitializationOptions()
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            .SetEnvironmentName("test");
#else
            .SetEnvironmentName("production");
#endif
        await UnityServices.InitializeAsync(options);
        ResourceRequest operation = Resources.LoadAsync<TextAsset>("IAPProductCatalog");
        operation.completed += HandleIAPCatalogLoaded;
    }



    //Step 3 Create methods
    public void buyGemSmallSize()
    {
        gemSmallSizeButton.enabled = false;
        HandlePurchase(StoreController.products.WithID(gemSmallSize), HandlePurchaseCompleteSmallSize);
    }
    public void buyGemMediumSize()
    {
        gemMediumSizeButton.enabled = false;
        HandlePurchase(StoreController.products.WithID(gemMediumSize), HandlePurchaseCompleteMediumSize);
    }
    public void buyGemBigSize()
    {
        gemBigSizeButton.enabled = false;
        HandlePurchase(StoreController.products.WithID(gemBigSize), HandlePurchaseCompleteBigSize);
    }
    public void buyGemHugeSize()
    {
        gemHugeSizeButton.enabled = false;
        HandlePurchase(StoreController.products.WithID(gemHugeSize), HandlePurchaseCompleteHugeSize);
    }

    

    private void HandlePurchaseCompleteSmallSize()
    {
        gemSmallSizeButton.enabled = true;
    }

    private void HandlePurchaseCompleteMediumSize()
    {
        gemMediumSizeButton.enabled = true;
    }

    private void HandlePurchaseCompleteBigSize()
    {
        gemBigSizeButton.enabled = true;
    }

    private void HandlePurchaseCompleteHugeSize()
    {
        gemHugeSizeButton.enabled = true;
    }








    public void openMenu()
    {
        realMoneyMenu.SetActive(true);
        finishBuyingGameObject.SetActive(false);
        soundHandler.clickSoundHandler();
        pauseGame();
    }

    public void closeMenu()
    {
        realMoneyMenu.SetActive(false);
        finishBuyingGameObject.SetActive(false);
        soundHandler.clickSoundHandler();
        resumeGame();
    }




    private void HandleIAPCatalogLoaded(AsyncOperation Operation)
    {
        ResourceRequest request = Operation as ResourceRequest;

        Debug.Log($"Loaded Asset: {request.asset}");
        ProductCatalog catalog = JsonUtility.FromJson<ProductCatalog>((request.asset as TextAsset).text);
        Debug.Log($"Loaded catalog with {catalog.allProducts.Count} items");



#if UNITY_ANDROID
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance(AppStore.GooglePlay)
        );
#elif UNITY_IOS
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance(AppStore.AppleAppStore)
        );
#else
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance(AppStore.NotSpecified)
        );
#endif
        builder.AddProduct(gemSmallSize, ProductType.Consumable);
        builder.AddProduct(gemMediumSize, ProductType.Consumable);
        builder.AddProduct(gemBigSize, ProductType.Consumable);
        builder.AddProduct(gemHugeSize, ProductType.Consumable);

        Debug.Log($"Initializing Unity IAP with {builder.products.Count} products");
        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        StoreController = controller;
        ExtensionProvider = extensions;
        Debug.Log($"Successfully Initialized Unity IAP. Store Controller has {StoreController.products.all.Length} products");
       

    }



    private void HandlePurchase(Product Product, Action OnPurchaseCompleted)
    {
        LoadingOverlay.SetActive(true);
        this.OnPurchaseCompleted = OnPurchaseCompleted;
        StoreController.InitiatePurchase(Product);
    }

    public void RestorePurchase() // Use a button to restore purchase only in iOS device.
    {
#if UNITY_IOS
        ExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(OnRestore);
#endif
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"Error initializing IAP because of {error}." +
            $"\r\nShow a message to the player depending on the error.");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string a)
    {
        Debug.LogError($"Error initializing IAP because of {error}." +
            $"\r\nShow a message to the player depending on the error.");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Failed to purchase {product.definition.id} because {failureReason}");
        OnPurchaseCompleted?.Invoke();
        OnPurchaseCompleted = null;
        LoadingOverlay.SetActive(false);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        Debug.Log($"Successfully purchased {purchaseEvent.purchasedProduct.definition.id}");
        OnPurchaseCompleted?.Invoke();
        OnPurchaseCompleted = null;
        LoadingOverlay.SetActive(false);



        // do something, like give the player their currency, unlock the item,
        // update some metrics or analytics, etc...

        finishBuyingGameObject.SetActive(true); //showing the text

        //handles the prizes
        if (purchaseEvent.purchasedProduct.definition.id == gemSmallSize)
        {
            howManyGemsText.text = "250";
            gemHandler.increaseAmountOfGem(250);
        }
        else if(purchaseEvent.purchasedProduct.definition.id == gemMediumSize)
        {
            howManyGemsText.text = "600";
            gemHandler.increaseAmountOfGem(600);
        }
        else if(purchaseEvent.purchasedProduct.definition.id == gemBigSize)
        {
            howManyGemsText.text = "2.000";
            gemHandler.increaseAmountOfGem(2000);
        }
        else if(purchaseEvent.purchasedProduct.definition.id == gemHugeSize)
        {
            howManyGemsText.text = "10.000";
            gemHandler.increaseAmountOfGem(10000);
        }

        soundHandler.boughtGemsSoundHandler();
        autoSaveHandler.save();


        return PurchaseProcessingResult.Complete;
    }






    void pauseGame()
    {
        Time.timeScale = 0;
    }

    void resumeGame()
    {
        Time.timeScale = 1;
    }
}
