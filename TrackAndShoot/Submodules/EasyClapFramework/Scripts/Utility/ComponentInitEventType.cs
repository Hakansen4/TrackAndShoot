using JetBrains.Annotations;

namespace EasyClap.Seneca.Common.Utility
{
    [PublicAPI]
    public enum ComponentInitEventType
    {
        Awake,
        OnEnable,
        Start
    }
}
