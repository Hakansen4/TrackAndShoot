namespace EasyClap.Seneca.Common
{
    public readonly struct DebugMenuOpenedEvent
    {
        public readonly DebugMenu DebugMenu;

        public DebugMenuOpenedEvent(DebugMenu debugMenu)
        {
            DebugMenu = debugMenu;
        }
    }
}