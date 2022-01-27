using System;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;


public sealed class CatalogItemsElement : MonoBehaviour
{
    [SerializeField] private TMP_Text _displayNameText;
    [SerializeField] private TMP_Text _priceText;

    public void SetItem(CatalogItem item)
    {
        _displayNameText.text = item.DisplayName;
        _priceText.text
            = item.VirtualCurrencyPrices.ContainsKey("SB")
            ? item.VirtualCurrencyPrices["SB"].ToString()
            : string.Empty;
    }
}