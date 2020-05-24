using UnityEngine;

namespace DefaultNamespace
{
    public static class Util
    {
        public static Vector2 XY(this Vector3 v) => new Vector2(v.x, v.y);

        public static Vector2 Rotate(this Vector2 v, float deg)
        {
            float sin = Mathf.Sin(deg * Mathf.Deg2Rad);
            float cos = Mathf.Cos(deg * Mathf.Deg2Rad);
         
            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }
    }
}