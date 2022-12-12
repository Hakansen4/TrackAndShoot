using System;
using UnityEngine;

namespace EasyClap.Seneca.Common.LevelManagement.Levels
{
    [Serializable]
    public class PrefabLevel : ILevel
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private LevelType levelType;

        public GameObject Prefab => prefab;
        public LevelType LevelType => levelType;
    }
}