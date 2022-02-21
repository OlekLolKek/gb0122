using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public sealed class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private TabGroup _tabGroup;
    [SerializeField] private GameObject _linkedWindow;
    private Image _background;

    private void Awake()
    {
        _background = GetComponent<Image>();
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tabGroup.OnTabEnter(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _tabGroup.OnTabSelected(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tabGroup.OnTabExit(this);
    }
}