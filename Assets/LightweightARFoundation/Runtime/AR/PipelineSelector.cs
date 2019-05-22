
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.LWRP;


namespace FreakshowStudio.LightweightARFoundation.Runtime.AR
{
    public class PipelineSelector : MonoBehaviour
    {
        #region Inspector Variables
        #pragma warning disable 0649

        [SerializeField]
        private LightweightRenderPipelineAsset _arKitPipelineAsset;

        [SerializeField]
        private LightweightRenderPipelineAsset _arCorePipelineAsset;

        #pragma warning restore 0649
        #endregion Inspector Variables

        private void OnEnable()
        {
#if UNITY_IOS
            GraphicsSettings.renderPipelineAsset = _arKitPipelineAsset;
#elif UNITY_ANDROID
            GraphicsSettings.renderPipelineAsset = _arCorePipelineAsset;
#else
            return;
#endif
        }
    }
}
