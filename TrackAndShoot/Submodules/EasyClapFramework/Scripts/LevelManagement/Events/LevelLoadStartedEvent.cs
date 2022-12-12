using JetBrains.Annotations;

namespace EasyClap.Seneca.Common.LevelManagement.Events
{
    /// <summary>
    /// Indicates a level has just started loading.
    /// </summary>
    /// <remarks>
    /// <para>This event is intended to be emitted from within the framework. Make sure you know what you are doing if you intend to emit it yourself.</para>
    /// <para><inheritdoc cref="Loaders.ILevelLoader.CurrentLevelIndex"/></para>
    /// <para><inheritdoc cref="VirtualLevelIndexBeforeLoad"/></para>
    /// </remarks>
    [PublicAPI]
    public readonly struct LevelLoadStartedEvent
    {
        /// <inheritdoc cref="LevelLoadCompletedEvent.CurrentLevelIndexBeforeLoad"/>
        public int CurrentLevelIndexBeforeLoad { get; }

        /// <inheritdoc cref="LevelLoadCompletedEvent.CurrentLevelIndexAfterLoad"/>
        public int CurrentLevelIndexAfterLoad { get; }

        /// <inheritdoc cref="LevelLoadCompletedEvent.IsReload"/>
        public bool IsReload { get; }

        /// <inheritdoc cref="LevelLoadCompletedEvent.VirtualLevelIndexBeforeLoad"/>
        public int VirtualLevelIndexBeforeLoad { get; }

        /// <inheritdoc cref="LevelLoadCompletedEvent.VirtualLevelIndexAfterLoad"/>
        public int VirtualLevelIndexAfterLoad { get; }

        /// <!-- Order of these inheritdoc statements are important! -->
        /// <inheritdoc cref="LevelLoadStartedEvent"/>
        /// <inheritdoc cref="LevelLoadCompletedEvent(int,int,bool,int,int)"/>
        public LevelLoadStartedEvent(int currentLevelIndexBeforeLoad,
            int currentLevelIndexAfterLoad,
            bool isReload,
            int virtualLevelIndexBeforeLoad,
            int virtualLevelIndexAfterLoad)
        {
            CurrentLevelIndexBeforeLoad = currentLevelIndexBeforeLoad;
            CurrentLevelIndexAfterLoad = currentLevelIndexAfterLoad;
            IsReload = isReload;
            VirtualLevelIndexBeforeLoad = virtualLevelIndexBeforeLoad;
            VirtualLevelIndexAfterLoad = virtualLevelIndexAfterLoad;
        }
    }
}
