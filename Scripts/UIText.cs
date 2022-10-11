using UnityEngine.UI;

namespace uLua {
    namespace PaddleGame {
        /// <summary>Wrapper class which exposes a UI Text object to the API.</summary>
        /** All public members of this class are exposed to Lua. Inherits from ```uLua.ExposedMonoBehaviour```. */
        public class UIText : ExposedMonoBehaviour {
            // Fields
            /** <summary>Reference to the Text component of the object.</summary> */
            private Text Text = null;

            // Properties
            // Public

            /// <summary>Access/set the message of the Text component.</summary>
            /** This property is exposed to the API. */
            public string Message {
                get { return Text ? Text.text : ""; }
                set { if (Text) Text.text = value; }
            }

            // Methods
            // Protected

            /// <summary>Initialises the Text component reference on awake.</summary>
            protected override void Awake() {
                base.Awake();
                Text = GetComponent<Text>();
            }
        }
    }
}
