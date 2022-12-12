using EasyClap.Seneca.Common.LevelManagement.Levels;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasyClap.Seneca.Common.LevelManagement.Databases
{
    /// <summary>
    /// Level database for the prefab-based level setup. Levels are prefabs.
    /// </summary>
    [PublicAPI]
    [CreateAssetMenu(menuName = MenuName, fileName = TypeName)]
    public class PrefabLevelDatabase : LevelDatabase<PrefabLevelDatabase, PrefabLevel>
    {
        private const string TypeName = nameof(PrefabLevelDatabase);
        private const string MenuName = Core.Constants.EasyClap + "/" + TypeName;

        /// <inheritdoc/>
        [field: SerializeField, AssetSelector, AssetsOnly, Required]
        public override PrefabLevel[] Levels { get; [UsedImplicitly] protected set; } = new PrefabLevel[0];
    }
}
