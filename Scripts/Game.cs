using System.Collections.Generic;
using UnityEngine;


/// <summary>Namespace containing the uLua project.</summary>
namespace uLua {
    /// <summary>Namespace containing the paddle game.</summary>
    /** This is a paddle-style game made as a demo of the uLua toolkit. */
    namespace PaddleGame {
        /// <summary>Manages all ball, brick, and paddle objects in the scene and keeps track of various game settings.</summary>
        /** All public members of this class are exposed to Lua. Inherits from ```uLua.ExposedMonoBehaviour```. */
        public class Game : ExposedMonoBehaviour<Game> {
            // Members
            /** <summary>Reference to the prefab used to instantiate ball objects.</summary> */
            [SerializeField] private GameObject BallPrefab = null;

            /** <summary>Reference to the prefab used to instantiate brick objects.</summary> */
            [SerializeField] private GameObject BrickPrefab = null;

            /** <summary>Reference to the prefab used to instantiate paddle objects.</summary> */
            [SerializeField] private GameObject PaddlePrefab = null;

            /** <summary>List of balls currently loaded to the scene.</summary> */
            [SerializeField] private List<Ball> Balls = new List<Ball>();

            /** <summary>List of bricks currently loaded to the scene.</summary> */
            [SerializeField] private List<Brick> Bricks = new List<Brick>();

            /** <summary>List of paddles currently loaded to the scene.</summary> */
            [SerializeField] private List<Paddle> Paddles = new List<Paddle>();

            /** <summary>Structure which keeps track of various game settings.</summary> */
            [SerializeField] private Settings Settings = new Settings("Settings");

            // Access Methods
            // Public

            /// <summary>Returns the number of balls currently loaded in the scene.</summary>
            /** This property is exposed to the Game API. */
            public int NumBalls {
                get { return Balls.Count; }
            }

            /// <summary>Returns the number of bricks currently loaded in the scene.</summary>
            /** This property is exposed to the Game API. */
            public int NumBricks {
                get { return Bricks.Count; }
            }

            /// <summary>Returns the number of paddles currently loaded in the scene.</summary>
            /** This property is exposed to the Game API. */
            public int NumPaddles {
                get { return Paddles.Count; }
            }

            /// <summary>Returns the ball at index i.</summary>
            /** This method is exposed to the Game API.
                @param i The index of the object. */
            public Ball GetBall(int i) {
                Ball Ball = null;
                if (Balls.Count > i) Ball = Balls[i];
                return Ball;
            }

            /// <summary>Returns the brick at index i.</summary>
            /** This method is exposed to the Game API.
                @param i The index of the object. */
            public Brick GetBrick(int i) {
                Brick Brick = null;
                if (Bricks.Count > i) Brick = Bricks[i];
                return Brick;
            }

            /// <summary>Returns the paddle at index i.</summary>
            /** This method is exposed to the Game API.
                @param i The index of the object. */
            public Paddle GetPaddle(int i) {
                Paddle Paddle = null;
                if (Paddles.Count > i) Paddle = Paddles[i];
                return Paddle;
            }

            // Process Methods
            // Public

            /// <summary>Adds a ball to the scene.</summary>
            /** This method is exposed to the Game API.
             *  @param X, Y Coordinates of the new ball object.
             *  @param Speed Speed of the new ball object.
             *  @param Delay Time before the ball is set in motion. 
             *  @param Name (Optional) Name of the new ball object. */
            public Ball AddBall(float X, float Y, float Speed, float Delay, string Name = "") {
                Ball Ball = null;
                                
                if (BallPrefab) {
                    GameObject Object = Instantiate(BallPrefab, new Vector3(X, Y, 0), Quaternion.identity, transform);

                    if (Object) {
                        Object.name = Name != "" ? Name : "Ball" + Mathf.RoundToInt(Time.realtimeSinceStartup * 10000f);

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

            /// <summary>Adds a brick to the scene.</summary>
            /** This method is exposed to the Game API.
             *  @param X, Y Coordinates of the new brick object.
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

            /// <summary>Adds a paddle to the scene.</summary>
            /** This method is exposed to the Game API.
             *  @param X, Y Coordinates of the new paddle object.
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

            /// <summary>Resets the scene by clearing all loaded objects.</summary>
            /** This method is exposed to the Game API.
             *  Destroys all balls, bricks, and paddles and removes them from the scene. */
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

            /// <summary>Destroys a brick and removes it from the scene.</summary>
            /** This method is exposed to the Game API.
             *  @param Brick The Brick to be removed. */
            public void RemoveBrick(Brick Brick) {
                if (Bricks.Contains(Brick)) {
                    Bricks.Remove(Brick);
                    if (Brick) Destroy(Brick.gameObject);
                }
            }

            /// <summary>Destroys a ball and removes it from the scene.</summary>
            /** This method is exposed to the Game API.
             *  @param Ball The Ball to be removed. */
            public void RemoveBall(Ball Ball) {
                if (Balls.Contains(Ball)) {
                    Balls.Remove(Ball);
                    if (Ball) Destroy(Ball.gameObject);
                }
            }

            /// <summary>Destroys a paddle and removes it from the scene.</summary>
            /** This method is exposed to the Game API.
             *  @param Paddle The Paddle to be removed. */
            public void RemovePaddle(Paddle Paddle) {
                if (Paddles.Contains(Paddle)) {
                    Paddles.Remove(Paddle);
                    if (Paddle) Destroy(Paddle.gameObject);
                }
            }

            /// <summary>Resets the position of all balls and paddles loaded in the scene.</summary>
            /** This method is exposed to the Game API.
             *  Used to reset the scene without changing any game settings. */
            public void ResetPositions() {
                foreach (Ball Ball in Balls) Ball.Reset();
                foreach (Paddle Paddle in Paddles) Paddle.Reset();
            }

            /// <summary>Resets the speed of all balls and paddles loaded in the scene.</summary>
            /** This method is exposed to the Game API.
             *  @param SpeedIncrement (Optional) The value (per game level) to be retracted from the ball/paddle speed and paddle scale. */
            public void SlowDown(float SpeedIncrement = 0f) {
                if (SpeedIncrement == 0f) SpeedIncrement = Settings.SpeedIncrement;
                float TotalSpeedIncrement = (Settings.Level-1)*SpeedIncrement;

                foreach (Ball Ball in Balls) Ball.Speed -= TotalSpeedIncrement;
                foreach (Paddle Paddle in Paddles) {
                    Paddle.Speed -= TotalSpeedIncrement;
                    Paddle.Scale -= TotalSpeedIncrement;
                }
            }

            /// <summary>Speeds up all balls and paddles loaded in the scene.</summary>
            /** This method is exposed to the Game API.
             *  @param SpeedIncrement (Optional) The value to be added to ball/paddle speed and paddle scale. */
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
