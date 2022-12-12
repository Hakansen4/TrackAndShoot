using System;
using UnityEngine;

namespace EasyClap.Seneca.Common.Core
{
    public static class DebugLogger
    {
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Info(string str)
        {
            Debug.Log(str);
        }

        public static void Warning(string str)
        {
            Debug.LogWarning(str);
        }

        public static void Error(string str)
        {
            Debug.LogError(str);
        }

        public static void InfoFast<T>(params NameValueBundle<T>[] arr)
        {
            var str = "";
            for (int i = 0; i < arr.Length; i++)
            {
                var e = arr[i];
                str += $"{e.Name} : {e.Value}";
                if (i < arr.Length - 1)
                {
                    str += ", ";
                }
            }
            
            Info(str);
        }
        
        public struct NameValueBundle<T>
        {
            public T Value; 
            public string Name;

            public NameValueBundle(T value) : this(value, String.Empty){}

            public NameValueBundle(T value, string name)
            {
                Value = value;
                Name = name;
            }
        }
    }
}