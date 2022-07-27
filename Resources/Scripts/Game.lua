-- SaveData
Game.SaveData = {
	["HighScore"] = 0
}

-- Placeholder Start function.
function Game:Start() end

-- Resets settings and starts the game.
function Game:Reset()
	self:SlowDown();
	Settings:Reset();
	self:Start();
end

-- Loads high score and starts the game.
function Game:OnSceneLoaded()
	Settings.HighScore = self.SaveData["HighScore"];

	Settings:Reset();
	self:Start();
end

-- Updates high score save data
function Game:OnExit()
	self.SaveData["HighScore"] = Settings.HighScore;
end

-- Updates all UIText objects.
function Game:OnUIUpdate()
	LevelText.Message = "LEVEL: " .. Settings.Level;
	LivesText.Message = "LIVES: " .. Settings.Lives;
	ScoreText.Message = "SCORE: " .. Settings.Score;
	HighScoreText.Message = "HIGH SCORE: " .. Settings.HighScore;
end