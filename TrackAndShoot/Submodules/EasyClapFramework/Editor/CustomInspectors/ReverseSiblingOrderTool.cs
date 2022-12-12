using UnityEditor;
using UnityEngine;

namespace EasyClap.Seneca.Common.Editor
{
    public class ReverseSiblingOrderTool
    {
        [MenuItem("Easy Clap/Utility/ReverseSiblingOrder")]
        private static void ReverseSiblingOrder()
        {
            if (Selection.gameObjects[0].TryGetComponent(out Transform transform))
            {
                ReverseSiblingOrder(transform);
            }
        }

        private static void ReverseSiblingOrder(Transform transform)
        {
            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).SetSiblingIndex(0);
            }
        }
    }
}