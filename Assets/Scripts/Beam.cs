using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Serialization;

public class Beam : MonoBehaviour
{
    [Header("Configuration")] 
    public float MaxDistance;
    
    public Light2D LightPrefab;

    [FormerlySerializedAs("LightDistance")] [Range(0.5f, 5f)]
    public float LightSpacing;
    public float LightFalloff = 1;
    
    
    [Header("AutoSet")] public Transform VisualTransform;

    List<Light2D> Lights;
    float StartingLightIntensity;

    void Start()
    {
        StartingLightIntensity = LightPrefab.intensity;
        
        if (!VisualTransform)
        {
            VisualTransform = GetComponentsInChildren<Transform>().First(t => t != transform);
        }

        Lights = new List<Light2D>();
        int numLights = Mathf.CeilToInt(MaxDistance / LightSpacing);
        for (int i = 0; i < numLights; i++)
        {
            
            var l  = Instantiate(LightPrefab, transform);
            Lights.Add(l);
            l.transform.localPosition = new Vector3(i * LightSpacing, 0, 0);
        }
    }

    void FixedUpdate()
    {
        var tf = transform;
        Vector2 origin = tf.position;

        int opposingTeams = Util.GetOpposingTeamLayerMask(gameObject.layer);
        var hit = Physics2D.Raycast(origin, tf.right, MaxDistance, opposingTeams);

        Vector2 hitLocation;
        if (!hit)
        {
            hitLocation = tf.position + tf.right * MaxDistance;
        }
        else hitLocation = hit.point;

        float xScale = (hitLocation - origin).magnitude * 2 + 0.2f;
        VisualTransform.localScale = new Vector3(xScale, 1, 1);

        // Disable lights that are outside of the beam
        float beamEnd = xScale * 0.5f;
        for (int i = 0; i < Lights.Count; i++)
        {
            float lightPos = LightSpacing * i;
            // xScale / 2 < lightPos : 0
            // xScale / 2 + Falloff < lightPos : 0 - 1
            // xScale / 2 + Falloff > lightPos : 1

            Lights[i].intensity = Mathf.Clamp((beamEnd - lightPos) / LightFalloff, 0, 1) * StartingLightIntensity;
        }
    }
}