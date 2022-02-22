using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public sealed class BasicTabGroup : TabGroup
{
    [SerializeField] private List<TabButton> _tabButtons = new List<TabButton>();
    private TabButton _selectedTab;
    
    private readonly Vector3 _normalScale = Vector3.one;
    private readonly Vector3 _selectedScale = Vector3.one * 1.05f;
    private const float SCALE_DURATION = 0.05f;
    
    private void Start()
    {
        SwitchToBasicTab();
    }
    
    public override void Subscribe(TabButton button)
    {
        _tabButtons.Add(button);
    }

    public override void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (_selectedTab == null || button != _selectedTab)
        {
            button.transform.DOScale(_selectedScale, SCALE_DURATION);
        }
    }

    public override void OnTabExit(TabButton button)
    {
        ResetTabs();
        button.transform.DOScale(_normalScale, SCALE_DURATION);
    }

    public override void OnTabSelected(TabButton button)
    {
        _selectedTab = button;
        ResetTabs();
        button.Activate();
    }
    
    private void ResetTabs()
    {
        foreach (var tabButton in _tabButtons)
        {
            if (_selectedTab == null || tabButton != _selectedTab)
            {
                tabButton.Deactivate();
            }
        }
    }
    
    public override void SwitchToBasicTab()
    {
        ResetTabs();
        _selectedTab = _tabButtons[0];
        _tabButtons[0].Activate();
    }
}