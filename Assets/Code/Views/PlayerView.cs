using System;
using UnityEngine;


public sealed class PlayerView : MonoBehaviour
{
    public event Action<Collision> OnCollisionStayEvent = delegate {  };

    private void OnCollisionStay(Collision other)
    {
        OnCollisionStayEvent.Invoke(other);
    }
}