using JetBrains.Annotations;

namespace EasyClap.Seneca.Common.Core
{
    [PublicAPI]
    public static class Constants
    {
        public const string EasyClap = "Easy Clap";
        public const string FrameworkName = "EasyClap.Seneca.Common";

        internal static class Errors
        {
            private const string PleaseReport = "Please report it as soon as possible.";

            public const string InternalFramework = "This is an internal error in the " + FrameworkName + " framework. " + PleaseReport;
        }

        public static class PlayerPrefsKeys
        {
            private const string PrefsKeyPrefix = FrameworkName + "__";

            /// <summary>
            /// The PlayerPrefs entry containing the level index of the last level player has successfully completed.
            /// </summary>
            public const string LastSuccessfulLevelIndex = PrefsKeyPrefix + "LastSuccessfulLevelIndex";

            /// <summary>
            /// PlayerPrefs entry indicating whether player has successfully completed all the levels at least once.
            /// </summary>
            public const string DidCompleteAtLeastOnce = PrefsKeyPrefix + "DidCompleteAtLeastOnce";

            /// <summary>
            /// PlayerPrefs entry containing the VIRTUAL level index of the last level player has successfully completed.
            /// </summary>
            public const string LastSuccessfulVirtualLevelIndex = PrefsKeyPrefix + "LastSuccessfulVirtualLevelIndex";
        }
    }
}
