// This attribute makes sure linker will always process this assembly even if it has not been references anywhere else.
// However, this doesn't guarantee a preserve. If there is an item that needs to be preserved, mark it as such.
[assembly: UnityEngine.Scripting.AlwaysLinkAssembly]
