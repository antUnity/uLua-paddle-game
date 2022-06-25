using UnityEngine;

namespace uLua {
    namespace PaddleGame {
        /// Wrapper class used to expose brick objects to Lua.
        /** Inherits from LuaMonoBehaviour. All public methods and members of this class are exposed to Lua. */
        public class Brick : ExposedMonoBehaviour<Brick> {
            // Members
            private SpriteRenderer SpriteRenderer = null;       //!< Reference to the SpriteRenderer component of the brick object.
            public int Health = 1;                              //!< Current health value of the brick.
            public int MaxHealth = 1;                           //!< Maximum health value of the brick.

            // Access Methods

            /// Static class constructor.
            /** Registers the Color type to Lua. */
            static Brick() {
                API.RegisterType<Color>();
                Lua.Set("Color", new Color());
            }

            // Public

            /// Used to access/set the Color of the brick.
            public Color Color {
                get { return SpriteRenderer ? SpriteRenderer.color : Color.white; }
                set { if (SpriteRenderer) SpriteRenderer.color = value; }
            }

            // Process Methods
            // Public

            /// Used to damage the brick and handle object destruction if its health is depleted.
            /** Invokes the BrickDestroyed event and destroys the game object if necessary. */
            public void Damage(int Damage) {
                Health -= Damage;

                if (Health <= 0) {
                    API.Invoke("BrickDestroyed", this);

                    Destroy(gameObject);
                }
            }

            // Private

            private void Awake() {
                SpriteRenderer = GetComponent<SpriteRenderer>();
            }

            /// Invokes the BrickHit event.
            private void OnCollisionEnter2D(Collision2D Other) {
                API.Invoke("BrickHit", this);
            }
        }
    }
}
