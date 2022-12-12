using JetBrains.Annotations;

namespace EasyClap.Seneca.Common.SplashScreen
{
    /// <summary>
    /// Indicates a certain stage in Splash Screen lifetime.
    /// </summary>
    /// <remarks>
    /// This event is intended to be emitted from within the framework. Make sure you know what you are doing if you intend to emit it yourself.
    /// </remarks>
    [PublicAPI]
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public enum SplashScreenLifetimeStage
    {
        /// <summary>
        /// Splash screen has just loaded. Logo will start to fade-in.
        /// </summary>
        /// <remarks>
        /// Anything done here will delay the logo fade-in.
        /// You can perform cheap initializations here, but nothing too expensive.
        /// </remarks>
        BeforeFadeIn,

        /// <summary>
        /// Logo faded-in. Waiting.
        /// </summary>
        /// <remarks>
        /// Nothing done here will cause a delay as long as it completes within the wait time limit.
        /// You can do expensive operations here, wait time will account for it.
        /// </remarks>
        AfterFadeIn,

        /// <summary>
        /// Wait time is over. Will fade-out the logo.
        /// </summary>
        /// <remarks>
        /// Anything done here will delay the logo fade-out.
        /// Try not to do anything here at all.
        /// </remarks>
        BeforeFadeOut,

        /// <summary>
        /// Logo faded-out completely. Will maybe load the next scene.
        /// </summary>
        /// <remarks>
        /// Anything done here will delay the next scene load.
        /// You can perform a custom scene load here.
        /// </remarks>
        AfterFadeOut
    }
}
