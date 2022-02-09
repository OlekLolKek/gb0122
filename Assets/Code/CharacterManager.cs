using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public sealed class CharacterManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _characterNameInputField;
    [SerializeField] private Button _createButton;
    [SerializeField] private GameObject _createPanel;
    [SerializeField] private GameObject _plusPanel;

    [SerializeField] private GameObject _plus1;
    [SerializeField] private GameObject _character1;
    [SerializeField] private TMP_Text _characterName1;
    [SerializeField] private TMP_Text _characterLevel1;
    [SerializeField] private TMP_Text _characterName2;
    [SerializeField] private TMP_Text _characterLevel2;

    private void Start()
    {
        _createButton.onClick.AddListener(OnCreateButtonClicked);

        UpdateCharacters();
    }

    private void OnDestroy()
    {
        _createButton.onClick.RemoveListener(OnCreateButtonClicked);
    }

    private void OnCreateButtonClicked()
    {
        if (string.IsNullOrEmpty(_characterNameInputField.text))
        {
            Debug.LogError("Character name cannot be empty!");
            return;
        }

        PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest()
        {
            StoreId = Constants.CARACTERS_STORE_ID
        }, HandleStoreResult, Debug.LogError);
    }

    private void HandleStoreResult(GetStoreItemsResult result)
    {
        foreach (var item in result.Store)
        {
            Debug.Log(item.ItemId);
            PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
            {
                ItemId = item.ItemId,
                VirtualCurrency = Constants.SBEU_CURRENCY_ID,
                Price = (int)item.VirtualCurrencyPrices[Constants.SBEU_CURRENCY_ID]
            }, itemResult =>
            {
                Debug.Log($"Item {itemResult.Items[0].ItemId} was purchased");
                TransformItemIntoCharacter(itemResult.Items[0].ItemId);
            }, Debug.LogError);
        }
    }

    private void TransformItemIntoCharacter(string itemId)
    {
        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
        {
            ItemId = itemId,
            CharacterName = _characterNameInputField.text
        }, result =>
        {
            UpdateCharacterStatistics(result.CharacterId);
        }, Debug.LogError);
    }

    private void UpdateCharacterStatistics(string characterId)
    {
        PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
        {
            CharacterId = characterId,
            CharacterStatistics = new Dictionary<string, int>
            {
                { "Level", 1 },
                { "Exp", 0 },
                { "Health", 100 }
            }
        }, result =>
        {
            _createPanel.SetActive(false);
            UpdateCharacters();
        }, Debug.LogError);
    }

    private void UpdateCharacters()
    {
        PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
            result =>
            {
                for (var i = 0; i != 2 && i != result.Characters.Count; ++i)
                {
                    var characterName = result.Characters[i].CharacterName;
                    PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
                    {
                        CharacterId = result.Characters[i].CharacterId
                    }, statisticsResult =>
                    {
                        _plus1.SetActive(false);
                        _characterName1.text = characterName;
                        _characterLevel1.text = statisticsResult.CharacterStatistics["Level"].ToString();
                        _character1.SetActive(true);
                    }, Debug.LogError);
                }
                
                _plusPanel.SetActive(true);

                Debug.Log($"Characters count: {result.Characters.Count}");
            },
            Debug.LogError);
    }
}
