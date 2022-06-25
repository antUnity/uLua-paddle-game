using UnityEngine;

namespace uLua {
    namespace PaddleGame {
        /// Wrapper class used to expose paddle objects to Lua.
        /** Inherits from LuaMonoBehaviour. All public methods and members of this class are exposed to Lua. */
        public class Paddle : ExposedMonoBehaviour<Paddle> {
            // Members
            private PaddleController Controller = null;         //!< Reference to the PaddleController component of the paddle object.

            // Access Methods
            // Public

            /// Returns the latest contact point of any ball relative to the center of the paddle.
            public Vector2 ContactPoint {
                get { return Controller ? Controller.ContactPoint : Vector2.zero; }
            }

            /// Used to access/set the scale of the paddle.
            public float Scale {
                get { return Controller ? Controller.Scale : 1; }
                set { if (Controller) Controller.Scale = value; }
            }

            /// Used to access/set the speed of the paddle.
            public float Speed {
                get { return Controller ? Controller.Speed : 0; }
                set { if (Controller) Controller.Speed = value; }
            }

            /// Used to access/set the starting position of the paddle.
            public Vector2 StartingPosition {
                get { return Controller ? Controller.StartingPosition : Vector2.zero; }
                set { if (Controller) Controller.StartingPosition = value; }
            }

            // Process Methods
            // Public

            /// Resets the position of the paddle to its starting position.
            /** This method may be used in Lua but is also used in the Game class to reset the position of paddles. */
            public void Reset() {
                if (Controller) Controller.Reset();
            }

            // Private

            /// Initialises the controller reference on awake.
            private void Awake() {
                Controller = GetComponent<PaddleController>();
            }

            /// Updates the latest ContactPoint of any ball with the paddle. Invokes OnHit event for the Paddle object.
            /** ContactPoint indicates the relative position where a ball hit the paddle in the range of (-1, 1) and it is used to adjust the angle of the Ball to allow a degree of player control. */
            private void OnCollisionEnter2D(Collision2D Other) {
                if (Controller) {
                    Controller.UpdateContactPoint(Other.contacts[0]);
                    if (Controller.ContactPoint.y > 0f) {
                        Invoke("OnHit");

                        if (Other != null) {
                            GameObject Object = Other.gameObject;
                            if (Object) {
                                Ball Ball = Object.GetComponent<Ball>();
                                if (Ball) Ball.AdjustAngle(-ContactPoint.x*15f);
                            }
                        }
                    }
                }
            }
        }
    }
}
