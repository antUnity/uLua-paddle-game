using UnityEngine;

namespace uLua {
    namespace PaddleGame {
        /// Wrapper class used to expose ball objects to Lua. 
        /** Inherits from LuaMonoBehaviour. All public methods and members of this class are exposed to Lua. */
        public class Ball : ExposedMonoBehaviour<Ball> {
            // Members
            private BallController Controller = null;       //!< Reference to the BallController component of the ball object.

            // Access Methods

            /// Static class constructor.
            /** Registers the Ball and Vector2 types to Lua. */
            static Ball() {
                API.RegisterType<Vector2>();
                Lua.Set("Vector2", new Vector2());
            }

            // Public

            /// Used to access/set the delay (in seconds) before the ball is set in motion.
            public float Delay {
                get { return Controller ? Controller.Delay : 0f; }
                set { if (Controller) Controller.Delay = value; }
            }

            /// Used to check if the ball is in risk of getting stuck bouncing in a straight line (horizontally or vertically).
            public bool IsStuck {
                get { return Controller ? Controller.IsStuck : false; }
            }

            /// Used to access/set the speed of the ball.
            public float Speed {
                get { return Controller ? Controller.Speed : 0f; }
                set { if (Controller) Controller.Speed = value; }
            }

            /// Used to access/set the starting position of the ball.
            public Vector2 StartingPosition {
                get { return Controller ? Controller.StartingPosition : Vector2.zero; }
                set { if (Controller) Controller.StartingPosition = value; }
            }

            // Process Methods
            // Public

            /// Adjusts the angle of motion of the ball by the specified value.
            /** @param Degrees The adjustment value (in degrees) for the angle. */
            public void AdjustAngle(float Degrees) {
                if (Controller) Controller.AdjustAngle(Degrees);
            }

            /// Resets the position of the ball to its starting position and sets the ball in motion.
            /** This method may be used in Lua but is also used in the Game class to reset the position of balls. */
            public void Reset() {
                if (Controller) Controller.Reset();
            }

            // Private

            /// Initialises the controller reference on awake.
            private void Awake() {
                Controller = GetComponent<BallController>();
            }

            /// Called when a Ball object collides with any other object
            /** Invokes OnHit event for a Ball object.
             *  Also used to "nudge" the ball by a few degrees when it is in risk of getting stuck bouncing in a straight line. The "nudge" limited to +/-15 degrees. */
            private void OnCollisionEnter2D(Collision2D Other) {
                Invoke("OnHit");

                if (IsStuck) AdjustAngle(Random.Range(-15f, 15f)); else AdjustAngle(0f);
            }
        }
    }
}
