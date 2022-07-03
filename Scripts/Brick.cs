using UnityEngine;

namespace uLua {
    namespace PaddleGame {
        /// <summary>Wrapper class which exposes brick objects to the Game API.</summary>
        /** All public members of this class are exposed to Lua. Inherits from ```uLua.ExposedMonoBehaviour```. */
        public class Brick : ExposedMonoBehaviour<Brick> {
            // Members
            /** <summary>Reference to the SpriteRenderer component of the brick object.</summary> */
            private SpriteRenderer SpriteRenderer = null;       //!< 

            /** <summary>Current health value of the brick.</summary> */
            public int Health = 1;

            /** <summary>Maximum health value of the brick.</summary> */
            public int MaxHealth = 1;

            // Access Methods

            /// <summary>Static class constructor. Registers the Color type to Lua.</summary>
            /** This static constructor is only executed once. */
            static Brick() {
                API.RegisterType<Color>();
                Lua.Set("Color", new Color());
            }

            // Public

            /// <summary>Access/set the Color of the brick.</summary>
            /** This property is exposed to the Game API. */
            public Color Color {
                get { return SpriteRenderer ? SpriteRenderer.color : Color.white; }
                set { if (SpriteRenderer) SpriteRenderer.color = value; }
            }

            // Process Methods
            // Public

            /// <summary>Damages the brick and handles object destruction if its health is depleted.</summary>
            /** Invokes the ```BrickDestroyed``` event and destroys the game object if necessary. */
            public void Damage(int Damage) {
                Health -= Damage;

                if (Health <= 0) {
                    API.Invoke("BrickDestroyed", this);

                    Destroy(gameObject);
                }
            }
            
            // Protected

            /// <summary>Initialises the SpriteRenderer component reference on awake.</summary>
            protected override void Awake() {
                base.Awake();
                SpriteRenderer = GetComponent<SpriteRenderer>();
            }

            // Private

            /// <summary>Invokes the BrickHit event.</summary>
            private void OnCollisionEnter2D(Collision2D Other) {
                API.Invoke("BrickHit", this);
                InvokeLua("OnHit");
            }
        }
    }
}
