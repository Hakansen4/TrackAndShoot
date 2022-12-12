using EasyClap.Seneca.Common.LevelManagement.Levels;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasyClap.Seneca.Common.LevelManagement.Databases
{
    /// <summary>
    /// Level database for the scene-based level setup. Levels are scenes.
    /// </summary>
    [PublicAPI]
    [CreateAssetMenu(menuName = MenuName, fileName = TypeName)]
    public class SceneLevelDatabase : LevelDatabase<SceneLevelDatabase, SceneLevel>
    {
        private const string TypeName = nameof(SceneLevelDatabase);
        private const string MenuName = Core.Constants.EasyClap + "/" + TypeName;

        /// <inheritdoc/>
        [field: SerializeField, InlineProperty, Required]
        public override SceneLevel[] Levels { get; [UsedImplicitly] protected set; } = new SceneLevel[0];
    }
}
