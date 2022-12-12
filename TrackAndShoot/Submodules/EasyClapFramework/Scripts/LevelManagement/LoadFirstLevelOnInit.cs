using EasyClap.Seneca.Common.Utility;
using JetBrains.Annotations;

namespace EasyClap.Seneca.Common.LevelManagement
{
    /// <summary>
    /// Calls <see cref="Loaders.ILevelLoader.LoadFirstLevel"/> on <see cref="LevelServiceLocator.ActiveLoader"/> at the beginning of the component lifecycle.
    /// </summary>
    /// <remarks><inheritdoc/></remarks>
    [PublicAPI]
    public class LoadFirstLevelOnInit : InitActionMonoBehaviour
    {
        protected override void PerformAction()
        {
            LevelServiceLocator.Instance.ActiveLoader.LoadFirstLevel();
        }
    }
}
