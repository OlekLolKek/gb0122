using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public sealed class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private TabGroup _tabGroup;
    [SerializeField] private GameObject _linkedWindow;
    [SerializeField] private Color _defaultTextColor;
    [SerializeField] private Color _disabledTextColor;

    private TMP_Text _text;
    private Image _background;

    private bool _isLocked;

    private void Awake()
    {
        _background = GetComponentInChildren<Image>();
        _text = GetComponentInChildren<TMP_Text>();
        _tabGroup.Subscribe(this);
    }

    public void Activate()
    {
        _background.enabled = true;
        if (_linkedWindow)
        {
            _linkedWindow.SetActive(true);
        }
    }

    public void Deactivate()
    {
        _background.enabled = false;
        if (_linkedWindow != null)
        {
            _linkedWindow.SetActive(false);
        }
    }

    public void Lock(bool locked)
    {
        _isLocked = locked;

        _text.color = locked ? _disabledTextColor : _defaultTextColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_isLocked)
            _tabGroup.OnTabEnter(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isLocked)
            _tabGroup.OnTabSelected(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isLocked)
            _tabGroup.OnTabExit(this);
    }
}