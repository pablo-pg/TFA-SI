using UnityEngine;
namespace EvolutionaryPerceptron.Util {
    public class CameraAspect : MonoBehaviour {
        /// <value> The target camera the ratio between height and width </value>
        public float aspect;

        /// <summary>
            /// Modify the main camera aspect to the specific ratio
        /// </summary>
        void Start() {
            var variance = aspect / Camera.main.aspect;
            if (variance < 1.0f)
                Camera.main.rect = new Rect((1.0f - variance) / 2.0f, 0, variance, 1.0f);
            else {
                variance = 1.0f / variance;
                Camera.main.rect = new Rect(0, (1.0f - variance) / 2.0f, 1.0f, variance);
            }
        }
    }
}
