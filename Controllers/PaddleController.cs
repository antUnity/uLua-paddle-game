using UnityEngine;

namespace uLua {
    namespace PaddleGame {
        /// Used to implement controls for paddle objects.
        public class PaddleController : MonoBehaviour {
            // Members
            public float Speed = 0f;                                        //!< Movement speed of the paddle object.
            public Vector2 StartingPosition = Vector2.zero;                 //!< Starting position of the paddle object.
            public Vector2 ContactPoint = Vector2.zero;                     //!< Latest contact point of the ball relative to the center of the paddle object.
            private BoxCollider2D BoxCollider2D = null;                     //!< Reference to the BoxCollider2D component of the paddle object.
            private Vector2 ScreenBounds = Vector2.zero;                    //!< Screen bounds used to limit the position of the paddle object.

            // Access Methods
            // Public

            /// Used to access/set the scale of the Paddle object.
            public float Scale {
                get { return transform.localScale.x; }
                set { transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z); }
            }

            // Process Methods
            // Public

            /// Resets the position of the paddle to its starting position.
            /** This method is called by the Paddle wrapper.*/
            public void Reset() {
                transform.position = StartingPosition;
            }

            /// Calculates the position of the specified contact point relative to the center of the paddle.
            /** @param Contact The ContactPoint2D of the collision with the ball object. */
            public void UpdateContactPoint(ContactPoint2D Contact) {
                 ContactPoint = new Vector2((Contact.point.x-transform.position.x)/(BoxCollider2D.size.x/2f), (Contact.point.y-transform.position.y)/(BoxCollider2D.size.y/2f));
            }

            // Private

            /// Initialises BoxCollider2D reference and ScreenBounds on awake.
            private void Awake() {
                BoxCollider2D = GetComponent<BoxCollider2D>();
                ScreenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            }

            /// Moves the position of the paddle.
            /** Movement is performed using Unity's Input.GetAxisRaw("Horizontal") (A/D/Left/Right keys by default). */
            private void Update() {
                float Movement = Input.GetAxisRaw("Horizontal")*Speed*Time.deltaTime;
                float NewPositionX = transform.position.x+Movement;
                if (Mathf.Abs(NewPositionX) <  ScreenBounds.x-BoxCollider2D.size.x/2*transform.localScale.x) {
                    transform.position += new Vector3(Movement, 0f);
                }
            }
        }
    }
}
