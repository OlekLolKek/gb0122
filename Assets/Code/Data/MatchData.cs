using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = nameof(MatchData), menuName = "Data/" + nameof(MatchData))]
public sealed class MatchData : ScriptableObject
{
    [field: SerializeField] public float TimeToLoad { get; private set; }
    [field: SerializeField] public float MatchStartCountdown { get; private set; }
    [field: SerializeField] public float MatchLength { get; private set; }
    [field: SerializeField] public float MatchEndCountdown { get; private set; }
    [field: SerializeField] public int MainMenuSceneIndex { get; private set; }
}