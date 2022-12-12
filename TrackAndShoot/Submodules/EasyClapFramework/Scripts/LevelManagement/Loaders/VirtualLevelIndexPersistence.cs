using JetBrains.Annotations;

namespace EasyClap.Seneca.Common.LevelManagement.Loaders
{
    /// <summary>
    /// Behaviour of the virtual level index between game sessions.
    /// </summary>
    [PublicAPI, UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public enum VirtualLevelIndexPersistence
    {
        /// <summary>
        /// Start virtual level index from 0 for each game session.
        /// </summary>
        Volatile,

        /// <summary>
        /// Continue virtual level index from what it was in the previous game session.
        /// </summary>
        Persistent
    }
}
