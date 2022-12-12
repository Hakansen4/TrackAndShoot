using UnityEditor;

namespace EasyClap.Seneca.Common.Editor
{
    public static class AssetReserializingTools
    {
        [MenuItem("Easy Clap/Danger Zone/Force Reserialize All Assets")]
        private static void ForceReserializeAllAssets()
        {
            var confirmation = SenecaEditorUtility.Confirm("You are about to reserialize ALL assets in the project. Depending on the project size, this operation may take a VERY long time. It may also take a VERY long time for everyone else to launch the project for the first time after this operation. Do you wish to continue?");

            if (confirmation)
            {
                AssetDatabase.ForceReserializeAssets();
            }
        }
    }
}
