using System.Collections.Generic;
using UnityEngine;

/// Namespace containing the uLua project.
namespace uLua {
    /// Namespace containing the paddle game.
    /** This is a paddle-style game made as a demo of the uLua toolkit. */
    namespace PaddleGame {
        /// Used to manage all ball, brick, and paddle objects in the scene and to keep track of various game settings.
        /** Inherits from LuaMonoBehaviour. All public methods and members of this class are exposed to Lua. */
        public class Game : ExposedMonoBehaviour<Game> {
            // Members
            [SerializeField] private GameObject BallPrefab = null;                      //!< Reference to the prefab used to instantiate ball objects.
            [SerializeField] private GameObject BrickPrefab = null;                     //!< Reference to the prefab used to instantiate brick objects.
            [SerializeField] private GameObject PaddlePrefab = null;                    //!< Reference to the prefab used to instantiate paddle objects.
            [SerializeField] private List<Ball> Balls = new List<Ball>();               //!< List of balls currently loaded to the scene.
            [SerializeField] private List<Brick> Bricks = new List<Brick>();            //!< List of bricks currently loaded to the scene.
            [SerializeField] private List<Paddle> Paddles = new List<Paddle>();         //!< List of paddles currently loaded to the scene.
            [SerializeField] private Settings Settings = new Settings("Settings");      //!< Structure used to keep track of various game settings.

            // Access Methods
            // Public

            /// Returns the number of balls currently loaded in the scene.
            public int NumBalls {
                get { return Balls.Count; }
            }

            /// Returns the number of bricks currently loaded in the scene.
            public int NumBricks {
                get { return Bricks.Count; }
            }

            /// Returns the number of paddles currently loaded in the scene.
            public int NumPaddles {
                get { return Paddles.Count; }
            }

            /// Returns the ball at index i.
            /** @param i The index of the object. */ 
            public Ball GetBall(int i) {
                Ball Ball = null;
                if (Balls.Count > i) Ball = Balls[i];
                return Ball;
            }

            /// Returns the brick at index i.
            /** @param i The index of the object. */
            public Brick GetBrick(int i) {
                Brick Brick = null;
                if (Bricks.Count > i) Brick = Bricks[i];
                return Brick;
            }

            /// Returns the paddle at index i.
            /** @param i The index of the object. */
            public Paddle GetPaddle(int i) {
                Paddle Paddle = null;
                if (Paddles.Count > i) Paddle = Paddles[i];
                return Paddle;
            }

            // Process Methods
            // Public

            /// Adds a ball to the scene.
            /** @param X, Y Coordinates of the new ball object.
             *  @param Speed Speed of the new ball object.
             *  @param Delay Time before the ball is set in motion. 
             *  @param Name (Optional) Name of the new ball object. */
            public Ball AddBall(float X, float Y, float Speed, float Delay, string Name = "") {
                Ball Ball = null;

                if (BallPrefab) {
                    GameObject Object = Instantiate(BallPrefab, new Vector3(X, Y, 0), Quaternion.identity, transform);

                    if (Object) {
                        Object.name = Name!="" ? Name : "Ball"+Mathf.RoundToInt(Time.realtimeSinceStartup * 10000f);

                        Ball = Object.GetComponent<Ball>();
                        if (Ball) {
                            API.Expose(Ball);

                            Ball.Speed = Speed;
                            Ball.Delay = Delay;
                            Ball.StartingPosition = new Vector2(X, Y);

                            Balls.Add(Ball);
                        }
                    }
                }

                return Ball;
            }

            /// Adds a brick to the scene.
            /** @param X, Y Coordinates of the new brick object.
             *  @param Health Health of the new brick object. 
                @param Name (Optional) Name of the new paddle object. */
            public Brick AddBrick(float X, float Y, int Health = 1, string Name = "") {
                Brick Brick = null;

                if (BrickPrefab) {
                    GameObject Object = Instantiate(BrickPrefab, new Vector3(X, Y, 0), Quaternion.identity, transform);

                    if (Object) {
                        Object.name = Name!=""?Name:"Brick"+Mathf.RoundToInt(Time.realtimeSinceStartup * 10000f);

                        Brick = Object.GetComponent<Brick>();
                        if (Brick) {
                            API.Expose(Brick, this);

                            Brick.Health = Health;
                            Brick.MaxHealth = Health;

                            Bricks.Add(Brick);
                        }
                    }
                }

                return Brick;
            }

            /// Adds a paddle to the scene.
            /** @param X, Y Coordinates of the new paddle object.
             *  @param Speed Speed of the new paddle object.
             *  @param Scale (Optional) Scale of the new paddle object.
             *  @param Name(Optional) Name of the new paddle object. */
            public Paddle AddPaddle(float X, float Y, float Speed, float Scale = 1f, string Name = "") {
                Paddle Paddle = null;

                if (PaddlePrefab) {
                    GameObject Object = Instantiate(PaddlePrefab, new Vector3(X, Y, 0), Quaternion.identity, transform);

                    if (Object) {
                        Object.name = Name!="" ? Name : "Paddle"+Mathf.RoundToInt(Time.realtimeSinceStartup * 10000f);

                        Paddle = Object.GetComponent<Paddle>();
                        if (Paddle) {
                            API.Expose(Paddle);

                            Paddle.Speed = Speed;
                            Paddle.StartingPosition = new Vector2(X, Y);
                            Paddle.Scale = Scale;

                            Paddles.Add(Paddle);
                        }
                    }
                }

                return Paddle;
            }

            /// Resets the scene by clearing all loaded objects.
            /** Destroys all balls, bricks, and paddles and removes them from the scene. */
            public void Clear() {
                // Balls
                List<Ball> ReferenceBalls = new List<Ball>();

                foreach (Ball Ball in Balls) ReferenceBalls.Add(Ball);
                foreach (Ball Ball in ReferenceBalls) RemoveBall(Ball);

                // Bricks
                List<Brick> ReferenceBricks = new List<Brick>();

                foreach (Brick Brick in Bricks) ReferenceBricks.Add(Brick);
                foreach (Brick Brick in ReferenceBricks) RemoveBrick(Brick);

                // Paddles
                List<Paddle> ReferencePaddles = new List<Paddle>();

                foreach (Paddle Paddle in Paddles) ReferencePaddles.Add(Paddle);
                foreach (Paddle Paddle in ReferencePaddles) RemovePaddle(Paddle);
            }

            /// Destroys a brick and removes it from the scene.
            /** @param Brick The Brick to be removed. */
            public void RemoveBrick(Brick Brick) {
                if (Bricks.Contains(Brick)) {
                    Bricks.Remove(Brick);
                    if (Brick) Destroy(Brick.gameObject);
                }
            }

            /// Destroys a ball and removes it from the scene.
            /** @param Ball The Ball to be removed. */
            public void RemoveBall(Ball Ball) {
                if (Balls.Contains(Ball)) {
                    Balls.Remove(Ball);
                    if (Ball) Destroy(Ball.gameObject);
                }
            }

            /// Destroys a paddle and removes it from the scene.
            /** @param Paddle The Paddle to be removed. */
            public void RemovePaddle(Paddle Paddle) {
                if (Paddles.Contains(Paddle)) {
                    Paddles.Remove(Paddle);
                    if (Paddle) Destroy(Paddle.gameObject);
                }
            }

            /// Resets the position of all balls and paddles loaded in the scene.
            /** Used to reset the scene without changing any game settings. */
            public void ResetPositions() {
                foreach (Ball Ball in Balls) Ball.Reset();
                foreach (Paddle Paddle in Paddles) Paddle.Reset();
            }

            /// Resets the speed of all balls and paddles loaded in the scene.
            /** @param SpeedIncrement (Optional) The value (per game level) to be retracted from the ball/paddle speed and paddle scale. */
            public void SlowDown(float SpeedIncrement = 0f) {
                if (SpeedIncrement == 0f) SpeedIncrement = Settings.SpeedIncrement;
                float TotalSpeedIncrement = (Settings.Level-1)*SpeedIncrement;

                foreach (Ball Ball in Balls) Ball.Speed -= TotalSpeedIncrement;
                foreach (Paddle Paddle in Paddles) {
                    Paddle.Speed -= TotalSpeedIncrement;
                    Paddle.Scale -= TotalSpeedIncrement;
                }
            }

            /// Speeds up all balls and paddles loaded in the scene.
            /** @param SpeedIncrement (Optional) The value to be added to ball/paddle speed and paddle scale. */
            public void SpeedUp(float SpeedIncrement = 0f) {
                if (SpeedIncrement == 0f) SpeedIncrement = Settings.SpeedIncrement;

                foreach (Ball Ball in Balls) Ball.Speed += SpeedIncrement;
                foreach (Paddle Paddle in Paddles) {
                    Paddle.Speed += SpeedIncrement;
                    Paddle.Scale += SpeedIncrement;
                }
            }
        }
    }
}
