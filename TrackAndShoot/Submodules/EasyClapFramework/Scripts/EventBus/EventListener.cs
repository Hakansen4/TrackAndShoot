namespace EasyClap.Seneca.Common.EventBus
{
    public delegate void EventListener<in TEvent>(object sender, TEvent e);
}
