using System;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public sealed class CatalogItemsElement : MonoBehaviour
{
    [SerializeField] private TMP_Text _displayNameText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Button _buyButton;

    private CatalogItem _item;

    public event Action<CatalogItem> OnButtonPressed = delegate { };
    
    public void SetItem(CatalogItem item)
    {
        _item = item;
        _displayNameText.text = _item.DisplayName;
        _priceText.text
            = _item.VirtualCurrencyPrices.ContainsKey(Constants.SBEU_CURRENCY_ID)
            ? _item.VirtualCurrencyPrices[Constants.SBEU_CURRENCY_ID].ToString()
            : string.Empty;
        
        _buyButton.onClick.AddListener(ButtonPressed);
        Debug.Log("Set button");
    }

    private void ButtonPressed()
    {
        Debug.Log("Button pressed");
        OnButtonPressed.Invoke(_item);
    }
}