using UnityEngine;

namespace Data
{
    public static class DataExtensions
    {
        public static Vector3Data ToVectorData(this Vector3 vector) => new Vector3Data(vector.x, vector.y, vector.z);
        
        public static Vector3 ToVector3(this Vector3Data vector) => new Vector3(vector.X, vector.Y, vector.Z);

        public static Vector3 AddY(this Vector3 vector, float Yoffset) => vector + Vector3.up * Yoffset;
        
        public static string ToJSON(this object obj) => JsonUtility.ToJson(obj);
        
        public static T Deserialize<T>(this string json) => JsonUtility.FromJson<T>(json);
    }
}