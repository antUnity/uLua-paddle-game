using System;

namespace uLua {
    namespace PaddleGame {
        [Serializable]
        /// <summary>Class which keeps track of various game settings.</summary>
        /** All public members of this class are exposed to Lua. Inherits from ```uLua.ExposedClass```. */
        public class Settings : ExposedClass<Settings> {
            // Members
            /** <summary>Keeps track of the current level.</summary> */
            public int Level = 0;

            /** <summary>Keeps track of remaining lives.</summary> */
            public int Lives = 0;

            /** <summary>Keeps track of the player score.</summary> */
            public int Score = 0;

            /** <summary>Indicates the maximum game level.</summary> */
            public int MaximumLevel = 0;

            /** <summary>Delay before the ball is set in motion (in seconds).</summary> */
            public float Delay = 0f;

            /** <summary>Ball/Paddle speed increment per level.</summary> */
            public float SpeedIncrement = 0f;

            /** <summary>Multiplier to adjust the brick color intensity.</summary> */
            public float ColorMultiplier = 0f;

            // Access Methods
            // Public

            /// <summary>Public constructor.</summary>
            /** @param Name Sets the name of the object exposed to Lua.
             *  @param Context Sets the context of the object exposed to Lua.
                @param ExposeOnInit (Optional) Enables/disables the automatic exposure of this object to Lua. */
            public Settings(string Name, LuaMonoBehaviour Context = null, bool ExposeOnInit = true): base(Name, Context, ExposeOnInit) {
            }

            /// <summary>The total speed increment based on the current level.</summary>
            /** This property is exposed to the Game API. */
            public float TotalSpeedIncrement {
                get { return (Level-1)*SpeedIncrement; }
            }

            // Process Methods
            // Public

            /// <summary>Increments player score.</summary>
            /** This method is exposed to the Game API. Invokes a UIUpdate event.
             *  @param Number The number to be added to the score. */
            public void AddScore(int Number) {
                Score += Number;
                API.Invoke("UIUpdate");
            }

            /// <summary>Increments the game level.</summary>
            /** This method is exposed to the Game API. Invokes a UIUpdate event.
             *  @param Number (Optional) The number of levels to add. Defaults to 1. */
            public void AddLevel(int Number = 1) {
                Level += Number;
                API.Invoke("UIUpdate");
            }

            /// <summary>Increments player lives.</summary>
            /** This method is exposed to the Game API. Invokes a UIUpdate event. 
             *  @param Number (Optional) The number of lives to add. Defaults to 1. */
            public void AddLife(int Number = 1) {
                Lives += Number;
                API.Invoke("UIUpdate");
            }
        }
    }
}
