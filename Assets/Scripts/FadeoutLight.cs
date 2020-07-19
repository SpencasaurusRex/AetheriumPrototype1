using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FadeoutLight : MonoBehaviour
{
    public float FadeTime;
    Light2D _light;
    
    float startingIntensity;
    float t;
    
    void Start()
    {
        if (!TryGetComponent<Light2D>(out var l))
        {
            Destroy(this);
            return;
        }

        _light = l;
        startingIntensity = _light.intensity;
        t = FadeTime;
    }

    void Update()
    {
        t -= Time.deltaTime;
        _light.intensity = Mathf.Max(t / FadeTime * startingIntensity, 0);
    }
}