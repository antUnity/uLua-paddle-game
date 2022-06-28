# uLua Paddle Game

In this project I have developed an API for a basic paddle game to demonstrate the flexibility of the uLua API framework as a modding tool.

uLua is a Game API framework for Unity. It aims to streamline the development of a Game API in Lua for your Unity Project.

uLua wraps around MoonSharp and provides an object oriented framework for API development in Lua.
It works by setting up an application-wide Lua context and exposing game objects to it.
Objects exposed to Lua can then be accessed in Lua scripts to implement game behaviour in your project.
User-defined Lua scripts may be executed at runtime to allow modding of your project.

uLua implements the following features:
- Script execution framework which allows Lua scripts to be executed from the Resources folder or an external directory.
- Event handling system which allows you to invoke events from C# and handle them in Lua.
- Base classes which streamline exposing your game objects and data structures to Lua in order to develop your Game API.
- Callback function system for your Game API objects.

## Dependencies

- [MoonSharp for Unity](https://assetstore.unity.com/packages/tools/moonsharp-33776)
- [uLua for Unity](https://assetstore.unity.com/packages/slug/226043)

## Documentation

This document is accompanied by source code documentation, which is found on the [GitHub pages](https://antunity.github.io/uLua-paddle-game/) of this repository.
For any further questions do not hesitate to contact support@antsoftware.co.uk.

Working on this project and documentation has taken many hours. Please support me by starring this project on GitHub.
If you purchased uLua, please leave a review on the asset store! Follow my [Twitter](https://twitter.com/_ANTSoftware "@_ANTSoftware")!

## Usage Tutorial

***Note: It is recommended to read the [uLua Usage Tutorial](https://antunity.github.io/uLua-docs) in the full documentation of uLua before reading this document.***

The basic behaviour and game logic of this game are implemented in Unity Engine. Specifically:
- Controller scripts for uLua.PaddleGame.Paddle and uLua.PaddleGame.Ball objects.
- Invoking of game events and object callback functions.
- Collision detection logic for all objects.

However, the Unity scene for this game does not contain a ball, paddle, or bricks.
The levels for this game are set up entirely through the Game API in Lua.

### 1. Scripting API

#### 1.1. Game Objects

The Game API contains the following objects/data structures:
- **Game**: Manager object which is used to create and keep track of Balls, Paddles, and Bricks within the level.
- **Settings**: Data structure which contains the current level, remaining lives, score, and other game settings.
- **LevelText**: UI element to display the current level.
- **LivesText**: UI element to display the remaining lives.
- **ScoreText**: UI element to display the player score.

All ball, brick, and paddle objects are also exposed to the Game API, however, their names are based on
the application runtime, unless specified otherwise in the corresponding command:

```
Game:AddBall(0, 0, 5, 0.5, "BallName");
```

Omitting the last parameter in this method will auto-generate a name of the object. 
The following is an example of an auto-generated name:

```
Ball12345
```

Each Lua object contains various methods as defined in their corresponding Unity scripts. For a full list of the members of all object types,
refer to the relevant source code documentation. Remember that all public members of the following classes are accessible in the Game API.
- uLua.PaddleGame.Ball
- uLua.PaddleGame.Brick
- uLua.PaddleGame.Paddle
- uLua.PaddleGame.Game
- uLua.PaddleGame.Settings
- uLua.PaddleGame.UIText

#### 1.2. Events and Callback Functions

The following events are invoked during the game and may be handled in Lua:
- **BoundaryHit**: Invoked when a ball hits the bottom screen boundary.
- **BrickDestroyed**: Invoked when a brick is destroyed. Passes one argument, which is the brick object that was destroyed.
- **BrickHit**: Invoked when a brick is hit. Passes one argument, which is the brick object that was hit.
- **SceneLoaded**: Invoked when the scene is loaded.
- **UIUpdate**: Invoked when the game level, player lives, or player score are altered.

The following object callback functions are invoked during the game and may be handled in Lua:
- **Ball:OnHit()**: Called when a ball hits an object. To handle in the API, the ball name must be known.
- **Brick:OnHit()**:  Called when a brick hits an object. To handle in the API, the brick name must be known.
- **Paddle:OnHit()**: Called when a paddle hits an object. To handle in the API, the paddle name must be known.

### 2. Modding the Game

The Lua script which is executed by default for this game is listed at the end of this section.
You may mod the game by extending this script (adding custom Lua scripts at the end) or replacing the original script entirely.

To do that, you must place your new Lua scripts in the appropriate external directory.  The external directory is set to Unity's ```Application.persistentDataPath``` by default.
For more information for different platforms, check the [relevant Unity documentation](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html).

For this project, and for Windows specifically, the directory for the scripts would be the following:

```
C:\Users\{User}\AppData\LocalLow\ANT Software\Paddle Game\
```

In order to replace the original scene script you must create a file named ***PaddleGame.lua*** in this directory. I recommend copying the script below to start making small tweaks to familiarize yourself with the game API.

In order to extend the original scene script you may add any lua scripts in the "UserScripts" folder within the external directory. Again, for Windows that would be the following:

```
C:\Users\{User}\AppData\LocalLow\ANT Software\Paddle Game\UserScripts\
```

***PaddleGame.lua***
```
--[[ June 2022 																											]]
--[[ This Lua script implements a large portion of the gameplay in this simple Paddle-style game.						]]
--[[ It is meant to demonstrate the flexibility of the uLua API framework as a modding tool.							]]

math.randomseed(os.time());					-- Used to generate random seed.

local BrickColors = {						-- Brick color table. Used to color blocks by different health values.
	{1.0, 0.0, 0.0}, 						-- (1) Red
	{1.0, 0.5, 0.0}, 						-- (2) Orange
	{1.0, 1.0, 0.0}, 						-- (3) Yellow
	{0.5, 1.0, 0.0},						-- (4) Lawn Green
	{0.0, 1.0, 0.0},						-- (5) Green
	{0.0, 1.0, 0.5}, 						-- (6) Spring Green
	{0.0, 1.0, 1.0}, 						-- (7) Aqua
	{0.0, 0.5, 1.0}, 						-- (8) Dodger Blue
	{0.0, 0.0, 1.0}, 						-- (9) Blue
	{1.0, 0.0, 1.0}, 						-- (10) Magenta
}

--[[ Methods 																											]]
--[[ The following methods are fully implemented in Lua and have no equivalent in Unity. 								]]
--[[ They are defined as members of the 'Settings' and 'Game' objects for decorative purposes.							]]

-- Used to increment the game level after clearing the scene. This function ends by starting a new level.
-- For each level increment, the game is sped up. Even levels award a bonus life.
function Game:OnLevelFinished()
	-- Increment level
	if (Settings.Level%2 == 0) then Settings:AddLife(); end -- Add life only on even levels.
	Settings:AddLevel();
	
	-- Speed Up Game
	if (Settings.Level <= Settings.MaximumLevel) then 
		Game:SpeedUp();
	else
		Settings.Level = Settings.MaximumLevel;
	end

	-- Start new level
	self:Start();
end

-- Used to reset the game state when all lives are lost.
function Settings:Reset()
	Game:SlowDown();
	
	self.Level = 1; 						-- Used to keep track of the current level.
	self.Lives = 3;							-- Used to keep track of remaining lives.
	self.Score = 0;							-- Used to keep track of player score.
	self.MaximumLevel = 10;					-- Used to indicate the maximum game level.
	self.Delay = 0.5;						-- Delay before the ball is set in motion (in seconds).
	
	self.SpeedIncrement = 0.125;			-- Ball/Paddle speed increment per level.
	self.ColorMultiplier = 0.75;			-- Multiplier to adjust the brick color intensity.
	
	Game:OnUIUpdate();						-- Force UI Update.
end

-- Used to generate a random arrangement of bricks.
-- The health for each brick is assigned randomly to a value between 1 and the current level.
-- The color for each brick is assigned based on its Health and by using the BrickColors table.
function Game:Start()
	-- Clear up scene
	self:Clear();
	
	-- Set up Paddle and Ball
	local SpeedIncrement = Settings.TotalSpeedIncrement;
	self:AddBall(0, -1.75, 4+SpeedIncrement, Settings.Delay);
	self:AddPaddle(0, -2.25, 6+SpeedIncrement, SpeedIncrement+1);
	
	-- Generate random level	
	local Columns = math.random(Settings.Level/2+1, Settings.Level+2);
	local Rows = Settings.Level+2;
	
	for j=Rows/2-1,-Rows/2+1,-1 do
		local Increment = 1;
		Columns = Columns -1;
		if (Columns < 1) then Increment = -1; end
		for i=-Columns/2,Columns/2,Increment do
			local Health = math.random(1, Settings.Level/2+j);
			if (Health < 1) then Health = 1; elseif (Health > 10) then Health = 10; end
			local Brick = Game:AddBrick(i*0.6125, j*0.33+1.0, Health);
			Brick.Color = Color.__new(BrickColors[Health][1]*Settings.ColorMultiplier, BrickColors[Health][2]*Settings.ColorMultiplier, BrickColors[Health][3]*Settings.ColorMultiplier);
		end
	end
end

--[[ Event Handlers 																									]]
--[[ The following event handlers are fully implemented in Lua. They are defined as members of the 'Game' object for 	]]
--[[ decorative purposes. Events are registered within the Unity scene and then invoked in Unity when appropriate. 		]]

-- Called when the ball hits the bottom boundary.
-- Decreases lives by one and resets the game state when the last ball hits the bottom of the screen.
function Game:OnBoundaryHit(Ball)
	if (self.NumBalls >= 2) then
	-- Remove extra balls 
		self:RemoveBall(Ball);
	else
		Settings:AddLife(-1);
		
		-- Reset game state if all lives were used
		if (Settings.Lives < 0) then
			self:OnSceneLoaded();
		else
		-- Reset Paddle/Ball positions
			self:ResetPositions();
		end
	end
end

-- Called when a brick is destroyed. Chance to generate an extra ball.
-- Removes brick from the scene and increments player score. Restarts level if all bricks are destroyed.
function Game:OnBrickDestroyed(Brick)
	-- Update score
	Settings:AddScore(10*Settings.Level);
	
	-- Extra ball chance
	local Chance = Brick.MaxHealth/Settings.MaximumLevel;
	
	local Roll = math.random(0, 100);
	if (Roll/100 <= Chance) then
		self:AddBall(0, -1.75, 4+Settings.TotalSpeedIncrement, Settings.Delay);
	end

	-- Remove Brick
	self:RemoveBrick(Brick);
	
	-- Check if the level is finished
	if (self.NumBricks == 0) then
		self:OnLevelFinished();
	end
end

-- Called when a brick is hit.
-- Damages the block and updates its color. Also increments player score.
function Game:OnBrickHit(Brick)
	-- Update score
	Settings:AddScore(10*Settings.Level);

	-- Change brick color
	local Health = Brick.Health-1;
	if (Health > 0) then 
		Brick.Color = Color.__new(BrickColors[Health][1]*Settings.ColorMultiplier, BrickColors[Health][2]*Settings.ColorMultiplier, BrickColors[Health][3]*Settings.ColorMultiplier);
	end

	Brick:Damage(1);
end

-- Resets settings and starts the game.
function Game:OnSceneLoaded()
	Settings:Reset();
	self:Start();
end

-- Updates all UIText objects.
function Game:OnUIUpdate()
	LevelText.Message = "LEVEL: " .. Settings.Level;
	LivesText.Message = "LIVES: " .. Settings.Lives;
	ScoreText.Message = "SCORE: " .. Settings.Score;
end

-- The following commands register event handlers for different events invoked within Unity.
RegisterEventHandler("BoundaryHit", "OnBoundaryHit", Game);			-- Callback for BoundaryHit event.
RegisterEventHandler("BrickDestroyed", "OnBrickDestroyed", Game);	-- Callback for BrickDestroyed event.
RegisterEventHandler("BrickHit", "OnBrickHit", Game);				-- Callback for BrickHit event.
RegisterEventHandler("SceneLoaded", "OnSceneLoaded", Game);			-- Callback for SceneLoad event.
RegisterEventHandler("UIUpdate", "OnUIUpdate", Game);				-- Callback for UIUpdate event.
```


