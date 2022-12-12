using System;
using UnityEngine;

namespace EasyClap.Seneca.Common.LevelManagement.Levels
{
    [Serializable]
    public class SceneLevel : ILevel
    {
        [SerializeField] private SceneReference sceneReference;
        [SerializeField] private LevelType levelType;

        public SceneReference SceneReference => sceneReference;
        public LevelType LevelType => levelType;
    }
}