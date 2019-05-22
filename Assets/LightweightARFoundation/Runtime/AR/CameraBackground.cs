
using UnityEngine;
using UnityEngine.XR.ARFoundation;


namespace FreakshowStudio.LightweightARFoundation.Runtime.AR
{
    [RequireComponent(typeof(Camera))]
    public class CameraBackground : MonoBehaviour
    {
        #region Inspector Variables
        #pragma warning disable 0649

        [SerializeField]
        private ARCameraManager _cameraManager;

        [SerializeField]
        private Material _arKitMaterial;

        [SerializeField]
        private Material _arCoreMaterial;

        #pragma warning restore 0649
        #endregion Inspector Variables

        private Camera _camera;

        private static readonly int _displayTransformId =
            Shader.PropertyToID("_UnityDisplayTransform");

        void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
        {
            var count = eventArgs.textures.Count;

#if UNITY_IOS
            var material = _arKitMaterial;
#elif UNITY_ANDROID
            var material = _arCoreMaterial;
#else
            return;
#endif

            for (int i = 0; i < count; ++i)
            {
                material.SetTexture(
                    eventArgs.propertyNameIds[i],
                    eventArgs.textures[i]);
            }

            if (eventArgs.displayMatrix.HasValue)
            {
                material.SetMatrix(
                    _displayTransformId,
                    eventArgs.displayMatrix.Value);
            }

            if (eventArgs.projectionMatrix.HasValue)
            {
                _camera.projectionMatrix = eventArgs.projectionMatrix.Value;
            }
        }

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void OnEnable()
        {
            _cameraManager.frameReceived += OnCameraFrameReceived;
        }

        private void OnDisable()
        {
            _cameraManager.frameReceived -= OnCameraFrameReceived;
        }
    }
}
