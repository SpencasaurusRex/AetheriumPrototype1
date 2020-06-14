using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FadeoutLight : MonoBehaviour
{
    public float FadeTime;
    Light2D light;
    
    float startingIntensity;
    float t;
    
    void Start()
    {
        if (!TryGetComponent<Light2D>(out var l))
        {
            Destroy(this);
            return;
        }

        light = l;
        startingIntensity = light.intensity;
        t = FadeTime;
    }

    void Update()
    {
        t -= Time.deltaTime;
        light.intensity = Mathf.Max(t / FadeTime * startingIntensity, 0);
    }
}