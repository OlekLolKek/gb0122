using System;
using UnityEngine;


public sealed class PlayerView : MonoBehaviour
{
    [SerializeField] private GameObject _head;
    
    public GameObject Head => _head;

    public event Action<Collision> OnCollisionStayEvent = delegate {  };

    private void OnCollisionStay(Collision other)
    {
        OnCollisionStayEvent.Invoke(other);
    }

    public void SetHead(GameObject head)
    {
        _head = head;
    }
}