using JetBrains.Annotations;

namespace EasyClap.Seneca.Common.LevelManagement.Loaders
{
    /// <summary>
    /// Represents a type-free level loader.
    /// You need to access the specific level loader itself for specialized operations.
    /// </summary>
    [PublicAPI]
    public interface ILevelLoader
    {
        /// <inheritdoc cref="Loaders.LevelLoadMethodAfterCompletion"/>
        /// <remarks>
        /// This property being <see cref="Loaders.LevelLoadMethodAfterCompletion.Randomized"/> alone is not enough to indicate whether the loader will load a random level when appropriate. The definitive answer to that question is the value of <see cref="IsInRandomLevelMode"/>.
        /// </remarks>
        LevelLoadMethodAfterCompletion LevelLoadMethodAfterCompletion { get; }

        /// <inheritdoc cref="Loaders.VirtualLevelIndexPersistence"/>
        VirtualLevelIndexPersistence VirtualLevelIndexPersistence { get; }

        /// <summary>
        /// Are we currently loading a level?
        /// </summary>
        bool IsLoading { get; }

        /// <summary>
        /// Will we load random levels?
        /// </summary>
        /// <remarks>
        /// This property is THE definitive answer to whether a random level will be loaded when appropriate. It is NOT the same as checking whether <see cref="LevelLoadMethodAfterCompletion"/> is <see cref="Loaders.LevelLoadMethodAfterCompletion.Randomized"/>.
        /// </remarks>
        bool IsInRandomLevelMode { get; }

        /// <summary>
        /// This is the 0-based amount of non-reload level load operations since the beginning of the session.
        /// </summary>
        /// <remarks>
        /// It is called a virtual level index because it gets incremented during every level load operation that is not a reload.
        /// This would be the actual level index if we had an infinite amount of unique levels in the database.
        /// </remarks>
        int VirtualCurrentLevelIndex { get; }

        /// <summary>
        /// The level index of the currently active level.
        /// </summary>
        /// <remarks>
        /// Level index is the index of a level in the level database. It is NOT the same thing as scene index.
        /// </remarks>
        int CurrentLevelIndex { get; }

        /// <summary>
        /// Did we load the first level of the session yet?
        /// </summary>
        /// <remarks>First level of the session can be loaded via <see cref="LoadFirstLevel"/> or <see cref="SmartLoad"/>.</remarks>
        bool HasFirstLevelOfTheSessionLoaded { get; }

        /// <summary>
        /// Loads the most appropriate level depending on the current state of the framework.
        /// </summary>
        /// <exception cref="Core.SenecaCommonException">
        /// Throws if the first level is already loaded, and the current level is not completed yet. --- If you are not calling this method to load the first level of the session, then make sure you are calling <see cref="ProgressionManager.HandleLevelComplete">HandleLevelComplete</see> on the active <see cref="ProgressionManager"/> before calling this method, as the decision process depends on whether the current level completed with success or not.
        /// </exception>
        /// <remarks>
        /// This method is the recommended way of interacting with a level loader.
        /// <para/>
        /// This method chooses only one of these options, in order:
        /// <list type="number">
        ///     <item>If first level is not loaded yet, call <see cref="LoadFirstLevel"/>.</item>
        ///     <item>If the current level is not completed, throw <see cref="Core.SenecaCommonException"/>.</item>
        ///     <item>If current level was a success, call <see cref="LoadNextLevel"/>.</item>
        ///     <item>If current level was a fail, call <see cref="ReloadCurrentLevel"/>.</item>
        /// </list>
        /// </remarks>
        void SmartLoad();

        /// <summary>
        /// Loads the first level of the session.
        /// </summary>
        /// <remarks>
        /// First level of the session does NOT necessarily mean the first level as seen in the database.
        /// </remarks>
        /// <seealso cref="SmartLoad"/>
        /// <seealso cref="LoadFirstLevelOnInit"/>
        void LoadFirstLevel();

        /// <summary>
        /// Reloads the level that is currently active.
        /// </summary>
        /// <seealso cref="SmartLoad"/>
        void ReloadCurrentLevel();

        /// <summary>
        /// Loads next level.
        /// </summary>
        /// <remarks>
        /// Next level does NOT necessarily mean the level with the next index as seen in the level database.
        /// </remarks>
        /// <seealso cref="SmartLoad"/>
        void LoadNextLevel();

        /// <summary>
        /// Loads the level with the given level index.
        /// </summary>
        /// <param name="levelIndex">The level index to load.</param>
        /// <param name="isReload">You can indicate if this is a reload or not. Inferred if omitted.</param>
        /// <remarks><inheritdoc cref="CurrentLevelIndex"/></remarks>
        /// <seealso cref="SmartLoad"/>
        void LoadLevelWithIndex(int levelIndex, bool? isReload = null);
    }
}
