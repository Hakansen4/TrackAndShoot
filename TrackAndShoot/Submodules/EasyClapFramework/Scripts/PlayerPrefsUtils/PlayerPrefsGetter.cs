namespace EasyClap.Seneca.Common.PlayerPrefsUtils
{
    internal delegate T PlayerPrefsGetter<T>(string key, T defaultValue);
}
