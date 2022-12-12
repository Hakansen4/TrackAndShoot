namespace EasyClap.Seneca.Common.SplashScreen
{
    /// <summary>
    /// Scene load behaviour of the Splash Screen.
    /// </summary>
    internal enum SplashSceneLoadBehaviour
    {
        /// <summary>
        /// Do not perform any scene loading in the splash screen.
        /// </summary>
        Disabled,

        /// <summary>
        /// Load the scene that has the build index 1.
        /// </summary>
        SceneWithBuildIndexOne,

        /// <summary>
        /// Load a custom scene.
        /// </summary>
        CustomScene
    }
}
