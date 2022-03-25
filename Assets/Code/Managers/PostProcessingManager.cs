using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public sealed class PostProcessingManager : MonoBehaviour
{
    [SerializeField] private PostProcessProfile _profile;
    [SerializeField] private float _maxVignetteIntensity;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        
        var manager = FindObjectOfType<PostProcessingManager>();
        
        if (manager != null && manager != this)
        {
            Destroy(gameObject);
        }

        SetVignetteFromHealth(1, 1);
    }
    

    public void SetVignetteFromHealth(float health, float maxHealth)
    {
        if (_profile.TryGetSettings(out Vignette vignette))
        {
            vignette.intensity.value = _maxVignetteIntensity - health / maxHealth * _maxVignetteIntensity;
        }
    }
}