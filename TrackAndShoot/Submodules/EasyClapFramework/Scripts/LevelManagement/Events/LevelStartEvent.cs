using EasyClap.Seneca.Common.LevelManagement;

namespace EasyClap.Seneca.Common
{
    /// <summary>
    /// Please call this event right after the player tap to start the level.
    /// </summary>
    public readonly struct LevelStartEvent
    {
        public readonly int LevelIndex;
        public readonly int VirtualIndex;
        
        public LevelStartEvent(int levelIndex, int virtualLevelIndex)
        {
            LevelIndex = levelIndex;
            VirtualIndex = virtualLevelIndex;
        }
        
        public static LevelStartEvent CurrentLevel =>
            new LevelStartEvent(LevelServiceLocator.Instance.ActiveLoader.CurrentLevelIndex, LevelServiceLocator.Instance.ActiveLoader.VirtualCurrentLevelIndex);
    }
}