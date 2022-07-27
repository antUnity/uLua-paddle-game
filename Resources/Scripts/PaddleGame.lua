--[[ August 2022 																										]]
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

--[[ Settings 																											]]

Settings.MaximumLevel = 10;					-- Used to indicate the maximum game level.
Settings.Delay = 0.5;						-- Delay before the ball is set in motion (in seconds).

Settings.SpeedIncrement = 0.125;			-- Ball/Paddle speed increment per level.
Settings.ColorMultiplier = 0.75;			-- Multiplier to adjust the brick color intensity.

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
			self:Reset();
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

-- The following commands register event handlers for different events invoked within Unity.
RegisterEventHandler("BoundaryHit", "OnBoundaryHit", Game);			-- Callback for BoundaryHit event.
RegisterEventHandler("BrickDestroyed", "OnBrickDestroyed", Game);	-- Callback for BrickDestroyed event.
RegisterEventHandler("BrickHit", "OnBrickHit", Game);				-- Callback for BrickHit event.