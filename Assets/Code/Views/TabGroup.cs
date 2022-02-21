using UnityEngine;


public abstract class TabGroup : MonoBehaviour
{
    public abstract void Subscribe(TabButton button);

    public abstract void OnTabEnter(TabButton button);

    public abstract void OnTabExit(TabButton button);

    public abstract void OnTabSelected(TabButton button);

    public abstract void SwitchToBasicTab();
}