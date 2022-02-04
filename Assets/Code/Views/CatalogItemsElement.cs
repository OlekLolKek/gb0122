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

    private StoreItem _item;

    public event Action<StoreItem> OnButtonPressed = delegate { };
    
    // public void SetItem(CatalogItem item)
    // {
    //     _item = item;
    //     _displayNameText.text = _item.DisplayName;
    //     _priceText.text
    //         = _item.VirtualCurrencyPrices.ContainsKey(Constants.SBEU_CURRENCY_ID)
    //         ? _item.VirtualCurrencyPrices[Constants.SBEU_CURRENCY_ID].ToString()
    //         : string.Empty;
    //     
    //     _buyButton.onClick.AddListener(ButtonPressed);
    // }
    
    public void SetItem(StoreItem item)
    {
        _item = item;
        _displayNameText.text = item.ItemId;
        if (item.VirtualCurrencyPrices.ContainsKey(Constants.SBEU_CURRENCY_ID))
        {
            _priceText.text = item.VirtualCurrencyPrices[Constants.SBEU_CURRENCY_ID].ToString();
        }
        
        _buyButton.onClick.AddListener(ButtonPressed);
    }
    
    public void SetItem(ItemInstance itemInstance)
    {
        _displayNameText.text = itemInstance.DisplayName;

        _buyButton.onClick.AddListener(ButtonPressed);
    }

    private void ButtonPressed()
    {
        OnButtonPressed.Invoke(_item);
    }
}