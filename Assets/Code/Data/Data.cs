using System.IO;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Data/Data")]
public sealed class Data : ScriptableObject, IData
{
    [SerializeField] private string _dataRootPath;
    [SerializeField] private string _playerDataPath;
    [SerializeField] private string _cameraDataPath;
    [SerializeField] private string _inputDataPath;
    [SerializeField] private string _matchDataPath;
    [SerializeField] private string _botDataPath;

    private CameraData _cameraData;
    private PlayerData _playerData;
    private InputData _inputData;
    private MatchData _matchData;
    private BotData _botData;
        
        
    public PlayerData PlayerData
    {
        get
        {
            if (_playerData == null)
            {
                _playerData = Load<PlayerData>(_dataRootPath + _playerDataPath);
            }

            return _playerData;
        }
    }

    public CameraData CameraData
    {
        get
        {
            if (_cameraData == null)
            {
                _cameraData = Load<CameraData>(_dataRootPath + _cameraDataPath);
            }

            return _cameraData;
        }
    }

    public InputData InputData
    {
        get
        {
            if (_inputData == null)
            {
                _inputData = Load<InputData>(_dataRootPath + _inputDataPath);
            }

            return _inputData;
        }
    }
    
    public MatchData MatchData
    {
        get
        {
            if (_matchData == null)
            {
                _matchData = Load<MatchData>(_dataRootPath + _matchDataPath);
            }

            return _matchData;
        }
    }
    
    public BotData BotData
    {
        get
        {
            if (_botData == null)
            {
                _botData = Load<BotData>(_dataRootPath + _botDataPath);
            }

            return _botData;
        }
    }

    private T Load<T>(string resourcesPath) where T : Object
    {
        return Resources.Load<T>(Path.ChangeExtension(resourcesPath, null));
    }
}