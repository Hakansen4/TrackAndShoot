using JetBrains.Annotations;

namespace EasyClap.Seneca.Common.SplashScreen.Events
{
    /// <summary>
    /// Indicates that the Splash Screen has reached a certain stage of its lifetime.
    /// </summary>
    /// <remarks>
    /// This event is intended to be emitted from within the framework.
    /// Make sure you know what you are doing if you intend to emit it yourself.
    /// </remarks>
    [PublicAPI]
    public readonly struct SplashScreenLifetimeEvent
    {
        /// <inheritdoc cref="SplashScreenLifetimeStage"/>
        public SplashScreenLifetimeStage LifetimeStage { get; }

        /// <inheritdoc cref="SplashScreenLifetimeStage"/>
        /// <param name="lifetimeStage"><inheritdoc cref="LifetimeStage"/></param>
        public SplashScreenLifetimeEvent(SplashScreenLifetimeStage lifetimeStage)
        {
            LifetimeStage = lifetimeStage;
        }
    }
}
