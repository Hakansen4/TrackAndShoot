using EasyClap.Seneca.Common.LevelManagement.Levels;
using EasyClap.Seneca.Common.ModuleManagement;
using EasyClap.Seneca.Common.Utility;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyClap.Seneca.Common.LevelManagement.Databases
{
    /// <summary>
    /// Base class for a level database.
    /// </summary>
    /// <typeparam name="TDatabase">The deriving type itself.</typeparam>
    /// <typeparam name="TLevel">Type of the levels in the database.</typeparam>
    /// <example>
    /// Example level database that takes in levels of type <c>Foo</c>:
    /// <code>
    /// [CreateAssetMenu(menuName = MenuName, fileName = TypeName)]
    /// public class FooLevelDatabase : LevelDatabase&lt;FooLevelDatabase, Foo&gt;
    /// {
    ///     private const string TypeName = nameof(FooLevelDatabase);
    ///     private const string MenuName = Seneca.Common.Core.Constants.EasyClap + "/" + TypeName;
    ///
    ///     /// &lt;inheritdoc/&gt;
    ///     [field: SerializeField, AssetSelector, AssetsOnly, Required]
    ///     public override Foo[] Levels { get; [UsedImplicitly] protected set; } = new Foo[0];
    /// }
    /// </code>
    /// See also <see cref="SceneLevelDatabase"/> for a real level database example.
    /// </example>
    [PublicAPI]
    public abstract class LevelDatabase<TDatabase, TLevel> : SingletonModule<TDatabase>, ILevelDatabase
        where TDatabase : LevelDatabase<TDatabase, TLevel>
        where TLevel : ILevel
    {
        protected static readonly string FormattedDatabaseTypeName = Utils.GetFormattedName<TDatabase>();
        private static readonly string FormattedBaseLogPrefix = $"{FormattedDatabaseTypeName} (base)";

        /// <inheritdoc/>
        public int LevelCount => Levels.Length;

        public ILevel this[int index] => Levels[index];

        /// <summary>
        /// Levels registered on this database.
        /// </summary>
        public abstract TLevel[] Levels { get; protected set; }

        protected override void Init()
        {
            base.Init();
            
            if (LevelCount <= 0)
            {
                Debug.LogError($"{FormattedBaseLogPrefix}: Level count is {LevelCount}. Did you forget to register your levels on the level database?");
            }
        }

        protected override void InitSecondPass()
        {
            base.InitSecondPass();
            
            LevelServiceLocator.Instance.RegisterActiveDatabase(this);
        }

        /// <summary>
        /// Returns the level with the given index from <see cref="Levels"/>.
        /// </summary>
        public TLevel GetLevelWithIndex(int currentLevelIndex)
        {
            return Levels[currentLevelIndex];
        }
    }
}
