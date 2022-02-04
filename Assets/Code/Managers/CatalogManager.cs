using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;


public sealed class CatalogManager : MonoBehaviour
{
    [SerializeField] private CatalogItemsElement _elementPrefab;
    [SerializeField] private Transform _elementsRoot;
    [SerializeField] private TMP_Text _currencyText;

    private int _currency;
    
    private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();
    private readonly List<CatalogItemsElement> _elements = new List<CatalogItemsElement>();

    private void Start()
    {
        PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest { StoreId = "items_store" },
            result => HandleStore(result.Store), Debug.LogError);
        // PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), result => { HandleCatalog(result.Catalog); },
        //     error => { Debug.LogError($"Get catalog items failed: {error}"); });

        _currencyText.text = "Loading currency...";
        GetCurrency(result =>
        {
            _currencyText.text = $"Currency: {_currency}";
        });
    }
    
    private void HandleStore(List<StoreItem> store)
    {
        foreach (var item in store)
        {
            var element = Instantiate(_elementPrefab, _elementsRoot);
            element.gameObject.SetActive(true);
            element.SetItem(item);
            
            element.OnButtonPressed += ElementBuyButtonPressed;
            _elements.Add(element);
        }
    }

    // private void HandleCatalog(List<CatalogItem> catalog)
    // {
    //     foreach (var item in catalog)
    //     {
    //         _catalog.Add(item.ItemId, item);
    //         Debug.Log($"item with ID {item.ItemId} was added to dictionary");
    //
    //         if (item.VirtualCurrencyPrices.ContainsKey(Constants.SBEU_CURRENCY_ID))
    //         {
    //             var element = Instantiate(_elementPrefab, _elementsRoot);
    //             element.gameObject.SetActive(true);
    //             element.SetItem(item);
    //             element.OnButtonPressed += ElementBuyButtonPressed;
    //             _elements.Add(element);
    //         }
    //     }
    // }

    private void ElementBuyButtonPressed(StoreItem item)
    {
        Debug.Log("purchasing");
        
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            ItemId = item.ItemId,
            Price = (int)item.VirtualCurrencyPrices[Constants.SBEU_CURRENCY_ID],
            VirtualCurrency = Constants.SBEU_CURRENCY_ID
        }, result => {}, Debug.LogError);
    }

    private void GetCurrency(Action<GetUserInventoryResult> successCallback)
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
        {
            if (result.VirtualCurrency.ContainsKey(Constants.SBEU_CURRENCY_ID))
            {
                _currency = result.VirtualCurrency[Constants.SBEU_CURRENCY_ID];
                successCallback?.Invoke(result);
            }
        }, error =>
        {
            Debug.LogError($"Get user inventory request failed: {error}");
            _currency = int.MinValue;
        });
    }

    private void OnDestroy()
    {
        foreach (var element in _elements)
        {
            element.OnButtonPressed -= ElementBuyButtonPressed;
        }
    }
}