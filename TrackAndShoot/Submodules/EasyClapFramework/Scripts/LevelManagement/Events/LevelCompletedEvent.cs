using EasyClap.Seneca.Common.Utility;
using JetBrains.Annotations;

namespace EasyClap.Seneca.Common.LevelManagement.Events
{
    /// <summary>
    /// Indicates that a level has been completed.
    /// </summary>
    /// <remarks>
    /// This event is intended to be emitted from within the framework.
    /// Make sure you know what you are doing if you intend to emit it yourself.
    /// </remarks>
    [PublicAPI]
    public readonly struct LevelCompletedEvent
    {
        /// <summary>
        /// Level index of the level that got completed.
        /// </summary>
        /// <remarks><inheritdoc cref="Loaders.ILevelLoader.CurrentLevelIndex"/></remarks>
        public int LevelIndex { get; }
        
        /// <summary>
        /// Virtual level index of the level that got completed.
        /// </summary>
        /// <remarks><inheritdoc cref="Loaders.ILevelLoader.VirtualCurrentLevelIndex"/></remarks>
        public int VirtualLevelIndex { get; }

        /// <summary>
        /// Did the player win (true) or lose (false)?
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Optional. The final score for this level.
        /// </summary>
        public int Score { get; }

        /// <inheritdoc cref="LevelCompletedEvent"/>
        /// <param name="levelIndex">Level index of the level that got completed.</param>
        /// <param name="virtualLevelIndex">Virtual level index of the level that got completed.</param>
        /// <param name="isSuccess">Did the player win (true) or lose (false)?</param>
        /// <param name="score">Optional. The final score for this level.</param>
        public LevelCompletedEvent(int levelIndex, int virtualLevelIndex, bool isSuccess, int score = 0)
        {
            LevelIndex = levelIndex;
            VirtualLevelIndex = virtualLevelIndex;
            IsSuccess = isSuccess;
            Score = score;
        }
    }
}
