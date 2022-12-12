using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace EasyClap.Seneca.Common.Utility
{
    public static class ListUtility
    {
        public static T GetLast<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                return default;
            }
            
            return list[list.Count - 1];
        }
        
        public static void RemoveLast<T>(this IList<T> list)
        {
            list.RemoveAt(list.Count - 1);
        }
        
        public static void Resize<T>(this List<T> list, int sz, T c = default(T))
        {
            int cur = list.Count;
            if (sz < cur)
            {
                list.RemoveRange(sz, cur - sz);
            }
            else if (sz > cur)
            {
                list.AddRange(Enumerable.Repeat(c, sz - cur));
            }
        }
        
        public static void RemoveAtFast<T>(this IList<T> list, int i)
        {
            Assert.IsTrue(i >= 0 && i < list.Count);
            list[i] = list.Last();
            list.RemoveLast();
        }
    }
}