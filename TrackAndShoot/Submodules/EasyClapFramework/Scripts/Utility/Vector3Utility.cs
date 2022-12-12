using UnityEngine;

namespace EasyClap.Seneca.Common.Utility
{
    public static class Vector3Utility
    {
        public static Vector3 MinValue = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        public static Vector3 MaxValue = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

        public static Vector3 Divide(this Vector3 a, Vector3 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }
        
        public static Vector3 GetRandomXZUnit()
        {
            var rndUnitPointXY = Random.insideUnitCircle;
            return new Vector3(rndUnitPointXY.x, 0, rndUnitPointXY.y);
        }
    }
}