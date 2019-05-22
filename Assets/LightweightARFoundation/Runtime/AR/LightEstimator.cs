
using System;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;


namespace FreakshowStudio.LightweightARFoundation.Runtime.AR
{
    public class LightEstimator : MonoBehaviour
    {
        #region Inspector Variables
        #pragma warning disable 0649

        [SerializeField]
        private ARCameraManager _cameraManager;

        [SerializeField]
        private Light _light;

        [SerializeField]
        private bool _useLightEstimation = true;

        #pragma warning restore 0649
        #endregion Inspector Variables

        public bool UseLightEstimation
        {
            get => _useLightEstimation;
            set
            {
                _useLightEstimation = value;
                UpdateLighting();
            }
        }

        public float? Brightness { get; private set; }
        public float? Temperature { get; private set; }
        public Color? ColorCorrection { get; private set; }

        public static Color32 TemperatureToColor(float temperature)
        {
            var t = temperature / 100.0;

            double  red, green, blue;

            if (t <= 66.0)
            {
                red = 255;
                green = t;
                green = 99.4708025861 * Math.Log(green) - 161.1195681661;

                if (t <= 19.0)
                {
                    blue = 0.0;
                }
                else
                {
                    blue = t - 10.0;
                    blue = 138.5177312231 * Math.Log(blue) - 305.0447927307;
                }
            }
            else
            {
                red = t - 60.0;
                red = 329.698727446 * Math.Pow(red, -0.1332047592);

                green = t - 60.0;
                green = 288.1221695283 * Math.Pow(green, -0.0755148492);

                blue = 255.0;
            }

            byte r = (byte) Mathf.Clamp((float)red, 0f, 255f);
            byte g = (byte) Mathf.Clamp((float)green, 0f, 255f);
            byte b = (byte) Mathf.Clamp((float)blue, 0f, 255f);

            var c = new Color32(r, g, b, 255);

            return c;
        }

        private void UpdateLighting()
        {
            RenderSettings.ambientMode = AmbientMode.Trilight;

            if (_useLightEstimation)
            {
                if (Brightness.HasValue)
                {
                    _light.intensity = Brightness.Value;
                    RenderSettings.ambientIntensity = Brightness.Value;
                }
                else
                {
                    _light.intensity = 1f;
                    RenderSettings.ambientIntensity = 1f;
                }

                // TODO: Android ARCore support

                if (Temperature.HasValue)
                {
                    _light.colorTemperature = Temperature.Value;

                    var c = (Color)TemperatureToColor(Temperature.Value);

                    RenderSettings.ambientSkyColor =
                        Color.Lerp(c, Color.white, 0.75f);

                    RenderSettings.ambientEquatorColor = c;

                    RenderSettings.ambientGroundColor =
                        Color.Lerp(c, Color.black, 0.75f);
                }
                else
                {
                    _light.color = Color.white;

                    RenderSettings.ambientSkyColor = Color.gray;
                    RenderSettings.ambientEquatorColor = Color.gray;
                    RenderSettings.ambientGroundColor = Color.gray;
                }
            }
            else
            {
                _light.intensity = 1f;
                _light.color = Color.white;
                RenderSettings.ambientIntensity = 1f;
                RenderSettings.ambientSkyColor = Color.gray;
                RenderSettings.ambientEquatorColor = Color.gray;
                RenderSettings.ambientGroundColor = Color.gray;
            }
        }

        private void OnCameraFrameReceived(ARCameraFrameEventArgs args)
        {
            Brightness = args.lightEstimation.averageBrightness;
            Temperature = args.lightEstimation.averageColorTemperature;
            ColorCorrection = args.lightEstimation.colorCorrection;

            UpdateLighting();
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
