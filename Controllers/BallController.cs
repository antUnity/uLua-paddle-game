using UnityEngine;

namespace uLua {
    namespace PaddleGame {
        /// <summary>Implements controls for ball objects.</summary>
        public class BallController : MonoBehaviour {
            // Members
            /** <summary>Movement speed of the ball object.</summary> */
            public float Speed = 0f;

            /** <summary>Delay (in seconds) before the ball is set in motion.</summary> */
            public float Delay = 0f;

            /** <summary>Starting position of the ball object.</summary> */
            public Vector2 StartingPosition = Vector2.zero;

            /** <summary>Reference to the Rigidbody2D component of the ball object.</summary> */
            private Rigidbody2D Rigidbody2D = null;

            /** <summary>Reference time for the delayed start.</summary> */
            private float StartTime = -1f;

            // Access Methods
            // Public

            /// <summary>Checks if the ball is in risk of getting stuck bouncing in a straight line back and forth (horizontally or vertically).</summary>
            public bool IsStuck {
                get {
                    return (Mathf.Abs(Rigidbody2D.velocity.x) <= 0.25f || Mathf.Abs(Rigidbody2D.velocity.y) <= 0.25f);
                }
            }

            // Process Methods
            // Public

            /// <summary>Adjusts the angle of motion of the ball by the specified value.</summary>
            /** @param Degrees The adjustment value (in degrees) for the angle. */
            public void AdjustAngle(float Degrees) {
                if (Rigidbody2D && Rigidbody2D.velocity != Vector2.zero) {
                    float Angle = Mathf.Atan2(Rigidbody2D.velocity.y, Rigidbody2D.velocity.x);
                    Angle += Degrees*Mathf.Deg2Rad;
                    Rigidbody2D.velocity = new Vector2(Mathf.Cos(Angle)*Speed, Mathf.Sin(Angle)*Speed);
                }
            }

            /// <summary>Resets the position of the ball to its starting position and sets the ball in motion.</summary>
            /** This method is exposed to the Game API by the uLua.PaddleGame.Ball class. */
            public void Reset () {
                transform.position = StartingPosition;
                Start();
            }

            // Private

            private void Awake() {
                Rigidbody2D = GetComponent<Rigidbody2D>();
            }

            /// <summary>Starts the delay timer to set the ball in motion.</summary>
            private void Start() {
                StartTime = Time.realtimeSinceStartup;
                if (Rigidbody2D) Rigidbody2D.velocity = Vector2.zero;
            }

            /// <summary>Sets the ball in motion after the delay timer has passed.</summary>
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
