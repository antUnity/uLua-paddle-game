using UnityEngine;

namespace uLua {
    namespace PaddleGame {
        /// Used to implement controls for ball objects.
        public class BallController : MonoBehaviour {
            // Members
            public float Speed = 0f;                                    //!< Movement speed of the ball object.
            public float Delay = 0f;                                    //!< Delay (in seconds) before the ball is set in motion.
            public Vector2 StartingPosition = Vector2.zero;             //!< Starting position of the ball object.
            private Rigidbody2D Rigidbody2D = null;                     //!< Reference to the Rigidbody2D component of the ball object.
            private float StartTime = -1f;                              //!< Reference time for the delayed start.

            // Access Methods
            // Public

            /// Used to check if the ball is in risk of getting stuck bouncing in a straight line (horizontally or vertically).
            public bool IsStuck {
                get {
                    return (Mathf.Abs(Rigidbody2D.velocity.x) <= 0.25f || Mathf.Abs(Rigidbody2D.velocity.y) <= 0.25f);
                }
            }

            // Process Methods
            // Public

            /// Adjusts the angle of motion of the ball by the specified value.
            /** @param Degrees The adjustment value (in degrees) for the angle. */
            public void AdjustAngle(float Degrees) {
                if (Rigidbody2D && Rigidbody2D.velocity != Vector2.zero) {
                    float Angle = Mathf.Atan2(Rigidbody2D.velocity.y, Rigidbody2D.velocity.x);
                    Angle += Degrees*Mathf.Deg2Rad;
                    Rigidbody2D.velocity = new Vector2(Mathf.Cos(Angle)*Speed, Mathf.Sin(Angle)*Speed);
                }
            }

            /// Resets the position of the ball to its starting position and sets the ball in motion.
            /** This method is called by the Ball wrapper.*/
            public void Reset () {
                transform.position = StartingPosition;
                Start();
            }

            // Private

            private void Awake() {
                Rigidbody2D = GetComponent<Rigidbody2D>();
            }

            /// Starts the delay timer to set the ball in motion.
            private void Start() {
                StartTime = Time.realtimeSinceStartup;
                if (Rigidbody2D) Rigidbody2D.velocity = Vector2.zero;
            }

            /// Used to set the ball in motion after the delay timer has passed.
            private void Update() {
                if (Rigidbody2D) {
                    if (StartTime >= 0f && Time.realtimeSinceStartup-StartTime > Delay) {
                        float Angle = Random.Range(80f, 100f)*Mathf.Deg2Rad;
                        Rigidbody2D.velocity = new Vector2(Mathf.Cos(Angle)*Speed, Mathf.Sin(Angle)*Speed);
                        StartTime = -1f;
                    }
                }
            }
        }
    }
}
