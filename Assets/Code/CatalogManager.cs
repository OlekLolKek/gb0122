using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;


public sealed class CatalogManager : MonoBehaviour
{
    [SerializeField] private CatalogItemsElement _element;
    [SerializeField] private Transform _elementsRoot;
    
    private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();

    private void Start()
    {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), result =>
        {
            HandleCatalog(result.Catalog);
        }, error =>
        {
            Debug.LogError($"Get catalog items failed: {error}");
        });
    }
    
    private void HandleCatalog(List<CatalogItem> catalog)
    {
        foreach (var item in catalog)
        {
            _catalog.Add(item.ItemId, item);
            Debug.Log($"item with ID {item.ItemId} was added to dictionary");
            var element = Instantiate(_element, _elementsRoot);
            element.gameObject.SetActive(true);
            element.SetItem(item);
        }
    }
}