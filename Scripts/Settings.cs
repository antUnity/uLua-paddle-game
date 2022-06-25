using System;

namespace uLua {
    namespace PaddleGame {
        [Serializable]
        /// Class used to keep track of various game settings. 
        /** Inherits from LuaClass. All public methods and members of this class are exposed to Lua. */
        public class Settings : ExposedClass<Settings> {
            // Members
            public int Level = 0;                   //!< Used to keep track of the current level.
            public int Lives = 0;                   //!< Used to keep track of remaining lives.
            public int Score = 0;                   //!< Used to keep track of player score.
            public int MaximumLevel = 0;            //!< Used to indicate the maximum game level.

            public float Delay = 0f;                //!< Delay before the ball is set in motion (in seconds).
            public float SpeedIncrement = 0f;       //!< Ball/Paddle speed increment per level.
            public float ColorMultiplier = 0f;      //!< Multiplier to adjust the brick color intensity.

            // Access Methods
            // Public

            /// Public constructor.
            /** @param Name Sets the name of the object exposed to Lua. */
            public Settings(string Name, LuaMonoBehaviour Context = null): base(Name, Context) {
            }

            /// The total speed increment based on the current level.
            public float TotalSpeedIncrement {
                get { return (Level-1)*SpeedIncrement; }
            }

            // Process Methods
            // Public

            /// Increments player score.
            /** Invokes a UIUpdate event.
             *  @param Number The number to be added to the score. */
            public void AddScore(int Number) {
                Score += Number;
                API.Invoke("UIUpdate");
            }

            /// Increments the game level.
            /** Invokes a UIUpdate event.
             *  @param Number (Optional) The number of levels to add. Defaults to 1. */
            public void AddLevel(int Number = 1) {
                Level += Number;
                API.Invoke("UIUpdate");
            }

            /// Increments player lives.
            /** Invokes a UIUpdate event. 
                @param Number (Optional) The number of lives to add. Defaults to 1. */
            public void AddLife(int Number = 1) {
                Lives += Number;
                API.Invoke("UIUpdate");
            }
        }
    }
}
