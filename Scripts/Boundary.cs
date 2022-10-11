using UnityEngine;

namespace uLua {
    namespace PaddleGame {
        /// <summary>Implements behaviour for the boundary objects.</summary>
        public class Boundary : MonoBehaviour {
            // Methods
            // Private

            /// <summary>Invokes the BoundaryHit event.</summary>
            private void OnCollisionEnter2D(Collision2D Other) {
                if (Other != null) {
                    GameObject Object = Other.gameObject;
                    if (Object) {
                        Ball Ball = Object.GetComponent<Ball>();
                        if (Ball) {
                            if (name == "Bottom") API.Invoke("BoundaryHit", Ball);
                        }
                    }
                }
            }
        }
    }
}
