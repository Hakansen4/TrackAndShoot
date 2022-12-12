namespace EasyClap.Seneca.Common
{
    public readonly struct DebugMenuClosedEvent
    {
        public readonly DebugMenu DebugMenu;

        public DebugMenuClosedEvent(DebugMenu debugMenu)
        {
            DebugMenu = debugMenu;
        }
    }
}