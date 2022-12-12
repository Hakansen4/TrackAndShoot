using UnityEngine;

namespace EasyClap.Common.Input
{
    public interface ISinglePointerRunnerInputListener
    {
        public void OnInputStart();
        public void OnDrag(Vector2 cumulativeDrag);
        public void OnInputEnd();
    }
}
