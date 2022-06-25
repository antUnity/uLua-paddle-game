using UnityEngine.UI;

namespace uLua {
    namespace PaddleGame {
        /// Wrapper class used to expose a UI Text object to Lua.
        /** Inherits from LuaMonoBehaviour. All public methods and members of this class are exposed to Lua. */
        public class UIText : ExposedMonoBehaviour<UIText> {
            // Members
            private Text Text = null;                   //!< Reference to the Text component of the object.

            // Access Methods
            // Public

            /// Used to access/set the message of the Text component.
            public string Message {
                get { return Text ? Text.text : ""; }
                set { if (Text) Text.text = value; }
            }

            // Process Methods
            // Private

            /// Initialises the Text reference on awake.
            private void Awake() {
                Text = GetComponent<Text>();
            }
        }
    }
}
