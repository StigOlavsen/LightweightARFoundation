
using UnityEngine;


namespace FreakshowStudio.LightweightARFoundation.Runtime.AR
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(LineRenderer))]
    public class PlaneVisibility : MonoBehaviour
    {
        #region Inspector Variables
        #pragma warning disable 0649

        [SerializeField]
        private Material _visibleMaterial;

        [SerializeField]
        private Material _hiddenMaterial;

        [SerializeField]
        private Material _visibleLineMaterial;

        [SerializeField]
        private Material _hiddenLineMaterial;

        #pragma warning restore 0649
        #endregion Inspector Variables

        private MeshRenderer _meshRenderer;
        private LineRenderer _lineRenderer;

        public bool Visible { get; set; }

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            _meshRenderer.material = Visible
                ? _visibleMaterial
                : _hiddenMaterial;

            _lineRenderer.material = Visible
                ? _visibleLineMaterial
                : _hiddenLineMaterial;
        }
    }
}
