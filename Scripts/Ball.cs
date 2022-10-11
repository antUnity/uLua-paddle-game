using UnityEngine;

namespace uLua {
    namespace PaddleGame {
        /// <summary>Wrapper class which exposes ball objects to the API.</summary>
        /** All public members of this class are exposed to Lua. Inherits from ```uLua.ExposedMonoBehaviour```. */
        public class Ball : ExposedMonoBehaviour<Ball> {
            // Fields
            /** <summary>Reference to the BallController component of the ball object.</summary> */
            private BallController Controller = null;

            /// <summary>Static class constructor. Registers the Vector2 type to Lua.</summary>
            /** This static constructor is only executed once. */
            static Ball() {
                API.RegisterType<Vector2>();
                Lua.Set("Vector2", new Vector2());
            }

            // Properties
            // Public

            /// <summary>Access/set the delay (in seconds) before the ball is set in motion.</summary>
            /** This property is exposed to the API. */
            public float Delay {
                get { return Controller ? Controller.Delay : 0f; }
                set { if (Controller) Controller.Delay = value; }
            }

            /// <summary>Checks if the ball is in risk of getting stuck bouncing in a straight line back and forth (horizontally or vertically).</summary>
            /** This property is exposed to the API. */
            public bool IsStuck {
                get { return Controller ? Controller.IsStuck : false; }
            }

            /// <summary>Access/set the speed of the ball.</summary>
            /** This property is exposed to the API. */
            public float Speed {
                get { return Controller ? Controller.Speed : 0f; }
                set { if (Controller) Controller.Speed = value; }
            }

            /// <summary>Access/set the starting position of the ball.</summary>
            /** This property is exposed to the API. */
            public Vector2 StartingPosition {
                get { return Controller ? Controller.StartingPosition : Vector2.zero; }
                set { if (Controller) Controller.StartingPosition = value; }
            }

            // Methods
            // Public

            /// <summary>Adjusts the angle of motion of the ball by the specified value.</summary>
            /** This method is exposed to the API.
            /** @param Degrees The adjustment value (in degrees) for the angle. */
            public void AdjustAngle(float Degrees) {
                if (Controller) Controller.AdjustAngle(Degrees);
            }

            /// <summary>Resets the position of the ball to its starting position and sets the ball in motion.</summary>
            /** This method is exposed to the API. */
            public void Reset() {
                if (Controller) Controller.Reset();
            }

            // Protected

            /// <summary>Initialises the BallController component reference on awake.</summary>
            protected override void Awake() {
                base.Awake();
                Controller = GetComponent<BallController>();
            }

            // Private

            /// <summary>Called when a Ball object collides with any other object.</summary>
            /** Invokes OnHit event for a Ball object.
             *  Also used to "nudge" the ball by a few degrees when it is in risk of getting stuck bouncing in a straight line. The "nudge" limited to +/-15 degrees. */
            private void OnCollisionEnter2D(Collision2D Other) {
                InvokeLua("OnHit");

                if (IsStuck) AdjustAngle(Random.Range(-15f, 15f)); else AdjustAngle(0f);
            }
        }
    }
}
