using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;


public sealed class InventoryManager : MonoBehaviour
{
    [SerializeField] private CatalogItemsElement _elementPrefab;
    [SerializeField] private Transform _elementsRoot;
    
    private void OnEnable()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
        result =>
        {
            HandleInventory(result.Inventory);
        }, Debug.LogError);
    }

    private void OnDisable()
    {
        
    }

    private void HandleInventory(List<ItemInstance> items)
    {
        Debug.Log($"Handle inventory {items.Count}");
        foreach (var item in items)
        {
            var element = Instantiate(_elementPrefab, _elementsRoot);
            element.gameObject.SetActive(true);
            element.SetItem(item);
        }
    }
}