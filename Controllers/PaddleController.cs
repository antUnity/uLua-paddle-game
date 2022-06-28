using UnityEngine;

namespace uLua {
    namespace PaddleGame {
        /// <summary>Implements controls for paddle objects.</summary>
        public class PaddleController : MonoBehaviour {
            // Members
            /** <summary>Movement speed of the paddle object.</summary> */
            public float Speed = 0f;

            /** <summary>Starting position of the paddle object.</summary> */
            public Vector2 StartingPosition = Vector2.zero;

            /** <summary>Last known contact point of the ball relative to the center of the paddle object.</summary> */
            public Vector2 ContactPoint = Vector2.zero;

            /** <summary>Reference to the BoxCollider2D component of the paddle object.</summary> */
            private BoxCollider2D BoxCollider2D = null;

            /** <summary>Screen bounds which limit the position of the paddle object.</summary> */
            private Vector2 ScreenBounds = Vector2.zero;

            // Access Methods
            // Public

            /// <summary>Access/set the scale of the Paddle object.</summary>
            public float Scale {
                get { return transform.localScale.x; }
                set { transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z); }
            }

            // Process Methods
            // Public

            /// <summary>Resets the position of the paddle to its starting position.</summary>
            /** This method is exposed to the Game API by the uLua.PaddleGame.Paddle class. */
            public void Reset() {
                transform.position = StartingPosition;
            }

            /// <summary>Updates the ContactPoint vector based on a detected collision.</summary>
            /** @param Contact The ContactPoint2D of the collision with the ball object. */
            public void UpdateContactPoint(ContactPoint2D Contact) {
                 ContactPoint = new Vector2((Contact.point.x-transform.position.x)/(BoxCollider2D.size.x/2f), (Contact.point.y-transform.position.y)/(BoxCollider2D.size.y/2f));
            }

            // Private

            /// <summary>Initialises the BoxCollider2D reference and ScreenBounds on awake.</summary>
            private void Awake() {
                BoxCollider2D = GetComponent<BoxCollider2D>();
                ScreenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            }

            /// <summary>Moves the position of the paddle.</summary>
            /** Movement is performed using Unity's ```Input.GetAxisRaw("Horizontal")``` (A/D/Left/Right keys for keyboard by default). */
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
