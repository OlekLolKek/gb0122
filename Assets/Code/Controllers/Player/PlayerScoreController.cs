public sealed class PlayerScoreController : ICleanable
{
    private readonly PlayerView _playerView;
    private readonly HudView _hudView;
    
    public PlayerScoreController(PlayerView playerView, HudView hudView)
    {
        _playerView = playerView;
        _hudView = hudView;
        
        _playerView.OnUpdatedScore += UpdateScore;
    }

    private void UpdateScore(int kills, int deaths, int score)
    {
        _hudView.SetKills(kills);
        _hudView.SetDeaths(deaths);
        _hudView.SetScore(score);
    }

    public void Cleanup()
    {
        _playerView.OnUpdatedScore -= UpdateScore;
    }
}