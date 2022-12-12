using EasyClap.Seneca.Common.PlayerPrefsUtils;
using JetBrains.Annotations;

namespace EasyClap.Seneca.Common.Core
{
    [PublicAPI]
    public static class PlayerPrefsGateway
    {
        public static class LevelManagement
        {
            /// <inheritdoc cref="Core.Constants.PlayerPrefsKeys.LastSuccessfulLevelIndex"/>
            public static readonly PlayerPrefsEntryInt LastSuccessfulLevelIndex = new PlayerPrefsEntryInt(Constants.PlayerPrefsKeys.LastSuccessfulLevelIndex, -1);

            /// <inheritdoc cref="Core.Constants.PlayerPrefsKeys.DidCompleteAtLeastOnce"/>
            public static readonly PlayerPrefsEntryBool DidCompleteAtLeastOnce = new PlayerPrefsEntryBool(Constants.PlayerPrefsKeys.DidCompleteAtLeastOnce);

            /// <inheritdoc cref="Core.Constants.PlayerPrefsKeys.LastSuccessfulVirtualLevelIndex"/>
            public static readonly PlayerPrefsEntryInt LastSuccessfulVirtualLevelIndex = new PlayerPrefsEntryInt(Constants.PlayerPrefsKeys.LastSuccessfulVirtualLevelIndex, -1);
        }
    }
}
