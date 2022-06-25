using UnityEngine;

namespace uLua {
    namespace PaddleGame {
        /// Used to implement behaviour for the boundary objects.
        public class Boundary : MonoBehaviour {
            // Access Methods

            // Process Methods
            // Private

            /// Invokes the BoundaryHit event.
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
