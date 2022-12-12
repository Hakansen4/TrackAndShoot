using UnityEngine;

namespace EasyClap.Common.Input.Demo.Drag
{
    public class RunnerController : MonoBehaviour, ISinglePointerRunnerInputListener
    {
        [SerializeField] private SinglePointerRunnerInputHandler singlePointerRunnerInputHandler;
        [SerializeField] private float inputSensitivity = 5.5f;
        [SerializeField] private float lerpSpeed = 12f;
        [SerializeField] private Vector2 platformLimits = new Vector2(-4.5f, 4.5f);
    
        private float _pivotPosX;
        private float _pivotDragX;
        private float _targetPosX;
    
        private void Awake()
        {
            _targetPosX = transform.position.x;
            singlePointerRunnerInputHandler.AddListener(this);
        }
    
        private void Update()
        {
            var pos = transform.position;
            pos.x = Mathf.Lerp(pos.x, _targetPosX, lerpSpeed * Time.deltaTime);
            transform.position = pos;
        }
    
        public void OnInputStart()
        {
            UpdatePivots(transform.position.x, 0);
        }
    
        public void OnDrag(Vector2 cumulativeDrag)
        {
            var dragX = cumulativeDrag.x - _pivotDragX;
            var posX = _pivotPosX + dragX * inputSensitivity;
            if (posX < platformLimits.x)
            {
                posX = platformLimits.x;
                UpdatePivots(posX, cumulativeDrag.x);
            }
            else if(posX > platformLimits.y)
            {
                posX = platformLimits.y;
                UpdatePivots(posX, cumulativeDrag.x);
            }
    
            _targetPosX = posX;
        }
    
        public void OnInputEnd() { }
        
        private void UpdatePivots(float posX, float dragX)
        {
            _pivotPosX = posX;
            _pivotDragX = dragX;
        }
    }
}
