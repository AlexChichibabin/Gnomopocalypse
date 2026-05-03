public interface IPlayerProgress
{
	LevelConfig GetNextLevelConfig();
	void Init();
	void AddScore(LevelConfig config, int score);
}