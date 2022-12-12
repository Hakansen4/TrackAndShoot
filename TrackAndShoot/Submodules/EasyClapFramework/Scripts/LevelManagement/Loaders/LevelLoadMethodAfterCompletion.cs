using JetBrains.Annotations;

namespace EasyClap.Seneca.Common.LevelManagement.Loaders
{
    /// <summary>
    /// The level selection behaviour to follow after the player has completed all the levels.
    /// </summary>
    [PublicAPI, UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public enum LevelLoadMethodAfterCompletion
    {
        /// <summary>
        /// Continue selecting levels linearly. Select the first level after the last one (i.e. circular).
        /// </summary>
        Linear,

        /// <summary>
        /// Select levels randomly.
        /// </summary>
        Randomized
    }
}
