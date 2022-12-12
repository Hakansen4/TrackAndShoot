using EasyClap.Seneca.Common.ModuleManagement;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasyClap.Seneca.Common.EventBus
{
    [CreateAssetMenu(fileName = TypeName, menuName = MenuName)]
    internal sealed class EventBusConfigModule : SingletonModule<EventBusConfigModule>
    {
        private const string TypeName = nameof(EventBusConfigModule);
        private const string MenuName = Core.Constants.EasyClap + "/" + TypeName;

        [field: SerializeField]
        internal bool LogEmit { get; [UsedImplicitly] private set; } = true;

        [field: SerializeField]
        [field: Indent, EnableIf(nameof(LogEmit))]
        [field: InfoBox("Logging arguments is a VERY expensive operation. Use it only for debugging purposes.", InfoMessageType.Warning)]
        internal bool LogArguments { get; [UsedImplicitly] private set; } = false;

        [field: SerializeField]
        [field: Space]
        internal bool LogAddListener { get; [UsedImplicitly] private set; } = false;

        [field: SerializeField]
        [field: Space]
        internal bool LogRemoveListener { get; [UsedImplicitly] private set; } = false;
    }
}
