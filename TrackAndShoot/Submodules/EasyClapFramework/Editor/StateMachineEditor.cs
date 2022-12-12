using System;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace EasyClap.Seneca.StateMachine.Editor
{
    [CustomEditor(typeof(StateMachine))]
    public class StateMachineEditor : OdinEditor
    {
        public override bool RequiresConstantRepaint()
        {
            return false;
        }
    }
}