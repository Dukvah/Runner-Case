using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        #region Variables

        [Tooltip("The speed at which the camera moves")] 
        [SerializeField] [Range(0, 1)]  float lerpSpeed = 0.125f;
    
        [Tooltip("Distance of the camera to the target")] 
        [SerializeField] Vector3 offset = new(0, -5, 5);
    
        [Tooltip("Target for the camera to follow")] 
        [SerializeField] Transform target;

        private Vector3 _lerpPos;
    
        #endregion
        
        #region Functions

        private void LateUpdate() => CameraMove();
    
        private void CameraMove()
        {
            if (target == null) return;

            _lerpPos = Vector3.Lerp(transform.localPosition, target.localPosition - offset, lerpSpeed);
            transform.localPosition = _lerpPos;
        }

        #endregion
    }
}
