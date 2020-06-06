using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Beam : MonoBehaviour
{
    [Header("Configuration")]
    public float MaxDistance;

    public float LightWidth = 1f / 16f;
    
    [Header("AutoSet")]
    public Transform VisualTransform;
    public Light2D Light;

    void Start()
    {
        if (!VisualTransform)
        {
            VisualTransform = GetComponentsInChildren<Transform>().First(t => t != transform);   
        }
        if (!Light)
        {
            Light = GetComponentInChildren<Light2D>();
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

        // Unfortunately Unity does not allow us to dynamically create light boundaries
        // Vector3 w = tf.up * LightWidth / 2;
        // Vector3 l = tf.right * xScale;
        // Vector3[] lightPath = new Vector3[4];
        // lightPath[0] = tf.position - w;
        // lightPath[1] = tf.position + w;
        // lightPath[2] = tf.position + l + w;
        // lightPath[3] = tf.position + l - w;

        Light.transform.localScale = new Vector3(xScale, 1, 1);
    }
}
