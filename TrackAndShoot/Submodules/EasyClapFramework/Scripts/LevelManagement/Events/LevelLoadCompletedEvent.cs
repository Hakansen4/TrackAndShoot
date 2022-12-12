using EasyClap.Seneca.Common.Utility;
using JetBrains.Annotations;

namespace EasyClap.Seneca.Common.LevelManagement.Events
{
    /// <summary>
    /// Indicates a level is loaded successfully.
    /// </summary>
    /// <remarks>
    /// <para>This event is intended to be emitted from within the framework. Make sure you know what you are doing if you intend to emit it yourself.</para>
    /// <para><inheritdoc cref="Loaders.ILevelLoader.CurrentLevelIndex"/></para>
    /// <para><inheritdoc cref="VirtualLevelIndexBeforeLoad"/></para>
    /// </remarks>
    [PublicAPI]
    public readonly struct LevelLoadCompletedEvent
    {
        /// <summary>
        /// Current level index value before the load operation.
        /// </summary>
        /// <remarks><inheritdoc cref="Loaders.ILevelLoader.CurrentLevelIndex"/></remarks>
        public int CurrentLevelIndexBeforeLoad { get; }

        /// <summary>
        /// Current level index value after the load operation.
        /// </summary>
        /// <remarks><inheritdoc cref="Loaders.ILevelLoader.CurrentLevelIndex"/></remarks>
        public int CurrentLevelIndexAfterLoad { get; }

        /// <summary>
        /// Is the load operation for loading the same level again?
        /// </summary>
        public bool IsReload { get; }

        /// <summary>
        /// Virtual level index value before the load operation.
        /// </summary>
        /// <remarks>
        /// See also <see cref="Loaders.ILevelLoader.VirtualCurrentLevelIndex"/> for explanation on <i>virtual level indices</i>.
        /// </remarks>
        public int VirtualLevelIndexBeforeLoad { get; }

        /// <summary>
        /// Virtual level index value after the load operation.
        /// </summary>
        /// <remarks><inheritdoc cref="VirtualLevelIndexBeforeLoad"/></remarks>
        public int VirtualLevelIndexAfterLoad { get; }

        /// <summary>
        /// Streamlined level name of the current level after the load operation.
        /// </summary>
        /// <remarks>
        /// This is not intended to be used in-game. This for integrations which require a level name.
        /// Format your own strings for in-game level display purposes.
        /// </remarks>
        public string CurrentLevelNameAfterLoad { get; }

        /// <inheritdoc cref="LevelLoadCompletedEvent"/>
        /// <param name="currentLevelIndexBeforeLoad">Current level index value before the load operation.</param>
        /// <param name="currentLevelIndexAfterLoad">Current level index value after the load operation.</param>
        /// <param name="isReload">Is the load operation for loading the same level again?</param>
        /// <param name="virtualLevelIndexBeforeLoad">Virtual level index value before the load operation.</param>
        /// <param name="virtualLevelIndexAfterLoad">Virtual level index value after the load operation.</param>
        public LevelLoadCompletedEvent(int currentLevelIndexBeforeLoad,
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

            CurrentLevelNameAfterLoad = Utils.GetLevelName(currentLevelIndexAfterLoad);
        }
    }
}
