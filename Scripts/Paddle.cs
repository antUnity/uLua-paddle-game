using UnityEngine;

namespace uLua {
    namespace PaddleGame {
        /// <summary>Wrapper class which exposes paddle objects to the Game API.</summary>
        /** All public members of this class are exposed to Lua. Inherits from ```uLua.ExposedMonoBehaviour```. */
        public class Paddle : ExposedMonoBehaviour<Paddle> {
            // Members
            /** <summary>Reference to the PaddleController component of the paddle object.</summary> */
            private PaddleController Controller = null;

            // Access Methods
            // Public

            /// <summary>Returns the last known contact point of the ball relative to the center of the paddle object.</summary>
            /** This property is exposed to the Game API. */
            public Vector2 ContactPoint {
                get { return Controller ? Controller.ContactPoint : Vector2.zero; }
            }

            /// <summary>Access/set the scale of the paddle.</summary>
            /** This property is exposed to the Game API. */
            public float Scale {
                get { return Controller ? Controller.Scale : 1; }
                set { if (Controller) Controller.Scale = value; }
            }

            /// <summary>Access/set the speed of the paddle.</summary>
            /** This property is exposed to the Game API. */
            public float Speed {
                get { return Controller ? Controller.Speed : 0; }
                set { if (Controller) Controller.Speed = value; }
            }

            /// <summary>Access/set the starting position of the paddle.</summary>
            /** This property is exposed to the Game API. */
            public Vector2 StartingPosition {
                get { return Controller ? Controller.StartingPosition : Vector2.zero; }
                set { if (Controller) Controller.StartingPosition = value; }
            }

            // Process Methods
            // Public

            /// <summary>Resets the position of the paddle to its starting position.</summary>
            /** This method is exposed to the Game API. */
            public void Reset() {
                if (Controller) Controller.Reset();
            }

            // Private

            /// <summary>Initialises the PaddleController component reference on awake.</summary>
            private void Awake() {
                Controller = GetComponent<PaddleController>();
            }

            /// <summary>Updates the latest ContactPoint of any ball with the paddle. Invokes the OnHit event for the paddle object.</summary>
            /** The ```ContactPoint``` vector indicates the relative position where a ball hit the paddle in the range of (-1, 1) and it is used to adjust the angle of the Ball to allow a degree of player control. */
            private void OnCollisionEnter2D(Collision2D Other) {
                if (Controller) {
                    Controller.UpdateContactPoint(Other.contacts[0]);
                    if (Controller.ContactPoint.y > 0f) {
                        InvokeLua("OnHit");

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
