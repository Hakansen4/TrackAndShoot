using System.Collections.Generic;
using System.Text;
using EasyClap.Seneca.Common.Utility;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyClap.Seneca.Common.EventBus
{
    public static class EventBus<TEvent>
    {
        private static readonly string TypeName = typeof(EventBus<TEvent>).GetFormattedName();
        private static readonly List<EventListener<TEvent>> Listeners = new List<EventListener<TEvent>>();

        private static EventBusConfigModule Config => EventBusConfigModule.Instance;

        [PublicAPI]
        public static void AddListener(EventListener<TEvent> listener)
        {
            if (Config.LogAddListener)
            {
                Debug.Log($"{TypeName}: Adding listener");
            }

            Listeners.Add(listener);
        }

        [PublicAPI]
        public static void RemoveListener(EventListener<TEvent> listener)
        {
            if (Config.LogRemoveListener)
            {
                Debug.Log($"{TypeName}: Removing listener");
            }

            Listeners.Remove(listener);
        }

        [PublicAPI]
        public static void Emit(object sender, TEvent e)
        {
            //A
            MaybeLogEmit(sender, e);

            for (var i = Listeners.Count - 1; i >= 0; i--)
            {
                var listener = Listeners[i];
                listener.Invoke(sender, e);
            }
        }

        private static void MaybeLogEmit(object sender, TEvent e)
        {
            if (!Config.LogEmit)
            {
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"{TypeName}: Emitting to {Listeners.Count} listeners");

            if (Config.LogArguments)
            {
                sb.AppendLine(e.ToObjectSummaryString());
            }

            Debug.Log(sb.ToString());
        }
    }
}
